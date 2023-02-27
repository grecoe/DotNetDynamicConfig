namespace DynamicConfiguration.models
{

    class MasterConfig
    {
        #region Existing properties of OepRpConfig
        public DataPartitionDeploymentConfig DataPartitionDeploymentConfig { get; set; }

        public MsiDataPlaneConfig MsiDataPlaneConfig { get; set; }
        #endregion

        #region Demo
        public ExampleSettings ExampleSettings {get; set;}
        #endregion

        #region New SkuDefinitions object
        public SkuDefinitions SkuDefinitions { get; set; }
        #endregion
    }
}
