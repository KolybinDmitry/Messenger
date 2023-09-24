using System.Collections.Generic;
using System.Windows;

namespace Messenger
{
    /// <summary>
    /// Логика взаимодействия для Start.xaml
    /// </summary>
    public partial class Start : Window
    {
        public static List<string> ClientsName { get; private set; } = new List<string>();
        public Start()
        {
            InitializeComponent();
        }

        private void buttonNewChat_Click(object sender, RoutedEventArgs e)
        {
            // Валидация Name
            // Валидация IP

            ClientsName.Add(textBoxName.Text);

            AdminWindow admin = new AdminWindow(textBoxIp.Text, textBoxName.Text);

            admin.Owner = this;

            this.Hide();

            admin.Show();
        }

        private void buttonConnect_Click(object sender, RoutedEventArgs e)
        {
            // Валидация Name
            // Валидация IP

            ClientsName.Add(textBoxName.Text);

            ClientWindow client = new ClientWindow(textBoxIp.Text, textBoxName.Text);

            client.Owner = this;

            this.Hide();

            client.Show();
        }
    }
}
