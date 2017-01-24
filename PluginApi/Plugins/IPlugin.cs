using PluginApi.Plugins;

namespace RemoteFork.Plugins {
    public interface IPlugin {
        Playlist GetList(IPluginContext context);
    }
}
