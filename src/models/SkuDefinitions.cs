namespace DynamicConfiguration.models
{
    using System.Collections.Generic;
    using System.Linq;

    class SkuDefinitions
    {
        #region JSON Properties
        /// <summary>
        /// Default sku to use if one is not already present.
        /// </summary>
        public string DefaultSkuConfiguration { get; set; }
        /// <summary>
        /// Default tier to use if one is not already present.
        /// </summary>
        public string DefaultTierConfiguration { get; set; }

        /// <summary>
        /// List of SkuConfiguration objects for all valid Sku/Tier settings.
        /// </summary>
        public IList<SkuConfiguration> SkuConfigurations { get; set; }
        public IDictionary<string, IDictionary<string, SkuConfiguration>> SkuConfigurations2 { get; set; }

        #endregion JSON Properties

        public SkuDefinitions()
        {
            this.SkuConfigurations = new List<SkuConfiguration>();
        }

        /// <summary>
        /// Retrieve a configuration object based on the default Sku/Tier settings.
        /// </summary>
        /// <returns>SkuConfiguration object matching default sku/tier.</returns>
        public SkuConfiguration GetDefaultConfiguration()
        {
            SkuConfiguration returnConfig = this.GetSku(this.DefaultSkuConfiguration, this.DefaultTierConfiguration);

            if(returnConfig == null)
            {
                returnConfig = new SkuConfiguration();
            }

            return returnConfig;
        }

        /// <summary>
        /// Retrieve a configuration object based on sku/tier
        /// </summary>
        /// <param name="sku">Requested Sku</param>
        /// <param name="tier">Requested Tier</param>
        /// <returns>SkuConfiguration object matching incoming parameters. If a match cannot be 
        /// made throws an ArgumentException.</returns>
        public SkuConfiguration GetSku(string sku, string tier)
        {
            IEnumerable<SkuConfiguration> configurations = this.SkuConfigurations
                .Where(a => a.SkuName?.ToLower() == sku.ToLower() && a.SkuTier.ToLower() == tier.ToLower())
                .Select(a => a);

            return configurations.FirstOrDefault();
        }
    }
}
