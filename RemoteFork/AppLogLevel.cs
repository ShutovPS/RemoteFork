using NLog;

namespace RemoteFork {
    class AppLogLevel {
        private const int None = 0,
                          Info = 1,
                          Error = 2,
                          Debug = 3;

        public static LogLevel FromOrdinal(int ordinal) {
            switch (ordinal) {
                case Info:
                    return LogLevel.Info;
                case Error:
                    return LogLevel.Error;
                case Debug:
                    return LogLevel.Debug;
                default:
                    return LogLevel.Off;
            }
        }
    }
}