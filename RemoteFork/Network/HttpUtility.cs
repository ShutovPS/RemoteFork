using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Text;
using Common.Logging;
using RemoteFork.Properties;

namespace RemoteFork.Network {
    public static class HttpUtility {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HttpUtility));

        private static readonly CookieContainer CookieContainer = new CookieContainer();
        private static bool _clearCookies;

        public static string GetRequest(string link, Dictionary<string, string> header = null, bool verbose = false) {
            try {
                _clearCookies = false;
                if (header != null) {
                    foreach (var entry in header) {
                        Log.Info(entry.ToString());
                        if (entry.Key == "Cookie") {
                            _clearCookies = true;
                            CookieContainer.SetCookies(new Uri(link), entry.Value.Replace(";", ","));
                        }
                    }
                }
                using (var handler = new HttpClientHandler()) {
                    SetHandler(handler);
                    using (var httpClient = new HttpClient(handler)) {
                        AddHeader(httpClient, header);
                        var response = httpClient.GetAsync(link).Result;
                        if (_clearCookies) {
                            var cookies = handler.CookieContainer.GetCookies(new Uri(link));
                            foreach (Cookie co in cookies) {
                                co.Expires = DateTime.Now.Subtract(TimeSpan.FromDays(1));
                            }
                        }
                        Log.Info("return get headers=" + verbose);
                        //return response.ToString();
                        if (verbose) {
                            return string.Format("{0}\n{1}", response.Headers, ReadContext(response.Content));
                        } else {
                            return ReadContext(response.Content);
                        }
                    }
                }
            } catch (Exception ex) {
                Log.Error(m => m("HttpUtility->GetRequest: {0}", ex.Message));
                return ex.Message;
            }
        }

        public static string PostRequest(string link, Dictionary<string, string> data,
                                         Dictionary<string, string> header = null, bool verbose = false) {
            try {
                _clearCookies = false;
                if (header != null) {
                    foreach (var entry in header) {
                        Log.Info(entry.ToString());
                        if (entry.Key == "Cookie") {
                            _clearCookies = true;
                            CookieContainer.SetCookies(new Uri(link), entry.Value.Replace(";", ","));
                        }
                    }
                }

                using (var handler = new HttpClientHandler()) {
                    SetHandler(handler);
                    using (var httpClient = new HttpClient(handler)) {
                        AddHeader(httpClient, header);

                        HttpContent content = new FormUrlEncodedContent(data);

                        var response = httpClient.PostAsync(link, content).Result;
                        if (_clearCookies) {
                            var cookies = handler.CookieContainer.GetCookies(new Uri(link));
                            foreach (Cookie co in cookies) {
                                co.Expires = DateTime.Now.Subtract(TimeSpan.FromDays(1));
                            }
                        }
                        Log.Info("return post headers=" + verbose);
                        if (verbose) {
                            return string.Format("{0}\n{1}", response.Headers, ReadContext(response.Content));
                        } else {
                            return ReadContext(response.Content);
                        }
                    }
                }
            } catch (Exception ex) {
                Log.Error(m => m("HttpUtility->PostRequest: {0}", ex.Message));
                return ex.Message;
            }
        }

        private static void SetHandler(HttpClientHandler handler) {
            handler.CookieContainer = CookieContainer;
            handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
        }

        private static void AddHeader(HttpClient httpClient, Dictionary<string, string> header) {
            if (header != null) {
                foreach (var h in header) {
                    try {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation(h.Key, h.Value);
                    } catch (Exception ex) {
                        Log.Debug(m => m("HttpUtility->AddHeader: {0}", ex.Message));
                    }
                }
            } else if (!httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(Settings.Default.UserAgent)) {
                Log.Debug(m => m("HttpUtility->AddUserAgent: {0}", Settings.Default.UserAgent));
            }
        }

        private static string ReadContext(HttpContent context) {
            try {
                return context.ReadAsStringAsync().Result;
            } catch (Exception e) {
                Log.Info("charset=" + context.Headers.ContentType.CharSet);
                var result = context.ReadAsByteArrayAsync().Result;
                try {
                    var encoding = Encoding.Default;
                    result = Encoding.Convert(encoding, Encoding.Default, result);
                } catch {
                    try {
                        var encoding = Encoding.GetEncoding(context.Headers.ContentType.CharSet);
                        result = Encoding.Convert(encoding, Encoding.Default, result);
                    } catch {
                        try {
                            var encoding = Encoding.UTF8;
                            result = Encoding.Convert(encoding, Encoding.Default, result);
                        } catch {
                            try {
                                var encoding = Encoding.ASCII;
                                result = Encoding.Convert(encoding, Encoding.Default, result);
                            } catch {
                                try {
                                    var encoding = Encoding.Unicode;
                                    result = Encoding.Convert(encoding, Encoding.Default, result);
                                } catch (Exception ex) {
                                    Log.Error(m => m("HttpUtility->ReadContext: {0}", ex.Message));
                                }
                            }
                        }
                    }
                }
                return Encoding.Default.GetString(result);
            }
        }

        public static string QueryParametersToString(NameValueCollection query) {
            if ((query == null) || (query.Count == 0)) {
                return string.Empty;
            }

            var sb = new StringBuilder();

            for (var i = 0; i < query.Count; i++) {
                var key = System.Web.HttpUtility.UrlEncode(query.GetKey(i));
                var keyPrefix = key != null ? key + "=" : string.Empty;
                var values = query.GetValues(i);

                if (sb.Length > 0) {
                    sb.Append('&');
                }

                if ((values == null) || (values.Length == 0)) {
                    sb.Append(keyPrefix);
                } else if (values.Length == 1) {
                    sb.Append(keyPrefix).Append(System.Web.HttpUtility.UrlEncode(values[0]));
                } else {
                    for (var j = 0; j < values.Length; j++) {
                        if (j > 0) {
                            sb.Append('&');
                        }

                        sb.Append(keyPrefix).Append(System.Web.HttpUtility.UrlEncode(values[j]));
                    }
                }
            }

            return sb.ToString();
        }
    }
}