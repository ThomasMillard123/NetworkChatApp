using Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
namespace Client
{
    public class Client
    {

        NetworkStream cStream;
        BinaryWriter cStreamWriter;
        BinaryReader cStreamReader;
        BinaryFormatter cFormatter;
        ClientForm cClientForm;
        TcpClient cTCPClient;
        UdpClient cUDPClient;

        RSACryptoServiceProvider cRSAProvider;
        RSAParameters cPublicKey;
        RSAParameters cPrivateKey;
        RSAParameters cServerKey;

        public bool cIsEnd { get; private set; }

        bool cIsFormAcive=false;
        public Client()
        {

            cTCPClient = new TcpClient();
            //scurity
            cRSAProvider = new RSACryptoServiceProvider(1024);
            cPublicKey = cRSAProvider.ExportParameters(false);
            cPrivateKey = cRSAProvider.ExportParameters(true);
        }

        public bool Connect(string ipAddress, int port)
        {
            //set up connection
            try
            {

                //setup tcp connection
                cTCPClient.Connect(ipAddress, port);

                //setup network stream reader/writers
                cStream = cTCPClient.GetStream();
                cStreamWriter = new BinaryWriter(cStream, Encoding.UTF8);
                cStreamReader = new BinaryReader(cStream, Encoding.UTF8);
                cFormatter = new BinaryFormatter();

                //setup udp connection
                cUDPClient = new UdpClient();
                cUDPClient.Connect(ipAddress, port);

                //send key to server
                PulicKeyPacket pulicKeyPacket = new PulicKeyPacket(cPublicKey);
                TCPSendMessage(pulicKeyPacket);

                return true;
            }
            catch (InvalidCastException e) {
                Console.WriteLine("Exception: " + e.Message);
                return false;
            }
        }


        public void Login()
        {
            LoginPacket loginPacket = new LoginPacket((IPEndPoint)cUDPClient.Client.LocalEndPoint);
            TCPSendMessage(loginPacket);
        }

        public void Run()
        {
            //creat form
            if (!cIsFormAcive)
            {
                cClientForm = new ClientForm(this);
               
            }

            Thread TCPThread = new Thread(() => { TCPProcessServerResponse(); });
            Thread Udpthread = new Thread(() => { UdpProcessServerResponse(); });
            TCPThread.Start();
            Udpthread.Start();
            Login();

            if (!cIsFormAcive)
            {   cIsFormAcive = true;
                cClientForm.ShowDialog();
                
            }

        }

        /// <summary>
        /// TCP Connection Messaging
        /// </summary>
        /// <returns></returns>
        public void TCPSendMessage(Packet Message)
        {
            if (!cIsEnd)
            {
                MemoryStream memoryStream = new MemoryStream();
                cFormatter.Serialize(memoryStream, Message);
                byte[] buffer = memoryStream.GetBuffer();
                cStreamWriter.Write(buffer.Length);
                cStreamWriter.Write(buffer);
                cStreamWriter.Flush();
            }
        }

        public Packet TCPRead()
        {
            int numberOfBytes;
            if ((numberOfBytes = cStreamReader.ReadInt32()) != -1)
            {
                byte[] buffer = cStreamReader.ReadBytes(numberOfBytes);
                MemoryStream memoryStream = new MemoryStream(buffer);

                return cFormatter.Deserialize(memoryStream) as Packet;
            }
            return null;

        }

