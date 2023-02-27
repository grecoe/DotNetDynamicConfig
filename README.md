# Dynamic JSON Configuration Override 

- Built using VS 2019 / .NET 5.0 SDK

Dynamically override settings using .NET reflection from a master configuration class that deserializes JSON.

Add in a SkuConfiguration object to your JSON object and to your master configuration object to have access to it.

Then use a SkuDefinitions in your configuration to override your settings. In the below description you can define what you want to override from object1 and object2 in the configurations. Note that in standard below, nothing gets overridden.  

```json
{
    "object1": {...},
    "object1": {...},
    "SkuDefinitions": {
        "DefaultSkuConfiguration": "standard",
        "DefaultTierConfiguration": "standard",
        "SkuConfigurations": [
          {
            "sku": "developer",
            "tier": "developer",
            "object1": {
              only fields you want to override
            },
            "object2" {
              only fields you want to override
            }
          },
          {
            "sku": "standard",
            "tier": "standard" <-- Overrides nothing because no other object definitions
          }
    ]
  }
}
```

Then just use the following code once you have your master config loaded.

```code
string text = System.IO.File.ReadAllText(
        @"C:\....\dotnetdynamicconfig\src\master_config.json"
    );
MasterConfig master_config = JsonConvert.DeserializeObject<MasterConfig>(text);

SkuConfiguration developer_config = master_config.SkuDefinitions.GetSku(
        "developer", 
        "developer"
    );
developer_config.UpdateAllSettings<MasterConfig>(master_config);
```