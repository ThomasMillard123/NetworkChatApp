using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Packets;
namespace Client
{
    public partial class ClientForm : Form
    {
        public delegate void UpdateChatWindowDelegate(string message);

        readonly Client cClient;

        Color cFontColour;
        Color cBackColour;
        bool cisNameSet;
        public ClientForm(Client client)
        {
            InitializeComponent();
            cClient = client;
            cFontColour = new Color();
            cFontColour = Color.Black;
            cBackColour = new Color();
            BackColor = Color.White;
            InputField.ReadOnly = true;
            ColourModeSelect.SelectedIndex=0;
            cisNameSet = false;
        }

        public void UpdateChatWindow(String message)
        {
            if (!cClient.cIsEnd)
            {
                if (MessageWindow.InvokeRequired)
                {
                    Invoke(new Action(() => UpdateChatWindow(message)));
                }
                else
                {
                    MessageWindow.Text += message += Environment.NewLine;
                    MessageWindow.SelectionStart = MessageWindow.Text.Length;
                    MessageWindow.ScrollToCaret();



                }
            }
        }

        public void UpdateChatWindow(string from,string tabName, String message)
        {
            if (!cClient.cIsEnd)
            {
                if (MessageWindow.InvokeRequired)
                {
                    Invoke(new Action(() => UpdateChatWindow(from, tabName,message)));
                }
                else
                {
                    bool tabExists = true;
                    for (int i = 0; i < ChatManager.TabCount; i++)
                    {
                        if (ChatManager.TabPages[i].Name != from)
                        {
                            tabExists = false;
                        }
                        if (ChatManager.TabPages[i].Name == from)
                        {
                            tabExists = true;
                            break;
                        }


                    }

                    if (tabExists == false)
                    {
                        ChatManager.TabPages.Insert(ChatManager.TabPages.IndexOf(Games), NewTab(from, tabName));

                    }

                    //find tab and text box

                    int tabIndex = 0;
                    for (int i = 0; i < ChatManager.TabCount; i++)
                    {

                        if (ChatManager.TabPages[i].Name == from)
                        {
                            tabIndex = i;
                            break;
                        }


                    }

                    foreach (Control ctrl in ChatManager.TabPages[tabIndex].Controls)
                    {
                        if (ctrl.GetType() == typeof(TextBox))
                        {

                            ((TextBox)ctrl).Text += message += Environment.NewLine;
                            ((TextBox)ctrl).SelectionStart = ((TextBox)ctrl).Text.Length;
                            ((TextBox)ctrl).ScrollToCaret();



                        }
                    }
                }
            }
        }

        public void UpdateNameTip(string message)
        {
            if (Tip.InvokeRequired)
            {
                Invoke(new Action(() => UpdateNameTip(message)));
            }
            else
            {

                if (message == "NameSet")
                {
                    
                    InputField.ReadOnly = false;
                    Tip.Text = "";
                    cisNameSet = true;
                    ChatManager.SelectedTab = GlobalChat;
                }
                else
                {
                    Tip.Text = message;
                }

            }
        }


        private void SubmitButtion_Click(object sender, EventArgs e)
        {

            if (cisNameSet)
            {
                Int32.TryParse(ChatManager.SelectedTab.Name, out int a);
                if (ChatManager.SelectedTab.Name == "GlobalChat")
                {

                    ChatMessagePacket chatPacket = new ChatMessagePacket(cClient.EncryptString(InputField.Text));
                    cClient.TCPSendMessage(chatPacket);
                }
                else if (a != 0)
                {
                    GamePacket gamePacket = new GamePacket(cClient.EncryptString(ChatManager.SelectedTab.Text), cClient.EncryptString(ChatManager.SelectedTab.Name), cClient.EncryptString(InputField.Text));
                    cClient.TCPSendMessage(gamePacket);
                }
                else
                {
                    if (ChatManager.SelectedTab.Name == "Settings")
                    {

                    }
                    else
                    {
                        PrivateMessagePacket privateMessage = new PrivateMessagePacket(cClient.EncryptString(ChatManager.SelectedTab.Text), cClient.EncryptString(InputField.Text));
                        cClient.TCPSendMessage(privateMessage);
                    }

                }

                InputField.Clear();
            }
        }

