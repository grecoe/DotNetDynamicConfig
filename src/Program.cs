using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DynamicConfiguration.models;

namespace DynamicConfiguration
{
    class Program
    {
        static void SummarizeChanges<TProp>(SkuConfiguration config) where TProp : class
        {
            IDictionary<string, object> partition_changes = config.GetModifiedValues<TProp>();
            Console.WriteLine(
                String.Format("Config {0}: {1} updates - {2}",
                    config.SkuName,
                    typeof(TProp).Name,
                    partition_changes.Count
                    )
                );
        }

        static void ShowCurrentValues<TProp>(IDictionary<string, object> field_changes, TProp original_property)
            where TProp: class
        {
            Console.WriteLine(
                String.Format(
                    "\nShowing current values in {0}",
                    original_property.GetType().Name
                    )
                );

            foreach (KeyValuePair<string, object> kvp_change in field_changes)
            {
#nullable enable
                PropertyInfo? property = original_property.GetType().GetProperty(kvp_change.Key);
#nullable disable
                if (property != null)
                {
                    string message = String.Format(
                        "The current value of {0} in {1} is - {2}",
                        kvp_change.Key,
                        original_property.GetType().Name,
                        property.GetValue(original_property)
                        );
                    Console.WriteLine(message);
                }
            }
        }

        static void ImplementChanges<TProp>(SkuConfiguration sku_config, MasterConfig master_config) where TProp : class
        {
            IDictionary<string, object> field_changes = sku_config.GetModifiedValues<TProp>();

            TProp original_property = SkuConfiguration.GetPropertyFromObject<TProp>(master_config);

            Program.ShowCurrentValues<TProp>(field_changes, original_property);
            Console.WriteLine("\nUpdating original master_config");
            sku_config.ModifyParentObject<TProp, MasterConfig>(master_config);
            Program.ShowCurrentValues<TProp>(field_changes, original_property);
        }

        static void Main()
        {
            // Load configuration which has new SkuConfiguration object within it.
            string text = System.IO.File.ReadAllText(@"C:\....\dotnetdynamicconfig\src\master_config.json");
            MasterConfig master_config = JsonConvert.DeserializeObject<MasterConfig>(text);

            // ------- WEB/WORKER changes only
            // Uncomment these linkes to just overwrite the developer sku changes to the master config. This
            // Is all that would be needed for actual changes
            /*
            SkuConfiguration developer_config2 = master_config.SkuDefinitions.GetSku("developer", "developer");
            developer_config2.UpdateAllSettings<MasterConfig>(master_config);
            */
            // ------- WEB/WORKER changes only

            // Get the standard configuration or use oep_config.SkuDefinitions.GetDefaultConfiguration()
            // In code this will be acquiring the Sku and Tier from the inbound configuration settings.
            SkuConfiguration standard_config = master_config.SkuDefinitions.GetSku("Standard", "Standard");
            SkuConfiguration developer_config = master_config.SkuDefinitions.GetSku("developer", "developer");

            // Show the changes for a specific property type from each SkuConfiguration.
            Console.WriteLine("-----------Summarize Changes -------------------");
            Program.SummarizeChanges<DataPartitionDeploymentConfig>(standard_config);
            Program.SummarizeChanges<DataPartitionDeploymentConfig>(developer_config);

            // Implement the changes on the master configuration and show outputs.
            Console.WriteLine("\n-------Implement Changes on one field------------");
            Program.ImplementChanges<DataPartitionDeploymentConfig>(developer_config, master_config);

            // Better option is to just fully overwrite the master object with ALL changes in the 
            // config. For this, we will only use the one we KNOW has changes in it. 
            Console.WriteLine("\n-----------Reset - Change All-------------------");
            master_config = JsonConvert.DeserializeObject<MasterConfig>(text);

            Console.WriteLine(
                String.Format(
                    "\nCurrent setting for oep_config.DataPartitionDeploymentConfig.MaxDataPartitionCount in master config - {0}\n",
                    master_config.DataPartitionDeploymentConfig.MaxDataPartitionCount
                    )
                );

            developer_config.UpdateAllSettings<MasterConfig>(master_config);

            Console.WriteLine(
                String.Format(
                    "\nCurrent setting for oep_config.DataPartitionDeploymentConfig.MaxDataPartitionCount in master config - {0}",
                    master_config.DataPartitionDeploymentConfig.MaxDataPartitionCount
                    )
                );
        }
    }
}
