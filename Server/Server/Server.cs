using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Security.AccessControl;
using System.IO;
using Packets;
namespace Server
{

    class Server
    {
        TcpListener cServer = null;
        UdpClient cUDPListener;
        public ConcurrentDictionary<int, Lobby> cLobbys;
        public ConcurrentDictionary<int,Client> cClients;
        List<string> cCurrentNames = new List<string>();
        List<string> cOldNames = new List<string>();
        Games cGames;
        int cLobbyindex=1;

        
        public Server( string ipAddress,int port)
        {
            IPAddress IP = IPAddress.Loopback;

            cServer = new TcpListener(IP, port);
            cUDPListener = new UdpClient(port);
            Thread UpdListen = new Thread(() => { UdpListen(); });
            UpdListen.Start();
        }

        public void Start()
        { 
            cClients = new ConcurrentDictionary<int,Client>();
            cLobbys = new ConcurrentDictionary<int, Lobby>();
            cServer.Start();
            cGames = new Games();


            Thread thread2 = new Thread(() => { UpdateNameList(); });
            

            
            thread2.Start();
            
            int clientIndex = 0;
            while (true) {
                int index = clientIndex;
                clientIndex++;

                //a blocking function waiting for a socket to be returned
                Socket socket = cServer.AcceptSocket();
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                Client client = new Client(socket);
               
                cClients.TryAdd(index,client);

                Thread thread = new Thread(() => { ClientMethord(index); });
               

                thread.Start();

            }
           
        }

        public void Stop()
        {
            cServer.Stop();
        }

        private void ClientMethord(int index)
        {
            
            Packet recivedMessage;
            

           


            while ((recivedMessage = cClients[index].TCPRead()) != null)
            {
               

                ServerToUser(index, recivedMessage);



                if (cClients[index].cIsDisconnect)
                {

                    break;
                }

            }
            cClients[index].Close();
             
            cClients.TryRemove(index ,out Client client);
            
        }