        private void DissconnectButtion_Click(object sender, EventArgs e)
        {
            if (DissconnectButtion.Text == "Dissconnect")
            {
                DisssconnectPacket chatPacket = new DisssconnectPacket(cClient.EncryptString("You have Dissconnected"));
                cClient.TCPSendMessage(chatPacket);
                cClient.Disconnect();
                NameBox.Clear();
                InputField.ReadOnly = true;
                int maxtab = ChatManager.TabCount;
                List<TabPage> TabsToRemove = new List<TabPage>();
                for (int i = 0; i < maxtab; i++)
                {
                    if (ChatManager.TabPages[i].Name == "Settings" || ChatManager.TabPages[i].Name == "NameChange" || ChatManager.TabPages[i].Name == "GlobalChat" || ChatManager.TabPages[i].Name == "Games")
                    {

                    }
                    else
                    {
                        TabsToRemove.Add(ChatManager.TabPages[i]);

                    }

                }

                for (int i = 0; i < TabsToRemove.Count; i++)
                {
                    ChatManager.TabPages.Remove(TabsToRemove[i]);
                }


                DissconnectButtion.Text = "Connect";
            }
            else if (DissconnectButtion.Text == "Connect")
            {
                cClient.Reconnect("127.0.0.1", 4444);
                DissconnectButtion.Text = "Dissconnect";
                ChatManager.SelectedTab = NameChange;
                NameBox.Focus();
            }




        }
        public void UpdateUserListWindow(List<string> message)
        {
            if (ConnectedClients.InvokeRequired)
            {
                Invoke(new Action(() => UpdateUserListWindow(message)));
            }
            else
            {

                ConnectedClients.DataSource = message;

                NumberConnected.Text = "Number Of users Connected: " + message.Count;
            }
        }

        public bool TabExist(string tab)
        {

            for (int i = 0; i < ChatManager.TabCount; i++)
            {

                if (ChatManager.TabPages[i].Name == tab)
                {
                    return true;

                }


            }
            return false;
        }

        private void NameBoc_Leave(object sender, EventArgs e)
        {
            if (NameBox.Text == "")
            {
                NameBox.Text = "Enter Name";

            }

        }

        private void NameBoc_Enter(object sender, EventArgs e)
        {
            if (NameBox.Text == "Enter Name")
            {

                NameBox.Clear();
            }

        }

        private void ConnectedClients_MouseClick(object sender, MouseEventArgs e)
        {
            if (ConnectedClients.Items.Count != 0)
            {
                string selectedItem = ConnectedClients.Items[ConnectedClients.SelectedIndex].ToString();
             
                
                PrivateMessagePacket privateMessage = new PrivateMessagePacket(cClient.EncryptString(selectedItem), cClient.EncryptString("Has started a chat"));
                cClient.TCPSendMessage(privateMessage);
            }
        }

        private void InputField_Enter(object sender, EventArgs e)
        {
            if (NameBox.Text == null || NameBox.Text == "Enter Name")
            {
                InputField.Clear();
                NameBox.Focus();

            }
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            ChatManager.SelectedTab = NameChange;
            NameBox.Focus();
        }

        //privet message tabs
        public TabPage NewTab(string Name, string text)
        {

            TabPage newPage = new TabPage();
            newPage.Name = Name;
            newPage.Text = text;
            //add Privet Message winddow 
            Button button = new Button();
            button.Name = "Exit";
            button.Text = "Close";
            button.Dock = DockStyle.Bottom;
            button.AutoSize = true;
            button.Click += new System.EventHandler(this.Button_Click);
            button.Show();
            newPage.Controls.Add(button);

            TextBox textBox = new TextBox();
            textBox.Name = "PMWin";
            textBox.Multiline = true;
            textBox.Dock = DockStyle.Fill;
            textBox.ReadOnly = true;
            textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            newPage.Controls.Add(textBox);


            return newPage;


        }



