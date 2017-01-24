using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginApi.Plugins
{
    public interface IHTTPClient
    {
        string GetRequest(string link, Dictionary<string, string> header = null);

        string PostRequest(string link, Dictionary<string, string> data, Dictionary<string, string> header = null);
    }
}
