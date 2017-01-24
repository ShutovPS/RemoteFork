using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Logging;
using RemoteFork.Plugins;
using RemoteFork.Server;
using Unosquare.Net;

namespace RemoteFork.Requestes {
    internal class PluginRequestHandler : BaseRequestHandler {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TestRequestHandler));

        internal static readonly string ParamPluginKey = "plugin";

        internal static readonly Regex PluginParamRegex = new Regex($@"{ParamPluginKey}(\w+)[\\]?", RegexOptions.Compiled);

        public override void Handle(HttpListenerRequest request, HttpListenerResponse response) {
            var pluginKey = ParsePluginKey(request);

            if (!string.IsNullOrEmpty(pluginKey)) {
                var plugin = PluginManager.Instance.GetPlugin(pluginKey);

                if (plugin != null) {
                    Log.Debug(m => m("Execute: {0}", plugin.Name));

                    var pluginResponse = plugin.Instance.GetList(new PluginContext(pluginKey, request, new NameValueCollection(request.QueryString)));

                    if (pluginResponse != null) {
                        WriteResponse(response, ResponseSerializer.ToXml(pluginResponse));
                    } else {
                        Log.Warn(m => m("Plugin Playlist is null. Plugin: {0}", pluginKey));

                        WriteResponse(response, HttpStatusCode.NotFound, $"Plugin Playlist is null. Plugin: {pluginKey}");
                    }
                } else {
                    Log.Warn(m => m("Plugin Not Found. Plugin: {0}", pluginKey));

                    WriteResponse(response, HttpStatusCode.NotFound, $"Plugin Not Found. Plugin: {pluginKey}");
                }
            } else {
                Log.Warn(m => m("Plugin is not defined in request. Plugin: {0}", pluginKey));

                WriteResponse(response, HttpStatusCode.NotFound, $"Plugin is not defined in request. Plugin: {pluginKey}");
            }
        }

        private static string ParsePluginKey(HttpListenerRequest request) {
            var pluginParam = request.QueryString.GetValues(null)?.FirstOrDefault(s => PluginParamRegex.IsMatch(s ?? string.Empty));

            var pluginParamMatch = PluginParamRegex.Match(pluginParam ?? string.Empty);

            return pluginParamMatch.Success ? pluginParamMatch.Groups[1].Value : string.Empty;
        }

        internal static string CreatePluginUrl(HttpListenerRequest request, string pluginName, NameValueCollection parameters = null) {
            var query = new NameValueCollection {
                [null] = string.Concat(ParamPluginKey, pluginName, Path.DirectorySeparatorChar, ".xml")
            };

            if (parameters != null) {
                query.Add(parameters);
            }

            return CreateUrl(request, RootRequestHandler.TreePath, query);
        }
    }
}