        private void JoinGame1_Click(object sender, EventArgs e)
        {

            cClient.TCPSendMessage(new GamePacket(cClient.EncryptString(GameSelect.Text), cClient.EncryptString(GameSelect.Text), cClient.EncryptString("Join")));

        }


        void Button_Click(object sender, EventArgs e)
        {
            LeavePacket LeaveMessage = new LeavePacket(cClient.EncryptString(ChatManager.SelectedTab.Text), cClient.EncryptString( "Has Left"));
            cClient.TCPSendMessage(LeaveMessage);
            ChatManager.SelectedTab.Dispose();


        }

        public void UpdateGameList(List<string> Games)
        {
            if (GameSelect.InvokeRequired)
            {
                Invoke(new Action(() => UpdateGameList(Games)));
            }
            else
            {

                GameSelect.DataSource = Games;

            }
        }



        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            DisssconnectPacket chatPacket = new DisssconnectPacket(cClient.EncryptString("You have Dissconnected"));
            cClient.TCPSendMessage(chatPacket);
            cClient.Disconnect();
        }

        private void CreatGame1_Click(object sender, EventArgs e)
        {
            cClient.TCPSendMessage(new GamePacket(cClient.EncryptString(GameSelect.Text), cClient.EncryptString(GameSelect.Text), cClient.EncryptString( "Start")));
        }

        private void NameSend_Click(object sender, EventArgs e)
        {
            if (NameBox.Text == "Enter Name")
            {
                Tip.Text = "Please enter a name";
            }
            else
            {
                
                SetNamePacket setNamePacket = new SetNamePacket(cClient.EncryptString(NameBox.Text));
                cClient.TCPSendMessage(setNamePacket);


            }
        }

        private void ColourModeSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ColourModeSelect.SelectedIndex == 0)
            {
                cFontColour = Color.Black;
                cBackColour = Color.White;
            }
            else if (ColourModeSelect.SelectedIndex == 1)
            {
                cFontColour = Color.White;
                cBackColour = Color.Black;
            }

            BackColor = cBackColour;
            ForeColor = cFontColour;

            ChatManager.SelectedTab.ForeColor = cFontColour;
            ChatManager.SelectedTab.BackColor = cBackColour;

            foreach (Control ctrl in ChatManager.SelectedTab.Controls)
            {
                ctrl.ForeColor = cFontColour;
                ctrl.BackColor = cBackColour;
            }

            ConnectedClients.ForeColor = cFontColour;
            ConnectedClients.BackColor = cBackColour;
            NumberConnected.ForeColor = Color.Black;
            NumberConnected.BackColor = Color.White;
            InputField.ForeColor = cFontColour;
            InputField.BackColor = cBackColour;

            MessageWindow.ForeColor = cFontColour;
            MessageWindow.BackColor = cBackColour;
        }



        private void ChatManager_Selected(object sender, TabControlEventArgs e)
        {
            if(NameBox.Text==""||NameBox.Text== "Enter Name"|| !cisNameSet)
            {
                if (ChatManager.SelectedTab == GlobalChat || ChatManager.SelectedTab == Games)
                {

                    ChatManager.SelectedTab = NameChange;

                    Tip.Text = "Please enter a name";
                }
            }

            ChatManager.SelectedTab.ForeColor = cFontColour;
            ChatManager.SelectedTab.BackColor = cBackColour;


            foreach (Control ctrl in ChatManager.SelectedTab.Controls)
            {
                ctrl.ForeColor = cFontColour;
                ctrl.BackColor = cBackColour;
            }
        }

 
    }
}
