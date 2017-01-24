using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;
using Common.Logging;
using RemoteFork.Server;
using Unosquare.Labs.EmbedIO;
using Unosquare.Net;

namespace RemoteFork.Requestes {
    internal abstract class BaseRequestHandler : IRequestHandler {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BaseRequestHandler));

        public virtual void Handle(HttpListenerContext context) {
            SetDefaultReponseHeaders(context.Response);

            Handle(context.Request, context.Response);
        }

        public virtual void Handle(HttpListenerRequest request, HttpListenerResponse response) { }

        protected virtual void SetDefaultReponseHeaders(HttpListenerResponse response) {
            response.SendChunked = false;
            response.ContentEncoding = Encoding.UTF8;
            response.ContentType = Constants.DefaultMimeTypes[".html"];
        }

        protected static string GetHostUrl(HttpListenerRequest request) {
            return new UriBuilder {
                Scheme = request.Url.Scheme,
                Host = request.Url.Host,
                Port = request.Url.Port,
                Path = "/",
            }.ToString();
        }

        internal static string CreateUrl(HttpListenerRequest request, string path, NameValueCollection query = null) {
            return new UriBuilder {
                Scheme = request.Url.Scheme,
                Host = request.Url.Host,
                Port = request.Url.Port,
                Path = HttpUtility.UrlPathEncode(path),
                Query = Network.HttpUtility.QueryParametersToString(query)
            }.ToString();
        }

        internal static void WriteResponse(HttpListenerResponse response, string responseText) {
            Log.Debug(m => m("Response: {0}", responseText));

            using (var writer = new StreamWriter(response.OutputStream)) {
                writer.Write(responseText);
            }
        }

        internal static void WriteResponse(HttpListenerResponse response, HttpStatusCode status, string responseText) {
            response.StatusCode = status.ToInteger();

            WriteResponse(response, responseText);
        }
    }
}