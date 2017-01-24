using System;
using System.Windows.Forms;
using RemoteFork.Forms;
using RemoteFork.Properties;

namespace RemoteFork {
    internal static class Program {
        [STAThread]
        private static void Main() {
            NLog.LogManager.GlobalThreshold = AppLogLevel.FromOrdinal(Settings.Default.LogLevel);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var main = new Main {
#if DEBUG
                WindowState = FormWindowState.Normal
#else
                WindowState = FormWindowState.Minimized
#endif
            };
            Application.Run(main);
        }
    }
}