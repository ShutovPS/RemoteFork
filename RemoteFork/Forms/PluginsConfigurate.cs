using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;
using RemoteFork.Plugins;
using RemoteFork.Properties;

namespace RemoteFork.Forms {
    public partial class PluginsConfigurate : Form {
        public PluginsConfigurate() {
            InitializeComponent();

            var plugins = PluginManager.Instance.GetPlugins(false);

            foreach (var plugin in plugins) {
                clbPlugins.Items.Add(
                              plugin.Value,
                              Settings.Default.EnablePlugins != null
                              && Settings.Default.EnablePlugins.Contains(plugin.Value.Key)
                          );
            }
        }

        private void bSave_Click(object sender, EventArgs e) {
            var collection = new StringCollection();

            foreach (var item in clbPlugins.CheckedItems.Cast<PluginInstance>()) {
                collection.Add(item.Key);
            }

            Settings.Default.EnablePlugins = collection;

            Settings.Default.Save();
        }

        private void bCancel_Click(object sender, EventArgs e) {
            Close();
        }
    }
}