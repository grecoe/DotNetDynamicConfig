
namespace DynamicConfiguration.models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;

    class SkuConfiguration
    {
        #region Constant values for value types as a default 
        const int DEFAULT_NUMERIC = -1;
        #endregion

        #region Identity Properties
        /// <summary>
        /// Defines which SKU level these settings apply to
        /// </summary>
        public string SkuName { get; set; }


        /// <summary>
        /// Defines which TIER level these settings apply to
        /// </summary>
        public string SkuTier { get; set; }
        #endregion

        #region Objects defined in the master configuration object that may have overrides
        public DataPartitionDeploymentConfig DataPartitionDeploymentConfig { get; set; }

        public MsiDataPlaneConfig MsiDataPlaneConfig { get; set; }

        #region Demo
        public ExampleSettings ExampleSettings { get; set; }
        #endregion

        #endregion


        public SkuConfiguration()
        {
            // Whenever a new object is added above, you MUST call the SetDefaultProperties on an instance
            // of it to ensure it's defaulted out and values are not picked up by accident for value types.

            this.ExampleSettings = new ExampleSettings();
            SkuConfiguration.SetDefaultProperties<ExampleSettings>(this.ExampleSettings);

            // Default all values of property bags so we if they were set
            this.DataPartitionDeploymentConfig = new DataPartitionDeploymentConfig();
            this.MsiDataPlaneConfig = new MsiDataPlaneConfig();
            SkuConfiguration.SetDefaultProperties<DataPartitionDeploymentConfig>(this.DataPartitionDeploymentConfig);
            SkuConfiguration.SetDefaultProperties<MsiDataPlaneConfig>(this.MsiDataPlaneConfig);
        }

        /// <summary>
        /// Dynamically search for properties within this class a property of the same type 
        /// within the incoming TClass object.
        /// 
        /// When types are matched, settings from the internal object will override the settings of
        /// the property within the TClass object.
        /// </summary>
        /// <typeparam name="TClass">Class type to override settings from the internal objects.</typeparam>
        /// <param name="modifiable_instance">Class to find a matching property based on type to update.</param>
        public void UpdateAllSettings<TClass>(TClass modifiable_instance) where TClass: class
        {
            PropertyInfo[] property_list = this.GetType().GetProperties();

            foreach(PropertyInfo property in property_list)
            {
                if( property.PropertyType.Name!= "String")
                {
                    Type[] generic_types = new Type[] { property.PropertyType, typeof(TClass)};
                    MethodInfo modify_parent_object_method = typeof(SkuConfiguration).GetMethod("ModifyParentObject");
                    MethodInfo invokable_method = modify_parent_object_method.MakeGenericMethod(generic_types);

                    invokable_method.Invoke(this, new[] { modifiable_instance});
                }
            }
        }

        /// <summary>
        /// Update an incoming object property which has the same type as an internal property to this class. 
        /// 
        /// For example, update OepRpConfig settings for DataPartitionDeploymentConfig from the internal settings
        /// on this class instance.
        /// </summary>
        /// <typeparam name="TProp">Class type of the internal object to update, such as DataPartitionDeploymentConfig</typeparam>
        /// <typeparam name="TClass">Class with a property matching the type of TProp which will be updated.</typeparam>
        /// <param name="modifiable_instance">An opbect with a property of type TProp in which TProp will be updated 
        /// with any of the settings provided - i.e. non defaulted values.</param>
        public void ModifyParentObject<TProp, TClass>(TClass modifiable_instance) 
            where TProp : class
            where TClass: class
        {
            TProp modifiable_object = SkuConfiguration.GetPropertyFromObject<TProp>(modifiable_instance);

            if( modifiable_object != null)
            {
                this.ModifyObject<TProp>(modifiable_object);
            }
        }

        /// <summary>
        /// Given an instance of TProp, look up internally for any alterations to that object type. 
        /// If found, apply them to the inbound object.
        /// </summary>
        /// <typeparam name="TProp">Type of incoming object</typeparam>
        /// <param name="modifiable_object">Instance of TProp Type to modify</param>
        public void ModifyObject<TProp>(TProp modifiable_object)
             where TProp : class
        {
            IDictionary<string, object> modified_values = this.GetModifiedValues<TProp>();

            if (modified_values.Count > 0 && modifiable_object != null)
            {
                foreach (KeyValuePair<string, object> kvp in modified_values)
                {
#nullable enable
                    PropertyInfo? property = modifiable_object.GetType().GetProperty(kvp.Key);
#nullable disable
                    if (property is not null)
                    {
                        Console.WriteLine(
                            String.Format(
                                "Updating {0} with property {1} - {2}",
                                typeof(TProp).Name,
                                kvp.Key,
                                kvp.Value)
                            );

                        property.SetValue(modifiable_object, kvp.Value);
                    }
                }
            }
        }
  
        /// <summary>
        /// Collect property names and values that have different values than the defaults.
        /// </summary>
        /// <typeparam name="T">Class type of a property to get all modified fields (non default) 
        /// in a property of this class instance.</typeparam>
        /// <returns>Dictionary of property names and values that should be updated.</returns>
        public IDictionary<string, object> GetModifiedValues<T>() where T : class
        {
            T this_property = SkuConfiguration.GetPropertyFromObject<T>(this);

            Dictionary<string, object> return_values = new();

            if (this_property != null)
            {

                Type lookup_type = typeof(T);
                PropertyInfo[] target_property_list = lookup_type.GetProperties();
                FieldInfo[] target_field_info_list = lookup_type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

                // Get all of the changes on standard fields, except bool
                if (this_property != null)
                {
                    // Check the properties
                    foreach (PropertyInfo prop_info in target_property_list)
                    {
                        var value = prop_info.GetValue(this_property);
                        if (prop_info.PropertyType == typeof(int) || prop_info.PropertyType == typeof(float))
                        {
                            if ((int)value != SkuConfiguration.DEFAULT_NUMERIC)
                            {
                                return_values.Add(prop_info.Name, value);
                            }
                        }
                        else if (prop_info.PropertyType == typeof(bool))
                        {
                            continue;
                        }
                        else if (value != null)
                        {
                            return_values.Add(prop_info.Name, value);
                        }
                    }

                    // Find any special fields that are backed (bool)
                    IList<Tuple<string, object>> field_checks = SkuConfiguration.GetPairedFieldsAndProperties<T>(
                        this_property,
                        target_property_list,
                        target_field_info_list
                       );

                    foreach (Tuple<string, object> tuple in field_checks)
                    {
                        return_values.Add(tuple.Item1, tuple.Item2);
                    }

                }
            }

            return return_values;
        }

        #region Static Methods

        /// <summary>
        /// Boolean and potentially other value types will have no distinguishable value
        /// from default to indicating not set. For these fields the class type has to 
        /// create a nullable private member to sit behind the public property which has 
        /// the same name as the property with an underscore prepended. 
        /// 
        /// This call will match up the field and property and return the name of the owning
        /// property and the value from the field for updating.
        /// </summary>
        /// <typeparam name="T">The property type to retrieve data from</typeparam>
        /// <param name="property_owner">Instance of T</param>
        /// <param name="properties">The properties of T</param>
        /// <param name="fields">The Fields of T</param>
        /// <returns>A list of tuples where Item1 is the property name, Item2 is the current stored value
        /// of the matching field.</returns>
        private static IList<Tuple<string, object>> GetPairedFieldsAndProperties<T>(
            T property_owner,
            PropertyInfo[] properties,
            FieldInfo[] fields
            ) where T : class
        {
            List<Tuple<string, object>> pairedFields = new();

            foreach (PropertyInfo prop_info in properties)
            {
                string prop_name = string.Format("_{0}", prop_info.Name);
                IEnumerable<FieldInfo> found_fields = fields
                    .Where(a => a.Name == prop_name)
                    .Select(a => a);

                if (found_fields.Count() > 1)
                {
                    FieldInfo fInfo = found_fields.First();
                    var value = fInfo.GetValue(property_owner);
                    if (value != null)
                    {
                        pairedFields.Add(new Tuple<string, object>(prop_info.Name, value));
                    }
                }
            }
            return pairedFields;
        }


        /// <summary>
        /// Retrieve a property value from an object based on it's type
        /// </summary>
        /// <typeparam name="T">Type of object to look for in the parent object</typeparam>
        /// <param name="parentObject">The object to retrieve the property</param>
        /// <returns>Instnace of T or null if not found</returns>
        public static T GetPropertyFromObject<T>(object parentObject) where T : class
        {
            Type lookup_type = typeof(T);
            PropertyInfo[] property_list = parentObject.GetType().GetProperties();

            T return_property = null;

            // Find the internal object property
            foreach (PropertyInfo local_property in property_list)
            {
                if (local_property.PropertyType == lookup_type)
                {
                    return_property = local_property.GetValue(parentObject) as T;
                    break;
                }
            }

            return return_property;
        }

        /// <summary>
        /// Sets all properties to default values. Nullables are null, numeric fields are 
        /// set to DEFAULT_NUMERIC, and bool is ignored since there is no viable default to 
        /// distinguish between what was there and any default.
        /// </summary>
        /// <typeparam name="T">Class type being passed in</typeparam>
        /// <param name="clsInstance">Instance of T to set all properties/fields to defaults</param>
        private static void SetDefaultProperties<T>(T clsInstance) where T : class
        {
            Type myType = typeof(T);
            PropertyInfo[] prop_info_list = myType.GetProperties();
            FieldInfo[] field_info_list = myType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            // All bools need to be backed by a private member which is bool nullable
            foreach (PropertyInfo prop_info in prop_info_list)
            {
                object set_value = null;

                if (prop_info.PropertyType == typeof(int) || prop_info.PropertyType == typeof(float))
                {
                    set_value = SkuConfiguration.DEFAULT_NUMERIC;
                }
                else if (prop_info.PropertyType == typeof(bool))
                {
                    continue;
                }

                prop_info.SetValue(clsInstance, set_value, null);
            }

            // Collect all private fields, bool cannot be assigned any special value so 
            // we will later search for a property that starts with a private field, which will
            // be a nullable bool which we can use to track for changes.
            foreach (FieldInfo fieldInfo in field_info_list)
            {
                if (fieldInfo.Name.Contains("k__BackingField"))
                {
                    continue;
                }

                fieldInfo.SetValue(clsInstance, null);
            }
        }
        #endregion
    }
}
