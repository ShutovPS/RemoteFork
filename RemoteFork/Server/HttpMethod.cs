using System;
using System.Collections.Concurrent;
using System.Linq;

namespace RemoteFork.Server {
    public enum HttpMethod {
        ANY,
        CONNECT,
        COPY,
        DELETE,
        GET,
        HEAD,
        LINK,
        LOCK,
        OPTIONS,
        PATCH,
        POST,
        PROPFIND,
        PURGE,
        PUT,
        TRACE,
        UNLINK,
        UNLOCK
    };

    public static class HttpMethodExtensions {
        private static readonly ConcurrentDictionary<string, int> Lookup;

        static HttpMethodExtensions() {
            Lookup = new ConcurrentDictionary<string, int>();

            foreach (var val in Enum.GetValues(typeof(HttpMethod)).Cast<HttpMethod>()) {
                var key = val.ToString();
                Lookup[key] = (int) val;
            }
        }

        public static bool IsEquivalent(this HttpMethod httpMethod, HttpMethod other) {
            return httpMethod == HttpMethod.ANY || other == HttpMethod.ANY || httpMethod == other;
        }

        public static HttpMethod FromString(this HttpMethod httpMethod, string method) {
            var ucMethod = method.ToUpper();
            return (Lookup.ContainsKey(ucMethod)) ? (HttpMethod) Lookup[ucMethod] : 0;
        }
    }
}