namespace DynamicConfiguration.models
{
    using Newtonsoft.Json;

    class MsiDataPlaneConfig
    {
        [JsonIgnore]
        private bool? _CMEKEnabled;

        [JsonIgnore]
        private bool? _CMEKForCosmosDBEnabled;

        [JsonIgnore]
        private bool? _CMEKForStorageAccountEnabled;

        /// <summary>
        /// Gets or Sets Flag whether keyvault secrets are required.
        /// </summary>
        public bool CMEKEnabled
        {
            get
            {
                return this._CMEKEnabled ?? false;
            }
            set
            {
                this._CMEKEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether CMEK for cosmos db account is enabled.
        /// </summary>
        public bool CMEKForCosmosDBEnabled
        {
            get
            {
                return this._CMEKForCosmosDBEnabled ?? false;
            }
            set
            {
                this._CMEKForCosmosDBEnabled = value;
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether CMEK for storage account is enabled.
        /// </summary>
        public bool CMEKForStorageAccountEnabled
        {
            get
            {
                return this._CMEKForStorageAccountEnabled ?? false;
            }
            set
            {
                this._CMEKForStorageAccountEnabled = value;
            }
        }


        /// <summary>
        /// Gets or Sets the ApiVersion to communicate with MSI data plane.
        /// </summary>
        public string ApiVersion { get; set; }

        /// <summary>
        /// Gets or Sets The AAD client id for the Managed identity.
        /// </summary>
        public string ClientID { get; set; }

        /// <summary>
        /// Gets or Sets the base64 encoded private key X509 certificate for the Managed identity.
        /// </summary>
        public string ClientSecretName { get; set; }

        /// <summary>
        /// Gets or Sets the MSI Service RP Audience endpoint.
        /// </summary>
        public string ServiceAudienceEndpoint { get; set; }

        /// <summary>
        /// Gets or Sets the MSI Service Tenant Id.
        /// </summary>
        public string MIRPTenantId { get; set; }

        /// <summary>
        /// Gets or Sets the MSI resource uri endpoint.
        /// </summary>
        public string MIResourceEndpoint { get; set; }

        /// <summary>
        /// Gets or Sets the AppId or RP.
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Gets or Sets the certificate name containing client secret
        /// </summary>
        public string MIClientCertName { get; set; }

        /// <summary>
        /// Gets or Sets Delay In Retries in Seconds.
        /// </summary>
        public int DelayInRetriesSeconds { get; set; }

        /// <summary>
        /// Gets or Sets the ARM Control plane uri endpoint used for provisioning calls.
        /// </summary>
        public string ARMCPEndpoint { get; set; }

        /// <summary>
        /// Gets or Sets the sts endpoint used for MIRP Control plane management calls .
        /// </summary>
        public string STSEndpoint { get; set; }

    }
}

