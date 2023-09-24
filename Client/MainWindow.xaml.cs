using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Socket server;
        private static int clientCount = 0;
        public MainWindow()
        {
            InitializeComponent();

            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.ConnectAsync("127.0.0.1", 8888);

            clientCount++;
            window.Title = "Client " + clientCount;

            RecieveMessege();
        }
        private async Task RecieveMessege()
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                ArraySegment<byte> segment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                await server.ReceiveAsync(segment, SocketFlags.None);
                string messege = Encoding.UTF8.GetString(bytes);

                listBoxMesseges.Items.Add(messege);
            }
        }

        private async void SendMessege(string messege)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(messege);
            ArraySegment<byte> segment = new ArraySegment<byte>(bytes, 0, bytes.Length);
            await server.SendAsync(segment, SocketFlags.None);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendMessege(textBoxMessege.Text);
        }
    }
}