        void SendGameList(int index)
        {
            List<byte[]> Games = new List<byte[]>();
            foreach (string Game in cGames.cGameList)
            {
                Games.Add(cClients[index].EncryptString(Game));
            }
            GameListPacket gameList = new GameListPacket(Games);
            cClients[index].TCPSend(gameList);  
        }
        //Udp
        private void UdpListen()
        {
           
            while (true)
            {
                try
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                    while (true)
                    {
                        //read
                        byte[] bytes = cUDPListener.Receive(ref endPoint);
                       
                        MemoryStream memoryStream = new MemoryStream(bytes);
                        Packet input = cClients[0].cFormatter.Deserialize(memoryStream) as Packet;
                        

                        foreach (Client c in cClients.Values)
                        {

                            if (endPoint.ToString() == c.cEndPoint.ToString())
                            {
                                //Handle your packet here
                                switch (input.packetType)
                                {
                                    case (PacketType.ChatMessage):

                                        foreach (Client a in cClients.Values)
                                        {
                                            
                                            //send
                                            
                                            a.UDPSend(input, cUDPListener);
                                        }


                                        break;
                                }
                               
                                    
                                
                            }



                        }
                     

                    }

                }
                catch (SocketException e)

                {

                    Console.WriteLine("Client UDP Read Method exception: " + e.Message);

                }


            }

        }
        //TCP
        private void ServerToUser(int index, Packet recivedMessage)
        {
            ChatMessagePacket ChatPacket;
            switch (recivedMessage.packetType)
            {

                case (PacketType.ChatMessage):
                    ChatMessagePacket chatOut = (ChatMessagePacket)recivedMessage;
                    string chatMessage = cClients[index].DecryptString(chatOut.message);
                    foreach (var c in cClients.Values)
                    {
                        ChatPacket = new ChatMessagePacket(c.EncryptString(cClients[index].cNickName + ": " + chatMessage));
                        c.TCPSend(ChatPacket);
                    }
                    break;

                case (PacketType.ClientName):

                    SetNamePacket namePacket = (SetNamePacket)recivedMessage;
                    bool nameTaken = false;
                    string name = cClients[index].DecryptString(namePacket.Name);
                    //check if it is uniqe
                    for (int i = 0; i < cCurrentNames.Count; i++)
                    {
                       if( cCurrentNames[i]==name)
                        {
                            //match not valid name
                            nameTaken = true;
                            break;
                        }
                       
                    }
                   if(cClients[index].cNickName == name)
                    {
                        nameTaken = false;
                    }
                    

                    if (!nameTaken)
                    {
                        cClients[index].cNickName = name;
                        ChatPacket = new ChatMessagePacket(cClients[index].EncryptString("Your Name is: " + cClients[index].cNickName));
                        cClients[index].TCPSend(ChatPacket);
                        namePacket = new SetNamePacket(cClients[index].EncryptString("NameSet"));
                        cClients[index].TCPSend(namePacket);
                        foreach (Client c in cClients.Values)
                        {
                            ChatPacket = new ChatMessagePacket(c.EncryptString("Server to ALL: " + cClients[index].cNickName + " has connected "));
                            c.TCPSend(ChatPacket);
                        }

                    }
                    else if (nameTaken)
                    {
                        namePacket = new SetNamePacket(cClients[index].EncryptString(name + " is taken \n Please select anothor"));
                        cClients[index].TCPSend(namePacket);
                    }
                    break;

                case (PacketType.PrivateMessage):

                    PrivateMessagePacket pmPacket = (PrivateMessagePacket)recivedMessage;
                    string nameTo = cClients[index].DecryptString(pmPacket.Name);
                    string pmMessage= cClients[index].DecryptString(pmPacket.message);
                    if (nameTo == cClients[index].cNickName)
                    {
                        //do nothing
                    }
                    else
                    {
                        foreach (Client clinet in cClients.Values)
                        {
                            if (clinet.cNickName == nameTo)
                            {
                                PrivateMessagePacket PrivetChatPacket = new PrivateMessagePacket(clinet.EncryptString(cClients[index].cNickName), clinet.EncryptString(cClients[index].cNickName + " : " + pmMessage));
                                clinet.TCPSend(PrivetChatPacket);
                                PrivetChatPacket = new PrivateMessagePacket(cClients[index].EncryptString(clinet.cNickName), cClients[index].EncryptString(cClients[index].cNickName + " : " + pmMessage));
                                cClients[index].TCPSend(PrivetChatPacket);
                            }
                        }
                    }

                    break;
                case (PacketType.Leave):

                    LeavePacket packetLeave = (LeavePacket)recivedMessage;
                    string lobbyName = cClients[index].DecryptString(packetLeave.Name);
                    string message = cClients[index].DecryptString(packetLeave.message);


                    int lobbyIndex;
                    Int32.TryParse(lobbyName, out lobbyIndex);
                    if (lobbyIndex == 0)
                    {
                        foreach (var c in cClients.Values)
                        {
                            if (c.cNickName == lobbyName)
                            {
                                LeavePacket privetChatPacket = new LeavePacket(c.EncryptString(cClients[index].cNickName), c.EncryptString(cClients[index].cNickName + " : " + message));
                                c.TCPSend(privetChatPacket);

                            }
                        }
                    }
                    else
                    {

                        //take out client from lobby
                        cLobbys[lobbyIndex].TakeOutUser(index);
                    }

                    break;


                case (PacketType.Login):
                    LoginPacket loginPacket = (LoginPacket)recivedMessage;
                    cClients[index].cEndPoint = loginPacket.EndPoint;
                    cClients[index].UDPSend(new ChatMessagePacket(cClients[index].EncryptString("UDP Connected")), cUDPListener);

                    SendGameList(index);

                    ChatPacket = new ChatMessagePacket(cClients[index].EncryptString("You have Connected"));
                    cClients[index].TCPSend(ChatPacket);
                    break;
                case (PacketType.PublicKey):
                    PulicKeyPacket pulicKeyPacket = (PulicKeyPacket)recivedMessage;
                    cClients[index].cClientKey=pulicKeyPacket.PublicKey;
                    pulicKeyPacket = new PulicKeyPacket(cClients[index].cPublicKey);
                    cClients[index].TCPSend(pulicKeyPacket);


                    break;
               
                case (PacketType.GameData):
                    //take out unsed lobbys
                    foreach(Lobby lobbys in cLobbys.Values)
                    {
                        if (lobbys.NumberOfUser() == 0)
                        {
                            cLobbys.TryRemove(lobbys.cLobbyName, out Lobby lobby);
                        }
                        else if (lobbys.cIsGameEnd)
                        {
                            cLobbys.TryRemove(lobbys.cLobbyName, out Lobby lobby);
                        }
                    }


                    GamePacket gamePacket = (GamePacket)recivedMessage;
                    //id game 
                    string gameName =cClients[index].DecryptString( gamePacket.GameName);
                    string lobbyNum = cClients[index].DecryptString(gamePacket.LobbyNo);
                    string command = cClients[index].DecryptString(gamePacket._Message);
                    int LobbyNO;
                    Int32.TryParse(lobbyNum, out LobbyNO);
                    switch (command.ToLower())
                    {
                        case "start":
                            //start lobby
                            int currentIndex = cLobbyindex;
                            Lobby lobby = new Lobby(lobbyNum, currentIndex, index);
                            cLobbys.TryAdd(currentIndex, lobby);
                            cLobbyindex++;

                            
                            SendUserAnwerOut(cLobbys[currentIndex].cLobbyType, cLobbys[currentIndex].cLobbyName, "Users has joied");
                            SendOutGameData(cLobbys[currentIndex].cLobbyName);
                            break;
                        case "join":
                            //add to lobby
                            foreach(Lobby lobbys in cLobbys.Values)
                            {
                                if (lobbys.cLobbyType == gameName)
                                {
                                    if (lobbys.NumberOfUser() < 10)
                                    {
                                        if (lobbys.AddUser(index))
                                        {
                                            SendUserAnwerOut(lobbys.cLobbyType, lobbys.cLobbyName, "Users has joied");
                                            SendUserAnwerOut(lobbys.cLobbyType, lobbys.cLobbyName, lobbys.GetQuestion());
                                            break;
                                        }
                                    }
                                }
                            }
                            break;
                        default:
                            
                            SendUserAnwerOut(cLobbys[LobbyNO].cLobbyType, LobbyNO, command);
                            //check answer
                            CheckAnswer(LobbyNO, command);
                            //send it to be displayed
                            SendOutGameData(cLobbys[LobbyNO].cLobbyName);

                            break;
                    }

                  
                  

                    break;
                case (PacketType.Dissconnect):
                    DisssconnectPacket dissconnectMessage = (DisssconnectPacket)recivedMessage;
                    ChatPacket = new ChatMessagePacket(cClients[index].EncryptString("Server to " + cClients[index].cNickName + ": " + dissconnectMessage.message));
                    cClients[index].TCPSend(ChatPacket);

                    foreach(Lobby lobby in cLobbys.Values)
                    {
                        if (lobby.GetUserList().Contains(index))
                        {
                            lobby.TakeOutUser(index);
                        }

                    }


                    foreach (Client c in cClients.Values)
                    {
                        ChatPacket = new ChatMessagePacket(c.EncryptString("Server to ALL: " + cClients[index].cNickName+ " has dissconnected "));
                        c.TCPSend(ChatPacket);
                    }

                    
                    cClients[index].cIsDisconnect = true;

                    

                    break;




            }

            

        }