        private void TCPProcessServerResponse()
        {
            Packet input;


            while ((input = TCPRead()) != null)
            {


                switch (input.packetType)
                {
                    case (PacketType.ChatMessage):

                        ChatMessagePacket chatPacket = (ChatMessagePacket)input;

                        string message = DecryptString(chatPacket.message);
                        cClientForm.UpdateChatWindow(message);
                        break;
                    case (PacketType.ClientName):

                        SetNamePacket namePacket = (SetNamePacket)input;

                        string nameResponce = DecryptString(namePacket.Name);

                        cClientForm.UpdateNameTip(nameResponce);
                        break;
                    case (PacketType.PrivateMessage):

                        PrivateMessagePacket privateMessagePacket = (PrivateMessagePacket)input;
                        string name = DecryptString(privateMessagePacket.Name);
                        string PMMessage = DecryptString(privateMessagePacket.message);
                        cClientForm.UpdateChatWindow(name,name, PMMessage);
                        break;
                    case (PacketType.Leave):

                        LeavePacket leave = (LeavePacket)input;
                        string nameLeave = DecryptString(leave.Name);
                        string PMMessageLeave = DecryptString(leave.message);
                        if (cClientForm.TabExist(nameLeave))
                        {

                            cClientForm.UpdateChatWindow(nameLeave,nameLeave, PMMessageLeave);
                        }
                        break;
                    case (PacketType.ClinetList):
                        ClinetListPacket nameListPacket = (ClinetListPacket)input;
                        List<string> names = new List<string>();
                        foreach (byte[] Name in nameListPacket.Name)
                        {
                            names.Add(DecryptString(Name));
                        }
                        cClientForm.UpdateUserListWindow(names);
                        break;
                    case (PacketType.GameList):
                        GameListPacket gameListPacket = (GameListPacket)input;
                        List<string> nameList = new List<string>();
                        foreach(byte[] Game in gameListPacket.GamesList)
                        {
                            nameList.Add(DecryptString(Game));
                        }
                        cClientForm.UpdateGameList(nameList);

                        break;
                    case (PacketType.PublicKey):
                        PulicKeyPacket pulicKeyPacket = (PulicKeyPacket)input;
                        cServerKey = pulicKeyPacket.PublicKey;

                        break;
                    case (PacketType.GameData):
                        GamePacket gamePacket = (GamePacket)input;
                        string gameName = DecryptString(gamePacket.GameName);
                        string lobbyNo = DecryptString(gamePacket.LobbyNo);
                        string command = DecryptString(gamePacket._Message);
                        cClientForm.UpdateChatWindow(lobbyNo,gameName, command);
                        break;
                    case (PacketType.Dissconnect):
                       
                        break;
                }
                if (cIsEnd)
                {
                    Close();
                    break;
                }

            }

        }

        /// <summary>
        /// UDP messaging
        /// </summary>
        public void UdpSendMessage(Packet message)
        {
            if (!cIsEnd)
            {
                MemoryStream memoryStream = new MemoryStream();
                cFormatter.Serialize(memoryStream, message);
                byte[] buffer = memoryStream.GetBuffer();
                cUDPClient.Send(buffer, buffer.Length);
            }
        }

        private void UdpProcessServerResponse()
        {
            
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                while (!cIsEnd)
                {
                    byte[] bytes = cUDPClient.Receive(ref endPoint);
                    MemoryStream memoryStream = new MemoryStream(bytes);

                    Packet input = cFormatter.Deserialize(memoryStream) as Packet;


                    //do somthing with input

                    switch (input.packetType)
                    {
                        case (PacketType.ChatMessage):

                            ChatMessagePacket chatPacket = (ChatMessagePacket)input;

                            string Message = DecryptString(chatPacket.message);
                            cClientForm.UpdateChatWindow(Message);
                            break;

                        case (PacketType.ClinetList):
                            ClinetListPacket NameListPacket = (ClinetListPacket)input;
                            List<string> Names = new List<string>();
                            foreach (byte[] Name in NameListPacket.Name)
                            {
                                Names.Add(DecryptString(Name));
                            }
                            cClientForm.UpdateUserListWindow(Names);
                            break;
                          
                    }

                }

            }
            catch (SocketException e)
            {

                Console.WriteLine("Client UDP Read Method exception: " + e.Message);

            }

        }


       
        public void Disconnect()
        {
            cIsEnd = true;
        }

        public void Reconnect(string ipAddress, int port)
        {   cIsEnd = false;
            cTCPClient = new TcpClient();
            Connect(ipAddress, port);
            Login();
            
            Run();
        }

        public void Close()
        {
            if (cTCPClient.Connected)
            {
                cTCPClient.Close();
            }
            cStream.Close();
            cStreamWriter.Close();
            cStreamReader.Close();

           
            cUDPClient.Close();


        }


        //encrption
        private byte[] Encrypt(byte[] data)
        {
           
                lock (cRSAProvider)
                {
                    cRSAProvider.ImportParameters(cServerKey);
                    return cRSAProvider.Encrypt(data, true);
                }
            
           
        }

        private byte[] Decrypt(byte[] data)
        {
                lock (cRSAProvider)
                {
                    cRSAProvider.ImportParameters(cPrivateKey);
                    return cRSAProvider.Decrypt(data, true);
                }
        
        }

        Encoding u8 = Encoding.UTF8;
        public byte[] EncryptString(string message)
        {
            
                byte[] Data;
                Data = u8.GetBytes(message);
                Data = Encrypt(Data);
                return Data;
            
            
        }

        public string DecryptString(byte[] data)
        {
            byte[] stringData=Decrypt(data);
            string Message= u8.GetString(stringData);
            return Message;
        }
        public byte[] EncryptInt(int message)
        {

            byte[] Data;
            Data= BitConverter.GetBytes(message);
            Data = Encrypt(Data);
            return Data;
        }

        public int DecryptInt(byte[] data)
        {
            byte[] stringData = Decrypt(data);
            int Message = BitConverter.ToInt32(stringData,0);
            return Message;
        }
        

       


     



    }


}
