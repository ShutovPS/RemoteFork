using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using Unosquare.Labs.EmbedIO;

namespace RemoteFork.Server {
    internal class HttpServer : IDisposable {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HttpServer));

        private WebServer _webServer;

        private CancellationTokenSource _cts;

        private Task _task;

        public HttpServer(IPAddress ip, int port) {
            Log.Info(m => m("Server start"));

            _cts = new CancellationTokenSource();

            _webServer = WebServer.Create(
                new UriBuilder {
                    Scheme = "http",
                    Host = ip.ToString(),
                    Port = port,
                    Path = "/"
                }.ToString(),
                new EmbedIOLogger()
            );

            _webServer.RegisterModule(new RequestDispatcher());

            _task = _webServer?.RunAsync(_cts.Token);
        }

        ~HttpServer() {
            Dispose(false);
        }

        private void Stop() {
            if (!(_cts?.IsCancellationRequested ?? true)) {
                _cts?.Cancel();

                try {
                    _task?.Wait();
                } catch (AggregateException) {
                    Log.Info(m => m("Server stop"));
                }
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (!disposing) {
                return;
            }

            Stop();

            if (_webServer != null) {
                _webServer.Dispose();
                _webServer = null;
            }

            if (_cts != null) {
                _cts.Dispose();
                _cts = null;
            }

            if (_task != null) {
                _task?.Dispose();
                _task = null;
            }
        }
    }
}