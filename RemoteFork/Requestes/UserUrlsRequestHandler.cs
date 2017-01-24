using System.Collections.Generic;
using System.Linq;
using RemoteFork.Plugins;
using RemoteFork.Properties;
using RemoteFork.Server;
using Unosquare.Net;

namespace RemoteFork.Requestes {
    internal class UserUrlsRequestHandler : BaseRequestHandler {
        internal static readonly string ParamUrls = "urls.m3u";

        public override void Handle(HttpListenerRequest request, HttpListenerResponse response) {
            var result = new List<Item>();

            if ((Settings.Default.UserUrls != null) && (Settings.Default.UserUrls.Count > 0)) {
                foreach (var url in Settings.Default.UserUrls) {
                    result.Add(
                        new Item {
                            Name = url.Split('\\').Last().Split('/').Last(),
                            Link = url,
                            Type = ItemType.FILE
                        }
                    );
                }
            }

            WriteResponse(response, ResponseSerializer.ToM3U(result.ToArray()));
        }
    }
}