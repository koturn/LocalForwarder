using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using LocalForwarder.Json;
using Renci.SshNet;


namespace LocalForwarder
{
    /// <summary>
    /// Entry point class.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// An entry point of this program.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>Status code.</returns>
        public static int Main(string[] args)
        {
            var sshClientList = new List<SshClient>();
            try
            {
                string[] jsonFiles;
                if (args.Length > 0)
                {
                    jsonFiles = args;
                }
                else
                {
                    jsonFiles = new[] { Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.json") };
                }

                var proxyConfigList = new List<ProxyConfig>();
                foreach (var jsonFile in jsonFiles)
                {
                    var config = ConfigRoot.LoadFromJsonFile(jsonFile);
                    proxyConfigList.AddRange(config.ProxyConfigList);
                }

                var isContinue = true;
                do
                {
                    sshClientList = StartLocalForward(proxyConfigList);
                    Console.WriteLine("Enter \"exit\" or EOF (Ctrl-Z) to terminate ssh connection and local forwardings, \"retry\" to reconnect.");
                    do
                    {
                        var s = Console.ReadLine();
                        if (s == null)
                        {
                            isContinue = false;
                            break;
                        }

                        s = s.Trim();
                        if (s == "exit" || s == "quit")
                        {
                            isContinue = false;
                            break;
                        }

                        if (s == "retry")
                        {
                            break;
                        }
                    } while (true);

                    DisposeAllSshClients(sshClientList);
                }
                while (isContinue);

                return 0;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex);
                Console.ResetColor();

                return 1;
            }
            finally
            {
                DisposeAllSshClients(sshClientList);
            }
        }

        /// <summary>
        /// Start local forward.
        /// </summary>
        /// <param name="proxyConfigList">Proxy configuration.</param>
        /// <returns><see cref="List{T}"/> of connected <see cref="SshClient"/> instance.</returns>
        private static List<SshClient> StartLocalForward(List<ProxyConfig> proxyConfigList)
        {
            var sshClientList = new List<SshClient>();
            foreach (var proxyConfig in proxyConfigList.Where(proxyConfig => proxyConfig.Enabled))
            {
                if (proxyConfig.UserName == null)
                {
                    Console.Write("Enter username for {0}:{1}> ", proxyConfig.Host, proxyConfig.Port);
                    Console.Out.Flush();
                    proxyConfig.UserName = Console.ReadLine();
                }

                ConnectionInfo connInfo;
                if (proxyConfig.PrivateKeyPath == null)
                {
                    connInfo = new PasswordConnectionInfo(proxyConfig.Host, proxyConfig.Port, proxyConfig.UserName, proxyConfig.Password);
                }
                else
                {
                    if (proxyConfig.Password == null)
                    {
                        Console.Write("Enter password for {0}:{1}> ", proxyConfig.Host, proxyConfig.Port);
                        Console.Out.Flush();
                        proxyConfig.Password = ConsoleEx.ReadPassword();
                    }
                    connInfo = new PrivateKeyConnectionInfo(proxyConfig.Host, proxyConfig.Port, proxyConfig.UserName, new PrivateKeyFile(proxyConfig.PrivateKeyPath, proxyConfig.PassPhrase));
                }

                // Try to connect and to start local forward.
                try
                {
                    Console.Write("Try to connect to ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("{0}:{1}", connInfo.Host, connInfo.Port);
                    Console.ResetColor();
                    Console.WriteLine("... ; {0}", proxyConfig.Description);

                    var sshClient = new SshClient(connInfo);
                    if (proxyConfig.ServerAliveInterval != -1)
                    {
                        sshClient.KeepAliveInterval = TimeSpan.FromSeconds(proxyConfig.ServerAliveInterval);
                    }
                    sshClient.Connect();
                    sshClientList.Add(sshClient);

                    Console.Write("Try to connect to ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("{0}:{1}", connInfo.Host, connInfo.Port);
                    Console.ResetColor();
                    Console.WriteLine("... Done; {0}", proxyConfig.Description);

                    foreach (var localForwardConfig in proxyConfig.LocalForwardList.Where(localForwardConfig => localForwardConfig.Enabled))
                    {
                        if (localForwardConfig.LocalPort == 0)
                        {
                            localForwardConfig.LocalPort = GetAvailablePort();
                        }
                        var fpl = new ForwardedPortLocal(
                            localForwardConfig.LocalHost,
                            (uint)localForwardConfig.LocalPort,
                            localForwardConfig.RemoteHost,
                            (uint)localForwardConfig.RemotePort);
                        sshClient.AddForwardedPort(fpl);
                        fpl.Start();

                        Console.Write("  Local forward started: ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("{0}:{1}", fpl.BoundHost, fpl.BoundPort);
                        Console.ResetColor();
                        Console.Write(" --> ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("{0}:{1}", fpl.Host, fpl.Port);
                        Console.ResetColor();
                        Console.Write(" (via ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("{0}:{1}", connInfo.Host, connInfo.Port);
                        Console.ResetColor();
                        Console.WriteLine("); {0}", localForwardConfig.Description);
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Error.WriteLine(ex.Message);
                    Console.Error.WriteLine(ex);
                    Console.ResetColor();
                }
            }
            return sshClientList;
        }

        /// <summary>
        /// Get free port number in specified range.
        /// </summary>
        /// <param name="startPort">Minimum port number for the search range.</param>
        /// <param name="endPort">Maximum port number for the search range.</param>
        /// <returns>Available port number if found, otherwise -1.</returns>
        private static int GetAvailablePort(int startPort = 49152, int endPort = 65535)
        {
            var usedPortSet = GetUsedPortSet(startPort, endPort);
            for (var port = startPort; port <= 65535; port++)
            {
                if (!usedPortSet.Contains(port))
                {
                    return port;
                }
            }

            return -1;
        }

        /// <summary>
        /// Get <see cref="HashSet{T}"/> of used port number in specified range.
        /// </summary>
        /// <param name="startPort">Minimum port number for the search range.</param>
        /// <param name="endPort">Maximum port number for the search range.</param>
        /// <returns><see cref="HashSet{T}"/> of used port number.</returns>
        private static HashSet<int> GetUsedPortSet(int startPort = 1, int endPort = 65535)
        {
            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            var usedPortSet = new HashSet<int>();
            foreach (var activeTcpConnection in ipGlobalProperties.GetActiveTcpConnections())
            {
                var port = activeTcpConnection.LocalEndPoint.Port;
                if (startPort <= port && port <= endPort)
                {
                    usedPortSet.Add(port);
                }
            }
            foreach (var activeTcpListener in ipGlobalProperties.GetActiveTcpListeners())
            {
                var port = activeTcpListener.Port;
                if (startPort <= port && port <= endPort)
                {
                    usedPortSet.Add(port);
                }
            }
            foreach (var activeUdpListener in ipGlobalProperties.GetActiveUdpListeners())
            {
                var port = activeUdpListener.Port;
                if (startPort <= port && port <= endPort)
                {
                    usedPortSet.Add(port);
                }
            }

            return usedPortSet;
        }

        /// <summary>
        /// Dispose all <see cref="SshClient"/> instance in the list.
        /// </summary>
        /// <param name="sshClientList"><see cref="List{T}"/> of <see cref="SshClient"/> instance.</param>
        private static void DisposeAllSshClients(List<SshClient> sshClientList)
        {
            foreach (var sshClient in sshClientList)
            {
                foreach (var fp in sshClient.ForwardedPorts)
                {
                    fp.Dispose();
                }
                sshClient.Dispose();
            }
            sshClientList.Clear();
        }
    }
}
