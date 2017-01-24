using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using Common.Logging;
using Microsoft.CSharp;
using RemoteFork.Properties;

namespace RemoteFork.Plugins {
    internal class PluginManager {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PluginManager));

        public static readonly PluginManager Instance = new PluginManager();

        private readonly Dictionary<string, PluginInstance> plugins = new Dictionary<string, PluginInstance>();

        private PluginManager() {
            LoadPlugins();
        }

        public static Assembly CompileAssembly(string sourceFile) {
            var codeProvider = new CSharpCodeProvider();

            var compilerParameters = new CompilerParameters {
                GenerateExecutable = false,
                GenerateInMemory = true,
                IncludeDebugInformation = true
            };

            compilerParameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
            compilerParameters.ReferencedAssemblies.Add("System.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Core.dll");

            var result = codeProvider.CompileAssemblyFromFile(compilerParameters, sourceFile);

            var assembly = result.CompiledAssembly;

            return assembly;
        }

        private void LoadPlugins() {
            var pathPlugins = Path.Combine(Environment.CurrentDirectory, "Plugins");

            if (Directory.Exists(pathPlugins)) {
                LoadScripts(pathPlugins);
                LoadAssemblies(pathPlugins);
            }
        }

        private void LoadAssemblies(string pathPlugins) {
            var dir = new DirectoryInfo(pathPlugins);

            foreach (var file in dir.GetFiles("*.dll")) {
                try {
                    LoadAssembly(Assembly.LoadFrom(file.FullName), GetChecksum(file.FullName));
                } catch (Exception e) {
                    Log.Debug(m => m("LoadPlugins->{0}: {1}", file, e));
                }
            }
        }

        private void LoadScripts(string pathPlugins) {
            var files = Directory.GetFiles(pathPlugins, "*.cs");

            foreach (var file in files) {
                try {
                    LoadAssembly(CompileAssembly(file), GetChecksum(file));
                } catch (Exception ex) {
                    Log.Debug(m => m("LoadPlugins->{0}: {1}", file, ex));
                }
            }
        }

        private void LoadAssembly(Assembly assembly, string hash) {
            foreach (var type in assembly.GetExportedTypes()) {
                if (typeof(IPlugin).IsAssignableFrom(type) && (type.IsAbstract == false)) {
                    var attributes = type.GetCustomAttributes(true);

                    if (attributes.Length > 0) {
                        var attribute = (PluginAttribute) attributes.FirstOrDefault(i => i.GetType() == typeof(PluginAttribute));
                        if (attribute != null) {
                            var plugin = new PluginInstance(hash, assembly, type, attribute);
                            if (!plugins.ContainsKey(plugin.Id)) {
                                plugins.Add(plugin.Id, plugin);

                                Log.Debug(m => m(
                                              "Loaded plugin [id: {0}, name: {1}, type: {2}, version: {3}]",
                                              plugin.Id,
                                              plugin.Name,
                                              type.FullName,
                                              plugin.Version
                                          )
                                );
                            }
                        }
                    }
                }
            }
        }

        private static string GetChecksum(string file) {
            using (var stream = File.OpenRead(file)) {
                var sha = new SHA256Managed();
                var checksum = sha.ComputeHash(stream);
                return BitConverter.ToString(checksum).Replace("-", string.Empty);
            }
        }

        public void ReimportPlugins() {
            plugins.Clear();
            LoadPlugins();
        }

        public void RemovePlugin(string name) {
            if (plugins.ContainsKey(name))
                plugins.Remove(name);
        }

        public Dictionary<string, PluginInstance> GetPlugins(bool filtering = true) {
            if (filtering) {
                var dict = new Dictionary<string, PluginInstance>();

                if (Settings.Default.Plugins && (Settings.Default.EnablePlugins != null)) {
#if DEBUG
                    foreach (var plugin in plugins) {
                        dict.Add(plugin.Key, plugin.Value);
                    }
#else
                    foreach (var plugin in plugins.Where(plugin => Settings.Default.EnablePlugins.Contains(plugin.Value.Key))) {
                        dict.Add(plugin.Key, plugin.Value);
                    }
#endif
                }

                return dict;
            }
            return plugins;
        }

        public PluginInstance GetPlugin(string id) {
            if (plugins.ContainsKey(id)) {
                if (Settings.Default.Plugins && (Settings.Default.EnablePlugins != null)) {
#if DEBUG
                    return plugins[id];
#else
                    if (Settings.Default.EnablePlugins.Contains(plugins[id].Key)) {
                        return plugins[id];
                    }
#endif
                }
            }
            return null;
        }
    }
}