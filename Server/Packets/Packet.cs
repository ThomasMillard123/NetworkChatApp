using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
namespace Packets
{
    public enum PacketType
    {
        ChatMessage,
        PrivateMessage,
        Leave,
        ClientName,
        Dissconnect,
        Connect,
        ClinetList,
        Login,
        GameList,
        PublicKey,
        GameData

    }

    [Serializable]
    public abstract class Packet{
         public PacketType packetType { get; protected set; }


        }

    [Serializable]
    public class ChatMessagePacket : Packet
    {
        public byte[] message { get; private set; }

        public ChatMessagePacket(byte[] Message)
        {
            message = Message;
            packetType = PacketType.ChatMessage;

        }
    }

    [Serializable]
    public class SetNamePacket : Packet
    {
        public byte[] Name { get; private set; }

        public SetNamePacket(byte[] name)
        {
            Name = name;
            packetType = PacketType.ClientName;

        }
    }

    [Serializable]
    public class PrivateMessagePacket : Packet
    {
        public byte[] Name { get; private set; }
        public byte[] message { get; private set; }

        public PrivateMessagePacket(byte[] name, byte[] Message)
        {
            Name = name;
            message = Message;
            packetType = PacketType.PrivateMessage;

        }
    }

    [Serializable]
    public class LeavePacket : Packet
    {
        public byte[] Name { get; private set; }
        public byte[] message { get; private set; }

        public LeavePacket(byte[] name, byte[] Message)
        {
            Name = name;
            message = Message;
            packetType = PacketType.Leave;

        }
    }

    [Serializable]
    public class ClinetListPacket : Packet
    {
       
        public List<byte[]> Name { get; private set; }
        public ClinetListPacket(List<byte[]> name)
        {
            Name =name;
            packetType = PacketType.ClinetList;

        }
    }

    [Serializable]
    public class DisssconnectPacket : Packet
    {
        public byte[] message { get; private set; }
        public DisssconnectPacket(byte[] Message)
        {
            message = Message;
            packetType = PacketType.Dissconnect;

        }
    }


    [Serializable]
    public class LoginPacket : Packet
    {
        public IPEndPoint EndPoint { get; private set; }
        public LoginPacket(IPEndPoint endPoint)
        {
            EndPoint = endPoint;

            packetType = PacketType.Login;

        }
    }


    [Serializable]
    public class GameListPacket : Packet
    {
        public List<byte[]> GamesList { get; private set; }
        public GameListPacket(List<byte[]> gameList)
        {
            GamesList = gameList;
            packetType = PacketType.GameList;

        }
    }



    [Serializable]
    public class PulicKeyPacket : Packet
    {
        public RSAParameters PublicKey { get; private set; }
        public PulicKeyPacket(RSAParameters PublicKeyA)
        {
            PublicKey = PublicKeyA;

            packetType = PacketType.PublicKey;

        }
    }


 



    

    //game data
    [Serializable]
    public class GamePacket : Packet
    {
        public byte[] GameName { get; private set; }
        public byte[] LobbyNo { get; private set; }
        public byte[] _Message { get; private set; }
        public GamePacket(byte[] GameType, byte[] LobbyNum, byte[] Message)
        {
            GameName = GameType;
            _Message = Message;
            LobbyNo = LobbyNum;
            packetType = PacketType.GameData;

        }
    }



    //return game data

}
