using Newtonsoft.Json;
using System.ComponentModel;


namespace LocalForwarder.Json
{
    /// <summary>
    /// Local forward configuration.
    /// </summary>
    internal class LocalForwardConfig
    {
        /// <summary>
        /// Description.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
        /// <summary>
        /// <para>Local host name. (IP addess)</para>
        /// <para>Default value: "127.0.0.1"</para>
        /// </summary>
        [DefaultValue("127.0.0.1")]
        [JsonProperty("local_host")]
        public string LocalHost { get; set; }
        /// <summary>
        /// <para>Local port.</para>
        /// <para>Default value: 22</para>
        /// </summary>
        [DefaultValue(0)]
        [JsonProperty("local_port")]
        public int LocalPort { get; set; }
        /// <summary>
        /// <para>Remote host name. (IP addess)</para>
        /// <para>This parameter is REQUIRED.</para>
        /// </summary>
        [JsonProperty("remote_host")]
        public string RemoteHost { get; set; }
        /// <summary>
        /// <para>Remote port number.</para>
        /// <para>Default value: 22</para>
        /// </summary>
        [DefaultValue(22)]
        [JsonProperty("remote_port")]
        public int RemotePort { get; set; }
        /// <summary>
        /// True if this configuration is enabled.
        /// </summary>
        [DefaultValue(true)]
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }
}
