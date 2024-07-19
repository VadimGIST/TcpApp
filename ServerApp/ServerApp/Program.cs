using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Data.Sqlite;

class Program
{
    static void Main()
    {
        string connectionString = "Data Source=messages.db";
        CreateDatabaseAndTable(connectionString);

        var listener = new TcpListener(IPAddress.Any, 12345);
        listener.Start();
        Console.WriteLine("Server started...");

        while (true)
        {
            var client = listener.AcceptTcpClient();
            var stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Console.WriteLine("Received: " + message);

            SaveMessage(connectionString, message);

            client.Close();
        }
    }

    private static void CreateDatabaseAndTable(string connectionString)
    {
        using (var connection = new SqliteConnection(connectionString))
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

    private static void SaveMessage(string connectionString, string message)
    {
        using (var connection = new SqliteConnection(connectionString))
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
}