        private void SendOutGameData(int lobbyIndex)
        {

            //for each lobby send out the data 
            string question= cLobbys[lobbyIndex].GetQuestion();
           
            
            //send to users
            for(int i=0;i< cLobbys[lobbyIndex].NumberOfUser(); i++)
            { GamePacket gamePacket = new GamePacket(cClients[cLobbys[lobbyIndex].GetUserList()[i]].EncryptString(cLobbys[lobbyIndex].cLobbyType), cClients[cLobbys[lobbyIndex].GetUserList()[i]].EncryptString(lobbyIndex.ToString()), cClients[cLobbys[lobbyIndex].GetUserList()[i]].EncryptString(question));
              cClients[cLobbys[lobbyIndex].GetUserList()[i]].TCPSend(gamePacket);
            }


            //if need hit
            if (cLobbys[lobbyIndex].IsHintNeed())
            {
                string hint = cLobbys[lobbyIndex].GetHint();

                //send to users
                for (int i = 0; i < cLobbys[lobbyIndex].NumberOfUser(); i++)
                {
                GamePacket gamePacket = new GamePacket(cClients[cLobbys[lobbyIndex].GetUserList()[i]].EncryptString(cLobbys[lobbyIndex].cLobbyType), cClients[cLobbys[lobbyIndex].GetUserList()[i]].EncryptString(lobbyIndex.ToString()), cClients[cLobbys[lobbyIndex].GetUserList()[i]].EncryptString(hint));
                    cClients[cLobbys[lobbyIndex].GetUserList()[i]].TCPSend(gamePacket);
                }
            }
        }
        
