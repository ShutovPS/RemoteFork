namespace RemoteFork.Forms {
    partial class Main {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.IP = new System.Windows.Forms.Label();
            this.cbDlna = new System.Windows.Forms.CheckBox();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.devicesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.loadPlaylistToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tHVPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playUrlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.bStartServer = new System.Windows.Forms.Button();
            this.bStopServer = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbIp = new System.Windows.Forms.ComboBox();
            this.cbAutoStart = new System.Windows.Forms.CheckBox();
            this.llDlnaConfigurate = new System.Windows.Forms.LinkLabel();
            this.cbPlugins = new System.Windows.Forms.CheckBox();
            this.llPluginsConfigurate = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.cbLogs = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbUserAgent = new System.Windows.Forms.TextBox();
            this.thvpAutoStart = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // IP
            // 
            this.IP.AutoSize = true;
            this.IP.Location = new System.Drawing.Point(3, 22);
            this.IP.Name = "IP";
            this.IP.Size = new System.Drawing.Size(20, 13);
            this.IP.TabIndex = 16;
            this.IP.Text = "IP:";
            // 
            // cbDlna
            // 
            this.cbDlna.AutoSize = true;
            this.cbDlna.Checked = true;
            this.cbDlna.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDlna.Location = new System.Drawing.Point(12, 169);
            this.cbDlna.Name = "cbDlna";
            this.cbDlna.Size = new System.Drawing.Size(94, 17);
            this.cbDlna.TabIndex = 5;
            this.cbDlna.Text = "DLNA сервер";
            this.cbDlna.UseVisualStyleBackColor = true;
            this.cbDlna.CheckedChanged += new System.EventHandler(this.cbDlna_CheckedChanged);
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(43, 45);
            this.tbPort.Name = "tbPort";
            this.tbPort.ReadOnly = true;
            this.tbPort.Size = new System.Drawing.Size(145, 20);
            this.tbPort.TabIndex = 4;
            this.tbPort.TabStop = false;
            this.tbPort.Text = "8027";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.exitToolStripMenuItem.Text = "Выход";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.settingsToolStripMenuItem.Text = "Настройки";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // openTestToolStripMenuItem
            // 
            this.openTestToolStripMenuItem.Name = "openTestToolStripMenuItem";
            this.openTestToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.openTestToolStripMenuItem.Text = "Открыть тест";
            this.openTestToolStripMenuItem.Click += new System.EventHandler(this.openTestToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Port:";
            // 
            // devicesToolStripMenuItem1
            // 
            this.devicesToolStripMenuItem1.Name = "devicesToolStripMenuItem1";
            this.devicesToolStripMenuItem1.Size = new System.Drawing.Size(206, 22);
            this.devicesToolStripMenuItem1.Text = "Нет активных устройств";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadPlaylistToolStripMenuItem1,
            this.pluginsToolStripMenuItem,
            this.tHVPToolStripMenuItem,
            this.playUrlToolStripMenuItem,
            this.openTestToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(221, 180);
            // 
            // loadPlaylistToolStripMenuItem1
            // 
            this.loadPlaylistToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.devicesToolStripMenuItem1});
            this.loadPlaylistToolStripMenuItem1.Name = "loadPlaylistToolStripMenuItem1";
            this.loadPlaylistToolStripMenuItem1.Size = new System.Drawing.Size(220, 22);
            this.loadPlaylistToolStripMenuItem1.Text = "Загрузить плейлист";
            this.loadPlaylistToolStripMenuItem1.DropDownOpening += new System.EventHandler(this.loadPlaylistToolStripMenuItem1_DropDownOpening);
            // 
            // pluginsToolStripMenuItem
            // 
            this.pluginsToolStripMenuItem.Name = "pluginsToolStripMenuItem";
            this.pluginsToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.pluginsToolStripMenuItem.Text = "Плагины";
            this.pluginsToolStripMenuItem.DropDownOpening += new System.EventHandler(this.pluginsToolStripMenuItem_DropDownOpening);
            // 
            // tHVPToolStripMenuItem
            // 
            this.tHVPToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem,
            this.gotoToolStripMenuItem});
            this.tHVPToolStripMenuItem.Name = "tHVPToolStripMenuItem";
            this.tHVPToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.tHVPToolStripMenuItem.Text = "THVP BitTorrent ";
            this.tHVPToolStripMenuItem.Click += new System.EventHandler(this.tHVPToolStripMenuItem_Click);
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.runToolStripMenuItem.Text = "Запуск THVP";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // gotoToolStripMenuItem
            // 
            this.gotoToolStripMenuItem.Name = "gotoToolStripMenuItem";
            this.gotoToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.gotoToolStripMenuItem.Text = "http://thvp.ru/about";
            this.gotoToolStripMenuItem.Click += new System.EventHandler(this.gotoToolStripMenuItem_Click);
            // 
            // playUrlToolStripMenuItem
            // 
            this.playUrlToolStripMenuItem.Name = "playUrlToolStripMenuItem";
            this.playUrlToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.playUrlToolStripMenuItem.Text = "Пользовательские ссылки";
            this.playUrlToolStripMenuItem.Click += new System.EventHandler(this.playUrlToolStripMenuItem_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "RemoteFork";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "playlist.m3u";
            this.openFileDialog1.Filter = "Playlist (*.m3u)|*.m3u|All files (*.*)|*.*";
            // 
            // bStartServer
            // 
            this.bStartServer.Location = new System.Drawing.Point(12, 12);
            this.bStartServer.Name = "bStartServer";
            this.bStartServer.Size = new System.Drawing.Size(94, 23);
            this.bStartServer.TabIndex = 0;
            this.bStartServer.Text = "Запустить";
            this.bStartServer.UseVisualStyleBackColor = true;
            this.bStartServer.Click += new System.EventHandler(this.bStartServer_Click);
            // 
            // bStopServer
            // 
            this.bStopServer.Enabled = false;
            this.bStopServer.Location = new System.Drawing.Point(112, 12);
            this.bStopServer.Name = "bStopServer";
            this.bStopServer.Size = new System.Drawing.Size(94, 23);
            this.bStopServer.TabIndex = 1;
            this.bStopServer.Text = "Остановить";
            this.bStopServer.UseVisualStyleBackColor = true;
            this.bStopServer.Click += new System.EventHandler(this.bStopServer_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 267);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(217, 22);
            this.statusStrip1.TabIndex = 19;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(114, 17);
            this.toolStripStatusLabel1.Text = "Сервер остановлен";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbIp);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.IP);
            this.groupBox1.Controls.Add(this.tbPort);
            this.groupBox1.Location = new System.Drawing.Point(12, 84);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(194, 79);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "IP:PORT";
            // 
            // cbIp
            // 
            this.cbIp.FormattingEnabled = true;
            this.cbIp.Location = new System.Drawing.Point(43, 19);
            this.cbIp.Name = "cbIp";
            this.cbIp.Size = new System.Drawing.Size(145, 21);
            this.cbIp.TabIndex = 18;
            this.cbIp.SelectedIndexChanged += new System.EventHandler(this.cbIp_SelectedIndexChanged);
            // 
            // cbAutoStart
            // 
            this.cbAutoStart.AutoSize = true;
            this.cbAutoStart.Checked = true;
            this.cbAutoStart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoStart.Location = new System.Drawing.Point(12, 41);
            this.cbAutoStart.Name = "cbAutoStart";
            this.cbAutoStart.Size = new System.Drawing.Size(143, 17);
            this.cbAutoStart.TabIndex = 5;
            this.cbAutoStart.Text = "Автостарт при запуске";
            this.cbAutoStart.UseVisualStyleBackColor = true;
            this.cbAutoStart.CheckedChanged += new System.EventHandler(this.cbAutoStart_CheckedChanged);
            // 
            // llDlnaConfigurate
            // 
            this.llDlnaConfigurate.AutoSize = true;
            this.llDlnaConfigurate.Location = new System.Drawing.Point(145, 170);
            this.llDlnaConfigurate.Name = "llDlnaConfigurate";
            this.llDlnaConfigurate.Size = new System.Drawing.Size(61, 13);
            this.llDlnaConfigurate.TabIndex = 21;
            this.llDlnaConfigurate.TabStop = true;
            this.llDlnaConfigurate.Text = "Настроить";
            this.llDlnaConfigurate.VisitedLinkColor = System.Drawing.Color.Blue;
            this.llDlnaConfigurate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llDlnaConfigurate_LinkClicked);
            // 
            // cbPlugins
            // 
            this.cbPlugins.AutoSize = true;
            this.cbPlugins.Checked = true;
            this.cbPlugins.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPlugins.Location = new System.Drawing.Point(12, 192);
            this.cbPlugins.Name = "cbPlugins";
            this.cbPlugins.Size = new System.Drawing.Size(71, 17);
            this.cbPlugins.TabIndex = 5;
            this.cbPlugins.Text = "Плагины";
            this.cbPlugins.UseVisualStyleBackColor = true;
            this.cbPlugins.CheckedChanged += new System.EventHandler(this.cbPlugins_CheckedChanged);
            // 
            // llPluginsConfigurate
            // 
            this.llPluginsConfigurate.AutoSize = true;
            this.llPluginsConfigurate.Location = new System.Drawing.Point(145, 193);
            this.llPluginsConfigurate.Name = "llPluginsConfigurate";
            this.llPluginsConfigurate.Size = new System.Drawing.Size(61, 13);
            this.llPluginsConfigurate.TabIndex = 21;
            this.llPluginsConfigurate.TabStop = true;
            this.llPluginsConfigurate.Text = "Настроить";
            this.llPluginsConfigurate.VisitedLinkColor = System.Drawing.Color.Blue;
            this.llPluginsConfigurate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llPluginsConfigurate_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 215);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Логирование";
            // 
            // cbLogs
            // 
            this.cbLogs.FormattingEnabled = true;
            this.cbLogs.Items.AddRange(new object[] {
            "Выключить",
            "Информация",
            "Ошибки",
            "Отладка"});
            this.cbLogs.Location = new System.Drawing.Point(89, 212);
            this.cbLogs.Name = "cbLogs";
            this.cbLogs.Size = new System.Drawing.Size(111, 21);
            this.cbLogs.TabIndex = 23;
            this.cbLogs.SelectedIndexChanged += new System.EventHandler(this.cbLogs_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 243);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "User-Agent";
            // 
            // tbUserAgent
            // 
            this.tbUserAgent.Location = new System.Drawing.Point(89, 240);
            this.tbUserAgent.Name = "tbUserAgent";
            this.tbUserAgent.Size = new System.Drawing.Size(111, 20);
            this.tbUserAgent.TabIndex = 25;
            this.tbUserAgent.TextChanged += new System.EventHandler(this.tbUserAgent_TextChanged);
            this.tbUserAgent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUserAgent_KeyDown);
            // 
            // thvpAutoStart
            // 
            this.thvpAutoStart.AutoSize = true;
            this.thvpAutoStart.Checked = true;
            this.thvpAutoStart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.thvpAutoStart.Location = new System.Drawing.Point(12, 62);
            this.thvpAutoStart.Name = "thvpAutoStart";
            this.thvpAutoStart.Size = new System.Drawing.Size(159, 17);
            this.thvpAutoStart.TabIndex = 26;
            this.thvpAutoStart.Text = "Автостарт THVP BitTorrent";
            this.thvpAutoStart.UseVisualStyleBackColor = true;
            this.thvpAutoStart.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(217, 275);
            this.Controls.Add(this.thvpAutoStart);
            this.Controls.Add(this.tbUserAgent);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbLogs);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.llPluginsConfigurate);
            this.Controls.Add(this.llDlnaConfigurate);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.cbAutoStart);
            this.Controls.Add(this.cbPlugins);
            this.Controls.Add(this.cbDlna);
            this.Controls.Add(this.bStartServer);
            this.Controls.Add(this.bStopServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RemoteFork";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Resize += new System.EventHandler(this.Main_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label IP;
        private System.Windows.Forms.CheckBox cbDlna;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openTestToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem devicesToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem loadPlaylistToolStripMenuItem1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button bStartServer;
        private System.Windows.Forms.Button bStopServer;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbAutoStart;
        private System.Windows.Forms.ComboBox cbIp;
        private System.Windows.Forms.LinkLabel llDlnaConfigurate;
        private System.Windows.Forms.CheckBox cbPlugins;
        private System.Windows.Forms.LinkLabel llPluginsConfigurate;
        private System.Windows.Forms.ToolStripMenuItem pluginsToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbLogs;
        private System.Windows.Forms.ToolStripMenuItem playUrlToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbUserAgent;
        private System.Windows.Forms.ToolStripMenuItem tHVPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gotoToolStripMenuItem;
        private System.Windows.Forms.CheckBox thvpAutoStart;
    }
}