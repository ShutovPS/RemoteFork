using System;
using System.Reflection;

namespace RemoteFork.Plugins {
    internal class PluginInstance {
        private readonly Assembly assembly;
        public readonly string Author;
        public readonly string Description;
        public readonly string Id;
        public readonly string ImageLink;
        public readonly string Key;
        public readonly string Name;
        private readonly Type type;
        public readonly string Version;

        private IPlugin instance;

        public PluginInstance(string key, Assembly assembly, Type type, PluginAttribute attribute) {
            Key = key;
            Id = attribute.Id;
            Name = attribute.Name;
            Description = attribute.Description;
            ImageLink = attribute.ImageLink;
            Version = attribute.Version;
            Author = attribute.Author;
            this.assembly = assembly;
            this.type = type;
        }

        public IPlugin Instance => instance ?? (instance = assembly.CreateInstance(type.FullName) as IPlugin);

        public override string ToString() {
            return $"{Name} {Version} ({Author})";
        }
    }
}