        private void SendUserAnwerOut(string lobbyName,int lobbyIndex,string message)
        {
            

            //send to users
            for (int i = 0; i < cLobbys[lobbyIndex].NumberOfUser(); i++)
            {GamePacket gamePacket = new GamePacket(cClients[cLobbys[lobbyIndex].GetUserList()[i]].EncryptString(lobbyName), cClients[cLobbys[lobbyIndex].GetUserList()[i]].EncryptString(lobbyIndex.ToString()), cClients[cLobbys[lobbyIndex].GetUserList()[i]].EncryptString(message));
                cClients[cLobbys[lobbyIndex].GetUserList()[i]].TCPSend(gamePacket);
            }

        }
        private void CheckAnswer(int lobbyIndex,string answer)
        {
            //curre
            bool isCorrect = cLobbys[lobbyIndex].CheckAnswer(answer);
            if (isCorrect)
            {
                //correct
                
                for (int i = 0; i < cLobbys[lobbyIndex].NumberOfUser(); i++)
                {   GamePacket gamePacket = new GamePacket(cClients[cLobbys[lobbyIndex].GetUserList()[i]].EncryptString(cLobbys[lobbyIndex].cLobbyType), cClients[cLobbys[lobbyIndex].GetUserList()[i]].EncryptString(lobbyIndex.ToString()), cClients[cLobbys[lobbyIndex].GetUserList()[i]].EncryptString("Correct"));
                    cClients[cLobbys[lobbyIndex].GetUserList()[i]].TCPSend(gamePacket);
                }
                //next question
                cLobbys[lobbyIndex].GetQuestion();
            }
            else if(!isCorrect)
            {
                //wrong
                
                for (int i = 0; i < cLobbys[lobbyIndex].NumberOfUser(); i++)
                {GamePacket gamePacket = new GamePacket(cClients[cLobbys[lobbyIndex].GetUserList()[i]].EncryptString(cLobbys[lobbyIndex].cLobbyType), cClients[cLobbys[lobbyIndex].GetUserList()[i]].EncryptString(lobbyIndex.ToString()), cClients[cLobbys[lobbyIndex].GetUserList()[i]].EncryptString("Wrong"));
                    cClients[cLobbys[lobbyIndex].GetUserList()[i]].TCPSend(gamePacket);
                }
            }
        }
    
        private List<string> GetNameList()
        {
            List<string> names= new List<string>();

           
            foreach (var c in cClients.Values)
            {
                if (c.cNickName == null)
                {

                }
                else
                {
                    names.Add(c.cNickName);
                }
                
            }
           
            return names;
        }
        private void UpdateNameList()
        {
            
            while (cClients!=null) {
                
                bool isEqual = true;


                //if names updated send out new list 
                cCurrentNames = GetNameList();
                if (cOldNames == null)
                {
                    isEqual = false;
                }
                //if number of elments do not match send it out
                else if (cOldNames.Count < cCurrentNames.Count)
                {
                    isEqual = !cCurrentNames.Except(cOldNames).Any();


                }
                else if (cOldNames.Count > cCurrentNames.Count)
                {

                    isEqual = !cOldNames.Except(cCurrentNames).Any();

                }

                //check each option to see if they are the same
                else if (cOldNames.Count == cOldNames.Count) {
                    for (int i = 0; i < cCurrentNames.Count; i++)
                    {
                        if (cOldNames[i] == cCurrentNames[i])
                        {

                        }else if(cOldNames[i] != cCurrentNames[i])
                        {
                            isEqual = false;
                            break;
                        }
                }
                }

                if (!isEqual)
                {
                    
                    foreach (var c in cClients.Values)
                    {
                        List<byte[]> Names = new List<byte[]>();
                        foreach(string Name in cCurrentNames)
                        {
                           Names.Add( c.EncryptString(Name));
                        }
                        ClinetListPacket nameList = new ClinetListPacket(Names);
                        c.UDPSend(nameList, cUDPListener);
                        //exsit
                    }
                }

                cOldNames = cCurrentNames;
                
            }


            
        }
       
    }
}
