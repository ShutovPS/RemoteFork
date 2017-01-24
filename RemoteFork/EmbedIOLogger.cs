using System;
using Common.Logging;
using Unosquare.Labs.EmbedIO;

namespace RemoteFork {
    public sealed class EmbedIOLogger : Unosquare.Labs.EmbedIO.Log.ILog {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(WebServer));

        public void Info(object message) {
            Logger.Info(message);
        }

        public void Error(object message) {
            Logger.Error(message);
        }

        public void Error(object message, Exception exception) {
            Logger.Error(message, exception);
        }

        public void InfoFormat(string format, params object[] args) {
            Logger.InfoFormat(format, args);
        }

        public void WarnFormat(string format, params object[] args) {
            Logger.WarnFormat(format, args);
        }

        public void ErrorFormat(string format, params object[] args) {
            Logger.ErrorFormat(format, args);
        }

        public void DebugFormat(string format, params object[] args) {
            Logger.DebugFormat(format, args);
        }

        public void Debug(string message) {
            Logger.Debug(message);
        }
    }
}
