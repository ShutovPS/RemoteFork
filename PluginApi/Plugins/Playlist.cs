using System.Collections.Specialized;
using RemoteFork.Plugins;

namespace PluginApi.Plugins
{
    public class Playlist
    {
        public static readonly Playlist EmptyPlaylist = new Playlist
        {
            Items = new Item[0]
        };
        
        public string NextPageUrl { get; set; }
        public Item[] Items { get; set; }
    }
}
