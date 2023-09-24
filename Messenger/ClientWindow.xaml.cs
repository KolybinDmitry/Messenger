using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Xml.Linq;

namespace Messenger
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        private Socket server;
        public readonly string Name;
        private List<string> clientsName = new List<string>();

        public ClientWindow(string ip, string name)
        {
            InitializeComponent();

            Name = name;

            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.ConnectAsync(ip, 8888);
            RecieveMessege();

            // уведомляем о том, что мы (очередной пользователь) подключился
            byte[] bytes = Encoding.UTF8.GetBytes('#' + Name);
            ArraySegment<byte> segment = new ArraySegment<byte>(bytes, 0, bytes.Length);
            server.SendAsync(segment, SocketFlags.None);
        }

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
            byte[] bytes = Encoding.UTF8.GetBytes('@' + Name);
            ArraySegment<byte> segment = new ArraySegment<byte>(bytes, 0, bytes.Length);
            server.SendAsync(segment, SocketFlags.None);

            this.Owner.Show();
            this.Close();
        }
    }
}
