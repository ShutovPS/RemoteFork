using System;

namespace RemoteFork.Plugins {
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginAttribute : Attribute {
        public string Id;
        public string Name;
        public string ImageLink;
        public string Description;
        public string Version;
        public string Author;
    }
}
