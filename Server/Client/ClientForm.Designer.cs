namespace Client
{
    partial class ClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MessageWindow = new System.Windows.Forms.TextBox();
            this.InputField = new System.Windows.Forms.TextBox();
            this.SubmitButtion = new System.Windows.Forms.Button();
            this.DissconnectButtion = new System.Windows.Forms.Button();
            this.ConnectedClients = new System.Windows.Forms.ListBox();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.UserList = new System.Windows.Forms.GroupBox();
            this.NumberConnected = new System.Windows.Forms.Label();
            this.ChatManager = new System.Windows.Forms.TabControl();
            this.NameChange = new System.Windows.Forms.TabPage();
            this.NameLogin = new System.Windows.Forms.Panel();
            this.Tip = new System.Windows.Forms.Label();
            this.NameSend = new System.Windows.Forms.Button();
            this.NmaeTip = new System.Windows.Forms.Label();
            this.GlobalChat = new System.Windows.Forms.TabPage();
            this.Games = new System.Windows.Forms.TabPage();
            this.SlectGamePannle = new System.Windows.Forms.Panel();
            this.GameSelect = new System.Windows.Forms.ComboBox();
            this.LabelGames = new System.Windows.Forms.Label();
            this.JoinGame1 = new System.Windows.Forms.Button();
            this.CreatGame1 = new System.Windows.Forms.Button();
            this.Settings = new System.Windows.Forms.TabPage();
            this.ColourModePanel = new System.Windows.Forms.Panel();
            this.ColourModeSelect = new System.Windows.Forms.ComboBox();
            this.ColourModeLable = new System.Windows.Forms.Label();
            this.SettingsLabel = new System.Windows.Forms.Label();
            this.InputPannle = new System.Windows.Forms.Panel();
            this.UserList.SuspendLayout();
            this.ChatManager.SuspendLayout();
            this.NameChange.SuspendLayout();
            this.NameLogin.SuspendLayout();
            this.GlobalChat.SuspendLayout();
            this.Games.SuspendLayout();
            this.SlectGamePannle.SuspendLayout();
            this.Settings.SuspendLayout();
            this.ColourModePanel.SuspendLayout();
            this.InputPannle.SuspendLayout();
            this.SuspendLayout();
            // 
            // MessageWindow
            // 
            this.MessageWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessageWindow.Location = new System.Drawing.Point(3, 3);
            this.MessageWindow.Multiline = true;
            this.MessageWindow.Name = "MessageWindow";
            this.MessageWindow.ReadOnly = true;
            this.MessageWindow.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.MessageWindow.Size = new System.Drawing.Size(1182, 626);
            this.MessageWindow.TabIndex = 0;
            // 
            // InputField
            // 
            this.InputField.BackColor = System.Drawing.Color.White;
            this.InputField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InputField.Location = new System.Drawing.Point(0, 0);
            this.InputField.Multiline = true;
            this.InputField.Name = "InputField";
            this.InputField.Size = new System.Drawing.Size(1078, 132);
            this.InputField.TabIndex = 1;
            this.InputField.Enter += new System.EventHandler(this.InputField_Enter);
            // 
            // SubmitButtion
            // 
            this.SubmitButtion.Dock = System.Windows.Forms.DockStyle.Right;
            this.SubmitButtion.Location = new System.Drawing.Point(1078, 0);
            this.SubmitButtion.Name = "SubmitButtion";
            this.SubmitButtion.Size = new System.Drawing.Size(118, 132);
            this.SubmitButtion.TabIndex = 2;
            this.SubmitButtion.Text = "Submit";
            this.SubmitButtion.UseVisualStyleBackColor = true;
            this.SubmitButtion.Click += new System.EventHandler(this.SubmitButtion_Click);
            // 
            // DissconnectButtion
            // 
            this.DissconnectButtion.Location = new System.Drawing.Point(479, 140);
            this.DissconnectButtion.Name = "DissconnectButtion";
            this.DissconnectButtion.Size = new System.Drawing.Size(176, 118);
            this.DissconnectButtion.TabIndex = 4;
            this.DissconnectButtion.Text = "Dissconnect";
            this.DissconnectButtion.UseVisualStyleBackColor = true;
            this.DissconnectButtion.Click += new System.EventHandler(this.DissconnectButtion_Click);
            // 
            // ConnectedClients
            // 
            this.ConnectedClients.BackColor = System.Drawing.Color.White;
            this.ConnectedClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ConnectedClients.FormattingEnabled = true;
            this.ConnectedClients.ItemHeight = 20;
            this.ConnectedClients.Location = new System.Drawing.Point(3, 22);
            this.ConnectedClients.Name = "ConnectedClients";
            this.ConnectedClients.Size = new System.Drawing.Size(244, 643);
            this.ConnectedClients.TabIndex = 5;
            this.ConnectedClients.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ConnectedClients_MouseClick);
            // 
            // NameBox
            // 
            this.NameBox.AccessibleName = "NameBox";
            this.NameBox.Location = new System.Drawing.Point(183, 223);
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(607, 26);
            this.NameBox.TabIndex = 6;
            this.NameBox.Text = "Enter Name";
            this.NameBox.Enter += new System.EventHandler(this.NameBoc_Enter);
            this.NameBox.Leave += new System.EventHandler(this.NameBoc_Leave);
            // 
            // UserList
            // 
            this.UserList.BackColor = System.Drawing.Color.White;
            this.UserList.Controls.Add(this.ConnectedClients);
            this.UserList.Controls.Add(this.NumberConnected);
            this.UserList.Dock = System.Windows.Forms.DockStyle.Right;
            this.UserList.Location = new System.Drawing.Point(1196, 0);
            this.UserList.Name = "UserList";
            this.UserList.Size = new System.Drawing.Size(250, 797);
            this.UserList.TabIndex = 8;
            this.UserList.TabStop = false;
            this.UserList.Text = "Connected Clients";
            // 
            // NumberConnected
            // 
            this.NumberConnected.BackColor = System.Drawing.Color.Transparent;
            this.NumberConnected.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.NumberConnected.Enabled = false;
            this.NumberConnected.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NumberConnected.ForeColor = System.Drawing.Color.Black;
            this.NumberConnected.Location = new System.Drawing.Point(3, 665);
            this.NumberConnected.Name = "NumberConnected";
            this.NumberConnected.Size = new System.Drawing.Size(244, 129);
            this.NumberConnected.TabIndex = 6;
            this.NumberConnected.Text = "Number Of users Connected: 0";
            this.NumberConnected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ChatManager
            // 
            this.ChatManager.Controls.Add(this.NameChange);
            this.ChatManager.Controls.Add(this.GlobalChat);
            this.ChatManager.Controls.Add(this.Games);
            this.ChatManager.Controls.Add(this.Settings);
            this.ChatManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChatManager.Location = new System.Drawing.Point(0, 0);
            this.ChatManager.Name = "ChatManager";
            this.ChatManager.SelectedIndex = 0;
            this.ChatManager.Size = new System.Drawing.Size(1196, 665);
            this.ChatManager.TabIndex = 9;
            this.ChatManager.Selected += new System.Windows.Forms.TabControlEventHandler(this.ChatManager_Selected);
            // 
            // NameChange
            // 
            this.NameChange.Controls.Add(this.NameLogin);
            this.NameChange.Location = new System.Drawing.Point(4, 29);
            this.NameChange.Name = "NameChange";
            this.NameChange.Padding = new System.Windows.Forms.Padding(3);
            this.NameChange.Size = new System.Drawing.Size(1188, 632);
            this.NameChange.TabIndex = 3;
            this.NameChange.Text = "Name Select";
            this.NameChange.UseVisualStyleBackColor = true;
            // 
            // NameLogin
            // 
            this.NameLogin.Controls.Add(this.Tip);
            this.NameLogin.Controls.Add(this.NameBox);
            this.NameLogin.Controls.Add(this.NameSend);
            this.NameLogin.Controls.Add(this.NmaeTip);
            this.NameLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NameLogin.Location = new System.Drawing.Point(3, 3);
            this.NameLogin.Name = "NameLogin";
            this.NameLogin.Size = new System.Drawing.Size(1182, 626);
            this.NameLogin.TabIndex = 10;
            // 
            // Tip
            // 
            this.Tip.Location = new System.Drawing.Point(183, 320);
            this.Tip.Name = "Tip";
            this.Tip.Size = new System.Drawing.Size(607, 50);
            this.Tip.TabIndex = 10;
            this.Tip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NameSend
            // 
            this.NameSend.Location = new System.Drawing.Point(183, 255);
            this.NameSend.Name = "NameSend";
            this.NameSend.Size = new System.Drawing.Size(607, 62);
            this.NameSend.TabIndex = 9;
            this.NameSend.Text = "Enter";
            this.NameSend.UseVisualStyleBackColor = true;
            this.NameSend.Click += new System.EventHandler(this.NameSend_Click);
            // 
            // NmaeTip
            // 
            this.NmaeTip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NmaeTip.Location = new System.Drawing.Point(183, 170);
            this.NmaeTip.Name = "NmaeTip";
            this.NmaeTip.Size = new System.Drawing.Size(607, 40);
            this.NmaeTip.TabIndex = 8;
            this.NmaeTip.Text = "Enter Your Name";
            this.NmaeTip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GlobalChat
            // 
            this.GlobalChat.Controls.Add(this.MessageWindow);
            this.GlobalChat.Location = new System.Drawing.Point(4, 29);
            this.GlobalChat.Name = "GlobalChat";
            this.GlobalChat.Padding = new System.Windows.Forms.Padding(3);
            this.GlobalChat.Size = new System.Drawing.Size(1188, 632);
            this.GlobalChat.TabIndex = 0;
            this.GlobalChat.Text = "Global Chat";
            this.GlobalChat.UseVisualStyleBackColor = true;
            // 
            // Games
            // 
            this.Games.Controls.Add(this.SlectGamePannle);
            this.Games.Location = new System.Drawing.Point(4, 29);
            this.Games.Name = "Games";
            this.Games.Padding = new System.Windows.Forms.Padding(3);
            this.Games.Size = new System.Drawing.Size(1188, 632);
            this.Games.TabIndex = 2;
            this.Games.Text = "Games";
            this.Games.UseVisualStyleBackColor = true;
            // 
            // SlectGamePannle
            // 
            this.SlectGamePannle.Controls.Add(this.GameSelect);
            this.SlectGamePannle.Controls.Add(this.LabelGames);
            this.SlectGamePannle.Controls.Add(this.JoinGame1);
            this.SlectGamePannle.Controls.Add(this.CreatGame1);
            this.SlectGamePannle.Location = new System.Drawing.Point(3, 3);
            this.SlectGamePannle.Name = "SlectGamePannle";
            this.SlectGamePannle.Size = new System.Drawing.Size(1024, 593);
            this.SlectGamePannle.TabIndex = 6;
            // 
            // GameSelect
            // 
            this.GameSelect.FormattingEnabled = true;
            this.GameSelect.Location = new System.Drawing.Point(290, 219);
            this.GameSelect.Name = "GameSelect";
            this.GameSelect.Size = new System.Drawing.Size(425, 28);
            this.GameSelect.TabIndex = 4;
            // 
            // LabelGames
            // 
            this.LabelGames.AutoSize = true;
            this.LabelGames.Location = new System.Drawing.Point(440, 196);
            this.LabelGames.Name = "LabelGames";
            this.LabelGames.Size = new System.Drawing.Size(111, 20);
            this.LabelGames.TabIndex = 5;
            this.LabelGames.Text = "Select a game";
            // 
            // JoinGame1
            // 
            this.JoinGame1.Location = new System.Drawing.Point(290, 253);
            this.JoinGame1.Name = "JoinGame1";
            this.JoinGame1.Size = new System.Drawing.Size(205, 56);
            this.JoinGame1.TabIndex = 1;
            this.JoinGame1.Text = "Join";
            this.JoinGame1.UseVisualStyleBackColor = true;
            this.JoinGame1.Click += new System.EventHandler(this.JoinGame1_Click);
            // 
            // CreatGame1
            // 
            this.CreatGame1.Location = new System.Drawing.Point(501, 253);
            this.CreatGame1.Name = "CreatGame1";
            this.CreatGame1.Size = new System.Drawing.Size(214, 56);
            this.CreatGame1.TabIndex = 2;
            this.CreatGame1.Text = "Create";
            this.CreatGame1.UseVisualStyleBackColor = true;
            this.CreatGame1.Click += new System.EventHandler(this.CreatGame1_Click);
            // 
            // Settings
            // 
            this.Settings.BackColor = System.Drawing.Color.Transparent;
            this.Settings.Controls.Add(this.ColourModePanel);
            this.Settings.Controls.Add(this.DissconnectButtion);
            this.Settings.Controls.Add(this.SettingsLabel);
            this.Settings.Location = new System.Drawing.Point(4, 29);
            this.Settings.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.Settings.Name = "Settings";
            this.Settings.Padding = new System.Windows.Forms.Padding(3);
            this.Settings.Size = new System.Drawing.Size(1188, 632);
            this.Settings.TabIndex = 1;
            this.Settings.Text = "Settings";
            // 
            // ColourModePanel
            // 
            this.ColourModePanel.BackColor = System.Drawing.Color.Transparent;
            this.ColourModePanel.Controls.Add(this.ColourModeSelect);
            this.ColourModePanel.Controls.Add(this.ColourModeLable);
            this.ColourModePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ColourModePanel.Location = new System.Drawing.Point(3, 66);
            this.ColourModePanel.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.ColourModePanel.Name = "ColourModePanel";
            this.ColourModePanel.Size = new System.Drawing.Size(1182, 32);
            this.ColourModePanel.TabIndex = 6;
            // 
            // ColourModeSelect
            // 
            this.ColourModeSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ColourModeSelect.FormattingEnabled = true;
            this.ColourModeSelect.Items.AddRange(new object[] {
            "Light",
            "Dark"});
            this.ColourModeSelect.Location = new System.Drawing.Point(528, 0);
            this.ColourModeSelect.Name = "ColourModeSelect";
            this.ColourModeSelect.Size = new System.Drawing.Size(654, 28);
            this.ColourModeSelect.TabIndex = 1;
            this.ColourModeSelect.SelectedIndexChanged += new System.EventHandler(this.ColourModeSelect_SelectedIndexChanged);
            // 
            // ColourModeLable
            // 
            this.ColourModeLable.Dock = System.Windows.Forms.DockStyle.Left;
            this.ColourModeLable.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ColourModeLable.Location = new System.Drawing.Point(0, 0);
            this.ColourModeLable.Name = "ColourModeLable";
            this.ColourModeLable.Size = new System.Drawing.Size(528, 32);
            this.ColourModeLable.TabIndex = 0;
            this.ColourModeLable.Text = "Colour Mode";
            this.ColourModeLable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SettingsLabel
            // 
            this.SettingsLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.SettingsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SettingsLabel.Location = new System.Drawing.Point(3, 3);
            this.SettingsLabel.Name = "SettingsLabel";
            this.SettingsLabel.Size = new System.Drawing.Size(1182, 63);
            this.SettingsLabel.TabIndex = 7;
            this.SettingsLabel.Text = "Settings";
            this.SettingsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // InputPannle
            // 
            this.InputPannle.Controls.Add(this.InputField);
            this.InputPannle.Controls.Add(this.SubmitButtion);
            this.InputPannle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.InputPannle.Location = new System.Drawing.Point(0, 665);
            this.InputPannle.Name = "InputPannle";
            this.InputPannle.Size = new System.Drawing.Size(1196, 132);
            this.InputPannle.TabIndex = 9;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1446, 797);
            this.Controls.Add(this.ChatManager);
            this.Controls.Add(this.InputPannle);
            this.Controls.Add(this.UserList);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "ClientForm";
            this.Text = "Message";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientForm_FormClosing);
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.UserList.ResumeLayout(false);
            this.ChatManager.ResumeLayout(false);
            this.NameChange.ResumeLayout(false);
            this.NameLogin.ResumeLayout(false);
            this.NameLogin.PerformLayout();
            this.GlobalChat.ResumeLayout(false);
            this.GlobalChat.PerformLayout();
            this.Games.ResumeLayout(false);
            this.SlectGamePannle.ResumeLayout(false);
            this.SlectGamePannle.PerformLayout();
            this.Settings.ResumeLayout(false);
            this.ColourModePanel.ResumeLayout(false);
            this.InputPannle.ResumeLayout(false);
            this.InputPannle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox MessageWindow;
        private System.Windows.Forms.TextBox InputField;
        private System.Windows.Forms.Button SubmitButtion;
        private System.Windows.Forms.Button DissconnectButtion;
        private System.Windows.Forms.ListBox ConnectedClients;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.GroupBox UserList;
        private System.Windows.Forms.Label NumberConnected;
        private System.Windows.Forms.TabControl ChatManager;
        private System.Windows.Forms.TabPage GlobalChat;
        private System.Windows.Forms.TabPage Settings;
        private System.Windows.Forms.TabPage Games;
        private System.Windows.Forms.Button CreatGame1;
        private System.Windows.Forms.Button JoinGame1;
        private System.Windows.Forms.ComboBox GameSelect;
        private System.Windows.Forms.Label LabelGames;
        private System.Windows.Forms.TabPage NameChange;
        private System.Windows.Forms.Label NmaeTip;
        private System.Windows.Forms.Panel InputPannle;
        private System.Windows.Forms.Panel SlectGamePannle;
        private System.Windows.Forms.Panel NameLogin;
        private System.Windows.Forms.Button NameSend;
        private System.Windows.Forms.Label Tip;
        private System.Windows.Forms.Panel ColourModePanel;
        private System.Windows.Forms.ComboBox ColourModeSelect;
        private System.Windows.Forms.Label ColourModeLable;
        private System.Windows.Forms.Label SettingsLabel;
    }
}