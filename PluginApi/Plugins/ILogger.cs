using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginApi.Plugins
{
    public interface ILogger
    {
        void Info(string message);

        void Info(string format, params object[] args);

        void Error(string message);

        void Error(string format, params object[] args);

        void Debug(string message);

        void Debug(string format, params object[] args);

    }
}
