using System;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using RemoteFork.Log;
using RemoteFork.Network;
using RemoteFork.Server;
using RemoteFork.Settings;
using RemoteFork.Updater;

namespace RemoteFork {
    public class Program {
        private static Server server;

        private static readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private static readonly CancellationToken _cancellationToken = _cancellationTokenSource.Token;

        public static void Main(string[] args) {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            if (ProgramSettings.Settings.CheckUpdate) {
                UpdateController.AddUpdate("RemoteFork", new GithubProvider("RemoteFork", "ShutovPS/RemoteFork/RemoteFork", true),
                    AssemblyVersionChecker.GetInstalledVersionNumber());
                Task.Run(CheckForUpdate, _cancellationToken);
            }

            #region Command Line
            if (args != null && args.Length > 0)
            {
                switch (args[0])
                {
                    #region Docker CE
                    case "docker":
                        {
                            ProgramSettings.Settings.IpAddress = args[1]; // Внешний IP для плейлистов ttv, etc. 
                            server = new Server("0.0.0.0", ProgramSettings.Settings.Port);
                            server.Start();
                            _cancellationTokenSource.Cancel();
                            return;
                        }
                    #endregion
                }
            }
            #endregion
                
            server = new Server(ProgramSettings.Settings.IpAddress, ProgramSettings.Settings.Port);

            server.Start();
            _cancellationTokenSource.Cancel();
        }

        private static async Task CheckForUpdate() {
            while (true) {
                await UpdateController.CheckUpdates();
                Thread.Sleep(360000);
            }
        }

        internal class Server {
            private static readonly Logger Log = new Logger(typeof(Server));

            private IWebHost _webHost;

            private readonly string _ip;
            private readonly ushort _port;

            internal Server(string ip, ushort port) {
                this._ip = ip;
                this._port = port;
            }

            internal void Start() {
                var builder = WebHost.CreateDefaultBuilder()
                    //.UseEnvironment("Development")
                    .UseStartup<Startup>()
                    .UseKestrel()
#if DEBUG
                    .UseContentRoot(Environment.CurrentDirectory);
#else
                .UseContentRoot(AppDomain.CurrentDomain.BaseDirectory);
#endif

                if (ProgramSettings.Settings.UseProxy) {
                    HTTPUtility.CreateProxy(ProgramSettings.Settings.ProxyAddress, ProgramSettings.Settings.ProxyPort,
                        ProgramSettings.Settings.ProxyUserName, ProgramSettings.Settings.ProxyPassword);
                } else {
                    HTTPUtility.CreateProxy();
                }

                ServerRegistration();
                try {
                    var listen = new StringBuilder();
                    listen.Append($"http://{_ip}:{_port}");
                    if (ProgramSettings.Settings.ListenLocalhost) {
                        if (_ip != "0.0.0.0" && _ip != "127.0.0.1" && _ip != "localhost") {
                            listen.Append($";http://localhost:{_port}");
                        }
                    }

                    _webHost = builder
                        .UseUrls(listen.ToString())
                        .Build();

                    _webHost.Run();

                    _webHost.Dispose();
                } catch (Exception exception) {
                    Log.LogError(exception);
                }
            }

            private void ServerRegistration() {
                Task.Run(() => {
                    string url =
                        $"http://getlist2.obovse.ru/remote/index.php?v={Assembly.GetExecutingAssembly().GetName().Version}&do=list&localip={_ip}:{_port}&proxy=false";
                    HTTPUtility.GetRequest(url);
                });
            }

            internal void Stop() {
                _webHost?.StopAsync().Wait();
            }
        }
    }
}
