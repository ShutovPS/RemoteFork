using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using RemoteFork.Properties;

namespace RemoteFork.Forms {
    public partial class DlnaConfigurate : Form {
        public DlnaConfigurate() {
            InitializeComponent();

            LoadFilteringType();
            LoadDirectories();
            LoadFileExtensions();
        }

        private void bSave_Click(object sender, EventArgs e) {
            SaveFilteringType();
            SaveDirectories();
            SaveFileExtensions();

            Settings.Default.Save();
        }

        private void bCancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void LoadFilteringType() {
            switch (Settings.Default.DlnaFilterType) {
                case 0:
                    rbAll.Checked = true;
                    break;
                case 1:
                    rbIncludeSelected.Checked = true;
                    break;
                case 2:
                    rbExcludeSelected.Checked = true;
                    break;
            }
        }

        private void LoadDirectories() {
            if ((Settings.Default.DlnaDirectories != null) && (Settings.Default.DlnaDirectories.Count > 0)) {
                foreach (var directory in Settings.Default.DlnaDirectories) lbDirectories.Items.Add(directory);
            }
        }

        private void LoadFileExtensions() {
            if ((Settings.Default.DlnaFileExtensions != null) && (Settings.Default.DlnaFileExtensions.Count > 0)) {
                tbDlnaFileExtensions.Text = string.Join(",", Settings.Default.DlnaFileExtensions.Cast<string>());
            }
        }

        private void SaveFilteringType() {
            if (rbAll.Checked) {
                Settings.Default.DlnaFilterType = 0;
            } else if (rbIncludeSelected.Checked) {
                Settings.Default.DlnaFilterType = 1;
            } else if (rbExcludeSelected.Checked) {
                Settings.Default.DlnaFilterType = 2;
            }
        }

        private void SaveDirectories() {
            var collection = new StringCollection();

            foreach (var item in lbDirectories.Items) {
                collection.Add(item.ToString());
            }

            Settings.Default.DlnaDirectories = collection;
        }

        private void SaveFileExtensions() {
            var collection = new StringCollection();

            foreach (var item in tbDlnaFileExtensions.Text.Split(',')) {
                collection.Add(item.Trim());
            }

            Settings.Default.DlnaFileExtensions = collection;
        }

        private void bClearDirectories_Click(object sender, EventArgs e) {
            lbDirectories.Items.Clear();
        }

        private void bRemoveDirectories_Click(object sender, EventArgs e) {
            if (lbDirectories.SelectedIndices.Count > 0) {
                foreach (int id in lbDirectories.SelectedIndices) {
                    lbDirectories.Items.RemoveAt(id);
                }
            }
        }

        private void bAddDirectorues_Click(object sender, EventArgs e) {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
                if (!lbDirectories.Items.Contains(folderBrowserDialog1.SelectedPath)) {
                    lbDirectories.Items.Add(folderBrowserDialog1.SelectedPath);
                }
            }
        }

        public static bool CheckAccess(string file) {
            var result = true;

            file = Path.GetFullPath(file);

            if (Settings.Default.DlnaDirectories != null) {
                var filter = new List<string>(Settings.Default.DlnaDirectories.Cast<string>());
                switch (Settings.Default.DlnaFilterType) {
                    case 1:
                        if (filter.All(i => !file.StartsWith(i))) {
                            result = false;
                        }
                        break;
                    case 2:
                        if (filter.Any(file.StartsWith)) {
                            result = false;
                        }
                        break;
                }
            }

            if (File.Exists(file)) {
                if ((Settings.Default.DlnaFileExtensions != null) && (Settings.Default.DlnaFileExtensions.Count > 0)) {
                    result = Settings.Default.DlnaFileExtensions.Cast<string>().Any(file.EndsWith);
                }
            }

            return result;
        }
    }
}