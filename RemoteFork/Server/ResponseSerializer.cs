using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using PluginApi.Plugins;
using RemoteFork.Plugins;

namespace RemoteFork.Server {
    internal class ResponseSerializer {
        public static string ToM3U(Item[] items) {
            var sb = new StringBuilder("#EXTM3U");

            foreach (var item in items) {
                sb.AppendFormat(
                    @"{0}#EXTINF:0,{1}{0}{2}",
                    Environment.NewLine,
                    item.Name,
                    item.Link
                );
            }

            return sb.ToString();
        }

        public static string ToJson(Playlist pluginResponse) {
            var json = new JObject();

            if (pluginResponse.Items != null && pluginResponse.Items.Any()) {
                var channels = new JArray();

                foreach (var item in pluginResponse.Items) {
                    var channel = new JObject {
                        ["title"] = item.Name ?? string.Empty,
                        ["description"] = item.Description ?? string.Empty,
                        ["logo_30x30"] = item.ImageLink ?? string.Empty
                    };

                    switch (item.Type) {
                        case ItemType.DIRECTORY:
                            channel["playlist_url"] = item.Link ?? string.Empty;
                            break;
                        case ItemType.FILE:
                            channel["stream_url"] = item.Link ?? string.Empty;
                            break;
                        case ItemType.SEARCH:
                            channel["playlist_url"] = item.Link ?? string.Empty;
                            channel["search_on"] = item.Description ?? string.Empty;
                            break;
                    }

                    channels.Add(channel);
                }

                json["next_page_url"] = pluginResponse.NextPageUrl ?? string.Empty;
                json["channels"] = channels;
            }

            return json.ToString();
        }

        public static string ToXml(Playlist pluginResponse) {
            var items = ItemsToXml(pluginResponse.Items);

            items.AddFirst(new XElement("next_page_url", new XCData(pluginResponse.NextPageUrl ?? string.Empty)));

            return XmlToString(
                new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    items
                )
            );
        }

        public static string ToXml(Item[] items) {
            return XmlToString(
                new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    ItemsToXml(items)
                )
            );
        }

        private static string XmlToString(XDocument doc) {
            var builder = new StringBuilder();
            var settings = new XmlWriterSettings {
                Indent = true
            };

            using (var writer = XmlWriter.Create(builder, settings)) {
                doc.WriteTo(writer);
            }

            return builder.ToString();
        }

        private static XElement ItemsToXml(Item[] items) {
            var xmlItems = new XElement("items");

            if (items != null && items.Any()) {
                foreach (var item in items) {
                    var channel = new XElement("channel");

                    channel.Add(new XElement("title", new XCData(item.Name ?? string.Empty)));
                    channel.Add(new XElement("description", new XCData(item.Description ?? string.Empty)));
                    channel.Add(new XElement("logo_30x30", new XCData(item.ImageLink ?? string.Empty)));

                    switch (item.Type) {
                        case ItemType.DIRECTORY:
                            channel.Add(new XElement("playlist_url", new XCData(item.Link ?? string.Empty)));
                            break;
                        case ItemType.FILE:
                            channel.Add(new XElement("stream_url", new XCData(item.Link ?? string.Empty)));
                            break;
                        case ItemType.SEARCH:
                            channel.Add(new XElement("playlist_url", new XCData(item.Link ?? string.Empty)));
                            channel.Add(new XElement("search_on", new XCData(item.Description ?? string.Empty)));
                            break;
                    }

                    xmlItems.Add(channel);
                }
            }

            return xmlItems;
        }
    }
}