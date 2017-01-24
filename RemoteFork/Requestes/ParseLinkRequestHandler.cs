using System;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Logging;
using RemoteFork.Network;
using Unosquare.Net;

namespace RemoteFork.Requestes {
    internal class ParseLinkRequestHandler : BaseRequestHandler {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ParseLinkRequestHandler));

        internal static readonly string UrlPath = "/parserlink";

        public override void Handle(HttpListenerRequest request, HttpListenerResponse response) {
            var result = string.Empty;

            var requestStrings = request.RawUrl.Substring(UrlPath.Length + 1).Split('|');

            Log.Debug(m => m("Parsing: {0}", requestStrings[0]));

            var curlResponse = requestStrings[0].StartsWith("curl")
                ? CurlRequest(requestStrings[0])
                : HttpUtility.GetRequest(requestStrings[0]);

            if (requestStrings.Length == 1) {
                result = curlResponse;
            } else {
                if (!requestStrings[1].Contains(".*?")) {
                    if (string.IsNullOrEmpty(requestStrings[1]) && string.IsNullOrEmpty(requestStrings[2])) {
                        result = curlResponse;
                    } else {
                        var num1 = curlResponse.IndexOf(requestStrings[1], StringComparison.Ordinal);
                        if (num1 == -1) {
                            result = string.Empty;
                        } else {
                            num1 += requestStrings[1].Length;
                            var num2 = curlResponse.IndexOf(requestStrings[2], num1, StringComparison.Ordinal);
                            result = num2 == -1 ? string.Empty : curlResponse.Substring(num1, num2 - num1);
                        }
                    }
                } else {
                    Log.Debug(m => m("ParseLinkRequest: {0}", requestStrings[1] + "(.*?)" + requestStrings[2]));

                    var pattern = requestStrings[1] + "(.*?)" + requestStrings[2];
                    var regex = new Regex(pattern, RegexOptions.Multiline);
                    var match = regex.Match(curlResponse);
                    if (match.Success) result = match.Groups[1].Captures[0].ToString();
                }
            }

            WriteResponse(response, result);
        }

        public string CurlRequest(string text) {
            string result; bool verbose = text.IndexOf(" -i", StringComparison.Ordinal) > 0;

            var url = Regex.Match(text, "(?:\")(.*?)(?=\")").Groups[1].Value;
            var matches = Regex.Matches(text, "(?:-H\\s\")(.*?)(?=\")");
            var header = (
                from Match match in matches
                select match.Groups
                into groups
                where groups.Count > 1
                select groups[1].Value
                into value
                where value.Contains(": ")
                select value).ToDictionary(value => value.Remove(value.IndexOf(": ", StringComparison.Ordinal)),
                    value => value.Substring(value.IndexOf(": ", StringComparison.Ordinal) + 2));
            if (text.Contains("--data")) {
                var dataString = Regex.Match(text, "(?:--data\\s\")(.*?)(?=\")").Groups[1].Value;
                var dataArray = dataString.Split('&');
                var data =
                    dataArray.ToDictionary(value => value.Remove(value.IndexOf("=", StringComparison.Ordinal)),
                        value => value.Substring(value.IndexOf("=", StringComparison.Ordinal) + 1));
                result = HttpUtility.PostRequest(url, data, header, verbose);
            } else {
                Log.Info(url);
                result = HttpUtility.GetRequest(url, header, verbose);
            }

            return result;
        }
    }
}