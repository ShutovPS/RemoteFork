using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Common.Logging;
using RemoteFork.Forms;
using RemoteFork.Plugins;
using RemoteFork.Server;
using Unosquare.Net;

namespace RemoteFork.Requestes {
    internal class DlnaDirectoryRequestHandler : BaseRequestHandler {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DlnaDirectoryRequestHandler));

        public override void Handle(HttpListenerRequest request, HttpListenerResponse response) {
            var dir = request.QueryString.GetValues(null)?.FirstOrDefault(s => s.StartsWith(Uri.UriSchemeFile));

            if (!string.IsNullOrEmpty(dir)) {
                dir = new Uri(dir).LocalPath;

                var dirs = Directory.GetDirectories(dir);

                var result = new List<Item>();

                foreach (var directory in dirs.Where(DlnaConfigurate.CheckAccess)) {
                    result.Add(CreateDirectoryItem(request, directory));

                    Log.Debug(m => m("Directory: {0}", directory));
                }

                var files = Directory.GetFiles(dir);

                foreach (var file in files.Where(DlnaConfigurate.CheckAccess)) {
                    var fileInfo = new FileInfo(file);

                    result.Add(
                        new Item {
                            Name = $"{fileInfo.Name} ({Tools.FSize(fileInfo.Length)})",
                            Link = CreateUrl(request, RootRequestHandler.RootPath, new NameValueCollection {[null] = new Uri(file).AbsoluteUri}),
                            Type = ItemType.FILE
                        }
                    );

                    Log.Debug(m => m("File: {0}", file));
                }

                WriteResponse(response, ResponseSerializer.ToM3U(result.ToArray()));
            } else {
                Log.Debug(m => m("Directory Not Found: {0}", dir));

                WriteResponse(response, HttpStatusCode.NotFound, $"Directory Not Found: {dir}");
            }
        }

        internal static Item CreateDirectoryItem(HttpListenerRequest request, string directory) {
            var dirInfo = new FileInfo(directory);

            return new Item {
                Name = dirInfo.Name,
                Link = CreateUrl(
                    request,
                    RootRequestHandler.TreePath,
                    new NameValueCollection {[null] = new Uri(directory + Path.DirectorySeparatorChar).AbsoluteUri}
                ),
                Type = ItemType.DIRECTORY
            };
        }
    }
}