using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;


namespace LocalForwarder.Json
{
    /// <summary>
    /// Root json object of "config.json".
    /// </summary>
    [JsonObject]
    internal class ConfigRoot
    {
        /// <summary>
        /// <see cref="List{T}"/> of <see cref="ProxyConfig"/> instance.
        /// </summary>
        [JsonProperty("proxy_config")]
        public List<ProxyConfig> ProxyConfigList { get; set; }

        /// <summary>
        /// Create instance from specified json file.
        /// </summary>
        /// <param name="filePath">File path to json file.</param>
        /// <returns>Created <see cref="ConfigRoot"/> instance.</returns>
        /// <exception cref="FileNotFoundException">Thrown when specified file is not found.</exception>
        /// <exception cref="ArgumentNullException">Thrown when instance is not created.</exception>
        public static ConfigRoot LoadFromJsonFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found: " + filePath);
            }
            var configJson = JsonConvert.DeserializeObject<ConfigRoot>(File.ReadAllText(filePath));
            if (configJson == null)
            {
                throw new NullReferenceException("configJson");
            }
            return configJson;
        }
    }
}
