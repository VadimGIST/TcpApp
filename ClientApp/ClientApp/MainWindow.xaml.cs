using System;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using Microsoft.Data.Sqlite;

namespace ChatClient
{
    public partial class MainWindow : Window
    {
        private TcpClient _client;
        private string _connectionString = "Data Source=messages.db";

        public MainWindow()
        {
            InitializeComponent();
            CreateDatabaseAndTable();
            LoadMessages();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ip = IpTextBox.Text;
                int port = int.Parse(PortTextBox.Text);
                _client = new TcpClient(ip, port);
                StatusTextBlock.Text = "Connected";
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = "Connection failed: " + ex.Message;
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            IpTextBox.Clear();
            PortTextBox.Clear();
            MessageTextBox.Clear();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_client != null && _client.Connected)
                {
                    string message = MessageTextBox.Text;
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    _client.GetStream().Write(data, 0, data.Length);
                    StatusTextBlock.Text = "Message sent";
                    SaveMessage(message);
                    LoadMessages();
                }
                else
                {
                    StatusTextBlock.Text = "Not connected";
                }
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = "Failed to send message: " + ex.Message;
            }
        }

        private void SaveMessage(string message)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    INSERT INTO messages (message)
                    VALUES ($message)
                ";
                command.Parameters.AddWithValue("$message", message);
                command.ExecuteNonQuery();
            }
        }

        private void LoadMessages()
        {
            MessagesListBox.Items.Clear();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    SELECT message
                    FROM messages
                ";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MessagesListBox.Items.Add(reader.GetString(0));
                    }
                }
            }
        }

        private void CreateDatabaseAndTable()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS messages (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        message TEXT
                    )
                ";
                command.ExecuteNonQuery();
            }
        }
    }
}
