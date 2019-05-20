using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Seed.Environment.Engine.Configuration
{
    public class EnginesSettingsSources : IEnginesSettingsSources
    {
        private readonly string _tenants;

        public EnginesSettingsSources(IOptions<EngineOptions> engineOptions)
        {
            _tenants = Path.Combine(engineOptions.Value.ApplicationDataPath, "tenants.json");
        }

        public void AddSources(IConfigurationBuilder builder)
        {
            builder.AddJsonFile(_tenants, optional: true);
        }

        public void Save(string tenant, IDictionary<string, string> data)
        {
            lock (this)
            {
                var tenantsSettings = !File.Exists(_tenants) ? new JObject()
                : JObject.Parse(File.ReadAllText(_tenants));

                var settings = tenantsSettings.GetValue(tenant) as JObject ?? new JObject();

                foreach (var key in data.Keys)
                {
                    if (data[key] != null)
                    {
                        settings[key] = data[key];
                    }
                    else
                    {
                        settings.Remove(key);
                    }
                }

                tenantsSettings[tenant] = settings;
                File.WriteAllText(_tenants, tenantsSettings.ToString());
            }
        }
    }
}