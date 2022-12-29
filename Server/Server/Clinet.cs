using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Security.AccessControl;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using Packets;
using System.Security.Cryptography;
namespace Server
{
   public class Client
    {
        Socket cSocket;
        NetworkStream cStream;
        BinaryWriter cStreamWriter;
        BinaryReader cStreamReader;
        public BinaryFormatter cFormatter { get; private set; }

        public IPEndPoint cEndPoint;

        RSACryptoServiceProvider cRSAProvider;
        public RSAParameters cPublicKey { get; set; }
        private RSAParameters cPrivateKey;
        public RSAParameters cClientKey { get; set; }

        object cReadLock;
        object cWriteLock;
        public String cNickName { get; set; }
       public bool cIsDisconnect { get; set; }





        public Client(Socket socket)
        {
            cWriteLock = new object();
            cReadLock = new object();
            cSocket = socket;


            cStream = new NetworkStream(cSocket);
            cStreamReader = new BinaryReader(cStream, Encoding.UTF8);
            cStreamWriter = new BinaryWriter(cStream, Encoding.UTF8);
            cFormatter = new BinaryFormatter();

            cRSAProvider = new RSACryptoServiceProvider(1024);
            cPublicKey = cRSAProvider.ExportParameters(false);
            cPrivateKey = cRSAProvider.ExportParameters(true);

        }

        public void Close()
        {
            cStreamReader.Close();
            cStreamWriter.Close();
            cStream.Close();
            
            cSocket.Close();
        }


        public Packet TCPRead()
        {
            lock (cReadLock)
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
        }
        



        public void TCPSend(Packet message)
        {
            lock (cWriteLock)
            {
                MemoryStream memoryStream = new MemoryStream();
                cFormatter.Serialize(memoryStream, message);
                byte[] buffer = memoryStream.GetBuffer();
                cStreamWriter.Write(buffer.Length);
                cStreamWriter.Write(buffer);
                cStreamWriter.Flush();
            }
        }


        public Packet UDPRead(UdpClient udpListener)
        {
            lock (cReadLock)
            {

                byte[] bytes = udpListener.Receive(ref cEndPoint);
                MemoryStream memoryStream = new MemoryStream(bytes);
                
                return cFormatter.Deserialize(memoryStream) as Packet;
              

            }
        }




        public void UDPSend(Packet message,UdpClient udpListener)
        {
            lock (cWriteLock)
            {
                MemoryStream memoryStream = new MemoryStream();
                cFormatter.Serialize(memoryStream, message);
                byte[] buffer = memoryStream.GetBuffer();
                udpListener.Send(buffer, buffer.Length,cEndPoint);
            }
        }


        //encription 
        private byte[] Encrypt(byte[] data)
        {
            lock (cRSAProvider)
            {
                cRSAProvider.ImportParameters(cClientKey);
                return cRSAProvider.Encrypt(data, true);
            }
        }

        public byte[] Decrypt(byte[] data)
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
            byte[] stringData = Decrypt(data);
            string Message = u8.GetString(stringData);
            return Message;
        }

        public int DecryptInt(byte[] data)
        {
            byte[] stringData = Decrypt(data);
            int Message = BitConverter.ToInt32(stringData, 0);
            return Message;
        }

        public byte[] EncryptInt(int message)
        {

            byte[] Data;
            Data = BitConverter.GetBytes(message);
            Data = Encrypt(Data);
            return Data;
        }

    }
}
