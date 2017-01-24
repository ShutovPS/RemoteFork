using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Logging;
using RemoteFork.Properties;
using RemoteFork.Requestes;
using Unosquare.Labs.EmbedIO;
using Unosquare.Net;

namespace RemoteFork.Server {
    internal class RequestDispatcher : WebModuleBase {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestDispatcher));

        private static readonly List<Route> Routes = new List<Route> {
            new Route {
                CanHandle = c => string.Equals(c.RequestPath(), TestRequestHandler.UrlPath, StringComparison.OrdinalIgnoreCase),
                Handler = new TestRequestHandler()
            },
            new Route {
                CanHandle = c => Settings.Default.Dlna
                                 && string.Equals(c.RequestPath(), RootRequestHandler.TreePath, StringComparison.OrdinalIgnoreCase)
                                 && c.InQueryString(null)
                                 && c.Request.QueryString.GetValues(null)?.FirstOrDefault(s => string.Equals(s, RootRequestHandler.RootPath, StringComparison.OrdinalIgnoreCase)) != null,
                Handler = new RootRequestHandler()
            },
            new Route {
                CanHandle = c => Settings.Default.Dlna
                                 && string.Equals(c.RequestPath(), RootRequestHandler.TreePath, StringComparison.OrdinalIgnoreCase)
                                 && c.InQueryString(null)
                                 && c.Request.QueryString.GetValues(null)?.FirstOrDefault(s => s.StartsWith(Uri.UriSchemeFile)) != null,
                Handler = new DlnaDirectoryRequestHandler()
            },
            new Route {
                CanHandle = c => string.Equals(c.RequestPath(), RootRequestHandler.TreePath, StringComparison.OrdinalIgnoreCase)
                                 && c.InQueryString(null)
                                 && c.Request.QueryString.GetValues(null)?.FirstOrDefault(s => string.Equals(s, UserUrlsRequestHandler.ParamUrls, StringComparison.OrdinalIgnoreCase)) != null,
                Handler = new UserUrlsRequestHandler()
            },
            new Route {
                CanHandle = c => string.Equals(c.RequestPath(), RootRequestHandler.TreePath, StringComparison.OrdinalIgnoreCase)
                                 && c.InQueryString(null)
                                 && c.Request.QueryString.GetValues(null)?.FirstOrDefault(s => PluginRequestHandler.PluginParamRegex.IsMatch(s ?? string.Empty)) != null,
                Handler = new PluginRequestHandler()
            },
            new Route {
                CanHandle = c => string.Equals(c.RequestPath(), ParseLinkRequestHandler.UrlPath, StringComparison.OrdinalIgnoreCase),
                Handler = new ParseLinkRequestHandler()
            },
            new Route {
                CanHandle = c => Settings.Default.Dlna
                                 && string.Equals(c.RequestPath(), RootRequestHandler.RootPath, StringComparison.OrdinalIgnoreCase)
                                 && c.InQueryString(null)
                                 && c.Request.QueryString.GetValues(null)?.FirstOrDefault(s => s.StartsWith(Uri.UriSchemeFile)) != null,
                Handler = new DlnaFileRequestHandler()
            }
        };


        public RequestDispatcher() {
            AddHandler(ModuleMap.AnyPath, HttpVerbs.Any, (server, context) => Handle(context));
        }

        private bool Handle(HttpListenerContext context) {
            Log.Debug(m => m("Processing url: {0}", context.Request.RawUrl));

            context.Response.Headers.Add("Server", $"RemoteFork/{Assembly.GetExecutingAssembly().GetName().Version}");

            var route = Routes.FirstOrDefault(r => r.CanHandle(context));

            if (route?.Handler != null) {
                context.Response.Headers.Add(Constants.HeaderAccessControlAllowOrigin);

                context.Response.StatusCode = HttpStatusCode.Ok.ToInteger();

                try {
                    route.Handler.Handle(context);
                } catch (Exception e) {
                    Log.Error(e.Message, e);

                    context.Response.StatusCode = HttpStatusCode.InternalServerError.ToInteger();
                }
            } else {
                Log.Debug(m => m("Resource not found: {0}", context.Request.RawUrl));

                BaseRequestHandler.WriteResponse(context.Response, HttpStatusCode.NotFound, $"Resource not found: {context.Request.RawUrl}");
            }

            return true;
        }

        internal class Route {
            internal Predicate<HttpListenerContext> CanHandle = r => true;

            public IRequestHandler Handler { get; set; }
        }

        public override string Name => "RequestDispatcher";
    }
}