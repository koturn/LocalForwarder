using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LocalForwarder.Json
{
    /// <summary>
    /// Proxy configuration.
    /// </summary>
    internal class ProxyConfig
    {
        /// <summary>
        /// Description.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
        /// <summary>
        /// <para>Host name. (IP address)</para>
        /// <para>This parameter is REQUIRED.</para>
        /// </summary>
        [JsonProperty("host")]
        public string Host { get; set; }
        /// <summary>
        /// <para>SSH port number.</para>
        /// <para>Default value: 22</para>
        /// </summary>
        [DefaultValue(22)]
        [JsonProperty("port")]
        public int Port { get; set; }
        /// <summary>
        /// <para>Private key path.</para>
        /// <para>This parameter is required if <see cref="Password"/> is not specified.</para>
        /// </summary>
        [JsonProperty("privatekey")]
        public string PrivateKeyPath { get; set; }
        /// <summary>
        /// <para>Pass phrase for <see cref="PrivateKeyPath"/>.</para>
        /// <para>This parameter is REQUIRED if private key is generated with pass phrase.</para>
        /// </summary>
        [JsonProperty("passphrase")]
        public string PassPhrase { get; set; }
        /// <summary>
        /// <para>User name.</para>
        /// <para>This parameter is REQUIRED.</para>
        /// </summary>
        [JsonProperty("username")]
        public string UserName { get; set; }
        /// <summary>
        /// <para>Password for ssh connection.</para>
        /// <para>This parameter is required if <see cref="PrivateKeyPath"/> is not specified.</para>
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }
        /// <summary>
        /// <para>Keep alive interval in seconds.</para>
        /// <para>This parameter is arbitrary.</para>
        /// </summary>
        [DefaultValue(-1)]
        [JsonProperty("server_alive_interval")]
        public int ServerAliveInterval { get; set; }
        /// <summary>
        /// <see cref="List{T}"/> of <see cref="LocalForwardConfig"/> instance.
        /// </summary>
        [JsonProperty("local_forward")]
        public List<LocalForwardConfig> LocalForwardList { get; set; }
        /// <summary>
        /// True if this configuration is enabled.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }
}
