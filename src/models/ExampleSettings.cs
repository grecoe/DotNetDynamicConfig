namespace DynamicConfiguration.models
{
    using Newtonsoft.Json;

    class ExampleSettings
    {
        [JsonIgnore]
        private bool? _BooleanValue;

        /// <summary>
        /// Gets or Sets Flag whether keyvault secrets are required.
        /// </summary>
        public bool BooleanValue
        {
            get
            {
                return this._BooleanValue ?? false;
            }
            set
            {
                this._BooleanValue = value;
            }
        }

        public int IntValue { get; set; }

        public string StringValue { get; set; }


    }
}
