using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Messenger
{
    /// <summary>
    /// Логика взаимодействия для Server.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private bool isLookLog = false;

        // админ как сервер
        private List<Socket> clients = new List<Socket>();
        private List<string> clientsNameServer = new List<string>();
        private Socket socket;
        
        // админ как клиент
        private Socket server;
        public readonly string Name;
        private List<string> clientsName = new List<string>();

        public AdminWindow(string ip, string name)
        {
            InitializeComponent();

            // сервер
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 8888);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipPoint);
            socket.Listen(1000);

            ListenToClients();

            // клиент
            Name = name;
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.ConnectAsync(ip, 8888);
            RecieveMessege();

            // уведомляем о том, что мы (админ) подключился
            byte[] bytes = Encoding.UTF8.GetBytes('#' + Name);
            ArraySegment<byte> segment = new ArraySegment<byte>(bytes, 0, bytes.Length);
            server.SendAsync(segment, SocketFlags.None);
        }

        #region Server
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
                string message = Encoding.UTF8.GetString(bytes);

                // новое сообщение
                if (message.Contains("$"))
                {
                    var packet = message.Split('$');
                    listBoxLogs.Items.Add("[" + packet[0] + "] отправил:\n" + packet[1]);
                }
                // подключился новый пользователь
                if (message.Contains("#"))
                {
                    listBoxLogs.Items.Add("Подключился [" + message.Substring(1) + "]");
                    clientsNameServer.Add(message.Substring(1));

                }
                // отключился какой-то пользователь 
                else if (message.Contains("@"))
                {
                    listBoxLogs.Items.Add("Отключился [" + message.Substring(1) + "]");
                    clientsNameServer.Remove(message.Substring(1));
                }

                foreach (var item in clients)
                {
                    SendMessege(item, message);
                }
            }
        }

        private async void SendMessege(Socket client, string messege)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(messege);
            ArraySegment<byte> segment = new ArraySegment<byte>(bytes, 0, bytes.Length);
            await client.SendAsync(segment, SocketFlags.None);
        }
        #endregion

        #region Client
        private async Task RecieveMessege()
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                ArraySegment<byte> segment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                await server.ReceiveAsync(segment, SocketFlags.None);
                string message = Encoding.UTF8.GetString(bytes);

                // новое сообщение
                if (message.Contains("$"))
                {
                    var packet = message.Split('$');
                    listBoxMessages.Items.Add(packet[0] + " > " + packet[1]);
                }
                // подключился
                else if (message.Contains("#"))
                {
                    var names = message.Split('#').ToList();
                    names.RemoveAll(x => x.Length == 0);

                    foreach (var item in names)
                    {
                        if (!clientsName.Contains(item))
                            clientsName.Add(item);
                    }

                    listBoxUsers.Items.Clear();
                    foreach (var item in clientsName)
                    {
                        listBoxUsers.Items.Add(item);
                    }
                }
                // или отключился кто-то
                else if (message.Contains("@"))
                {
                    listBoxUsers.Items.Clear();
                    var names = message.Split('@').ToList();
                    names.RemoveAll(x => x.Length == 0);

                    clientsName.Remove(names[0]);

                    listBoxUsers.Items.Clear();
                    foreach (var item in clientsName)
                    {
                        listBoxUsers.Items.Add(item);
                    }
                }
            }
        }

        private async void SendMessage(string messege)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Name + '$' + messege);
            ArraySegment<byte> segment = new ArraySegment<byte>(bytes, 0, bytes.Length);
            await server.SendAsync(segment, SocketFlags.None);
        }
        #endregion

        private void buttonLookLog_Click(object sender, RoutedEventArgs e)
        {
            if (!isLookLog)
            {
                listBoxLogs.Visibility = Visibility.Visible;
                listBoxUsers.Visibility = Visibility.Hidden;
                textBlock.Visibility = Visibility.Hidden;

                buttonLookLog.Content = "Посмотреть список пользователей";
            }
            else
            {
                listBoxLogs.Visibility = Visibility.Hidden;
                listBoxUsers.Visibility = Visibility.Visible;
                textBlock.Visibility = Visibility.Visible;

                buttonLookLog.Content = "Посмотреть логи чата";
            }

            isLookLog = !isLookLog;
        }

        private void buttonSend_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxMessage.Text == "/disconnect")
            {
                Exit();
            }
            SendMessage(textBoxMessage.Text);
            textBoxMessage.Clear();
        } 

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Exit();
        }

        // обработать нажатие на крестик
        protected override void OnClosed(EventArgs e)
        {
            Exit();
        }

        private void Exit()
        {
            byte[] bytes = Encoding.UTF8.GetBytes('$' + Name);
            ArraySegment<byte> segment = new ArraySegment<byte>(bytes, 0, bytes.Length);
            server.SendAsync(segment, SocketFlags.None);

            this.Owner.Show();
            this.Close();
        }
    }
}
