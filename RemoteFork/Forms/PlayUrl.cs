using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;
using RemoteFork.Properties;

namespace RemoteFork.Forms {
    public partial class PlayUrl : Form {
        public PlayUrl() {
            InitializeComponent();
        }

        private void bCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void bPlay_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            var collection = new StringCollection();
            foreach (var line in tbUrls.Lines) {
                collection.Add(line);
            }
            Settings.Default.UserUrls = collection;
            Settings.Default.Save();
        }

        private void PlayUrl_Load(object sender, EventArgs e) {
            if (Settings.Default.UserUrls != null) {
                tbUrls.Text = string.Join(Environment.NewLine, Settings.Default.UserUrls.Cast<string>());
            }
        }
    }
}