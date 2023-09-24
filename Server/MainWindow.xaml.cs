using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Server
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Socket socket;
        private List<Socket> clients = new List<Socket>();
        public MainWindow()
        {
            InitializeComponent();

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
                string messege = Encoding.UTF8.GetString(bytes);

                listBoxMesseges.Items.Add($"[Сообщение от {client.RemoteEndPoint}] : {messege}");

                messege = client.RemoteEndPoint.ToString() + " > " + messege;

                foreach (var item in clients)
                {
                    SendMessege(item, messege);
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
