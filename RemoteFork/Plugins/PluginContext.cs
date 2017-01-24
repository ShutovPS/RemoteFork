using System.Collections.Generic;
using System.Collections.Specialized;
using Common.Logging;
using PluginApi.Plugins;
using RemoteFork.Network;
using RemoteFork.Requestes;
using Unosquare.Net;

namespace RemoteFork.Plugins {
    internal class PluginContext : IPluginContext {
        private readonly IHTTPClient _httpClient = new HttpClient();

        private readonly ILogger _logger;

        private readonly string _pluginName;
        private readonly HttpListenerRequest _request;
        private readonly NameValueCollection _requestParams;

        public PluginContext(string pluginName, HttpListenerRequest request, NameValueCollection requestParams) {
            _pluginName = pluginName;
            _request = request;
            _requestParams = requestParams;
            _logger = new Logger(pluginName);
        }

        public NameValueCollection GetRequestParams() {
            return _requestParams;
        }

        public string CreatePluginUrl(NameValueCollection parameters) {
            return PluginRequestHandler.CreatePluginUrl(_request, _pluginName, parameters);
        }

        public IHTTPClient GetHttpClient() {
            return _httpClient;
        }

        public ILogger GetLogger() {
            return _logger;
        }

        internal class HttpClient : IHTTPClient {
            public string GetRequest(string link, Dictionary<string, string> header = null) {
                return HttpUtility.GetRequest(link, header);
            }

            public string PostRequest(string link, Dictionary<string, string> data, Dictionary<string, string> header = null) {
                return HttpUtility.PostRequest(link, data, header);
            }
        }

        internal class Logger : ILogger {
            private readonly ILog _log;

            public Logger(string pluginName) {
                _log = LogManager.GetLogger(pluginName);
            }

            public void Info(string message) {
                _log.Info(m => m(message));
            }

            public void Info(string format, params object[] args) {
                _log.Info(m => m(format, args));
            }

            public void Error(string message) {
                _log.Error(m => m(message));
            }

            public void Error(string format, params object[] args) {
                _log.Error(m => m(format, args));
            }

            public void Debug(string message) {
                _log.Debug(m => m(message));
            }

            public void Debug(string format, params object[] args) {
                _log.Debug(m => m(format, args));
            }
        }
    }
}