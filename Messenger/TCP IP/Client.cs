using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Messenger
{
    public class Client
    {
        public Socket Server { get; private set; }
        public static int ClientsCount { get; private set; } = 1;
        public readonly string Name;

        private ListBox listBoxMessages;
        private ListBox listBoxUsers;

        public Client(string ip, string name, ListBox listBoxMessages, ListBox listBoxUsers)
        {
            ClientsCount++;

            Name = name;
            this.listBoxMessages = listBoxMessages;
            this.listBoxUsers = listBoxUsers;

            Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Server.ConnectAsync(ip, 8888);

            RecieveMessege();
            SendMyName(name);
            this.listBoxUsers = listBoxUsers;
        }

        public async void SendMyName(string name)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(name);
            ArraySegment<byte> segment = new ArraySegment<byte>(bytes, 0, bytes.Length);
            await Server.SendAsync(segment, SocketFlags.None);
        }

        public async Task RecieveNewClients()
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                ArraySegment<byte> segment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                var server = await Server.ReceiveAsync(segment, SocketFlags.None);
                var name = Encoding.UTF8.GetString(bytes);
                listBoxUsers.Items.Add(name);
            }
        }

        public async Task RecieveMessege()
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                ArraySegment<byte> segment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                var server = await Server.ReceiveAsync(segment, SocketFlags.None);
                var packet = Encoding.UTF8.GetString(bytes).Split('#');
                listBoxMessages.Items.Add($"{packet[0]} > {packet[1]}");
            }
        }

        public async void SendMessage(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Name + '#' + message);
            ArraySegment<byte> segment = new ArraySegment<byte>(bytes, 0, bytes.Length);
            await Server.SendAsync(segment, SocketFlags.None);
        }
    }
}
