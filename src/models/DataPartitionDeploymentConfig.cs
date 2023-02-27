namespace DynamicConfiguration.models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    class DataPartitionDeploymentConfig
    {
        [JsonIgnore]
        private bool? _keyvaultSecretsRequired;

        /// <summary>
        /// Gets or Sets Flag whether keyvault secrets are required.
        /// </summary>
        public bool keyvaultSecretsRequired
        {
            get
            {
                return this._keyvaultSecretsRequired ?? false;
            }
            set
            {
                this._keyvaultSecretsRequired = value;
            }
        }

        /// <summary>
        /// Gets or Sets templateSpec resource group name.
        /// </summary>
        public string DataPartitionId { get; set; }

        /// <summary>
        /// Gets or Sets RoleAssignment parameters list.
        /// </summary>
        public Dictionary<string,string> RoleAssignmentParameters { get; set; }


        /// <summary>
        /// Gets or Sets DataPartitionStorageAccountsPrefix.
        /// </summary>
        public Dictionary<string, string> DataPartitionStorageAccountsPrefix { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of data partitions  that can be created per OEP instance.
        /// </summary>
        public int MinDataPartitionCount { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of data partitions  that can be created per OEP instance.
        /// </summary>
        public int MaxDataPartitionCount { get; set; }
    }
}
