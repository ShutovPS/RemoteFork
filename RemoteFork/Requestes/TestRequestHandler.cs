using System.Reflection;
using RemoteFork.Forms;
using Unosquare.Net;

namespace RemoteFork.Requestes {
    internal class TestRequestHandler : BaseRequestHandler {
        internal static readonly string UrlPath = "/test";

        public override void Handle(HttpListenerRequest request, HttpListenerResponse response) {
            if (request.RawUrl.IndexOf('|') > 0) {
                var device = request.RawUrl.Replace("/test?", "");

                if (!Main.Devices.Contains(device)) {
                    Main.Devices.Add(device);
                }
            }

            WriteResponse(response, $"<html><h1>ForkPlayer DLNA Work!</h1><br><b>RemoteFork Server. v. {Assembly.GetExecutingAssembly().GetName().Version}</b></html>");
        }
    }
}