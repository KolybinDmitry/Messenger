using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Runtime.Remoting.Messaging;

namespace Messenger
{
    public class Server
    {
        private Socket socket;
        private List<Socket> clients = new List<Socket>();
        private ListBox listBoxLogs;
        public Server(ListBox listBoxLogs)
        {
            this.listBoxLogs = listBoxLogs;

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 8888);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipPoint);
            socket.Listen(1000);

            ListenToClients();
        }

        private async Task ListenToClients()
        {
            while (true)
            {
                var client = await socket.AcceptAsync();
                listBoxLogs.Items.Add($"[Подключился {client.RemoteEndPoint}]");
                clients.Add(client);
                RecieveMessege(client);
            }
        }

        private async Task RecieveMessege(Socket client)
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                ArraySegment<byte> segment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                await client.ReceiveAsync(segment, SocketFlags.None);
                var packet = Encoding.UTF8.GetString(bytes).Split('#');

                listBoxLogs.Items.Add($"[Сообщение от {client.RemoteEndPoint}] : {packet[1]}");

                foreach (var item in clients)
                {
                    SendMessege(item, Encoding.UTF8.GetString(bytes));
                }
            }
        }

        private async void SendMessege(Socket client, string messege)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(messege);
            ArraySegment<byte> segment = new ArraySegment<byte>(bytes, 0, bytes.Length);
            await client.SendAsync(segment, SocketFlags.None);
        }
    }
}
