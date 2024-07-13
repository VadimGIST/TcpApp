using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace TcpClientApp
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        private NetworkStream stream;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                client = new TcpClient(txtIpAddress.Text, int.Parse(txtPort.Text));
                stream = client.GetStream();
                Log("Соединение установлено");
            }
            catch (Exception ex)
            {
                Log($"Ошибка подключения: {ex.Message}");
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(txtMessage.Text);
                stream.Write(data, 0, data.Length);
                Log("Сообщение отправлено");
            }
            catch (Exception ex)
            {
                Log($"Ошибка отправки: {ex.Message}");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                stream.Close();
                client.Close();
                Log("Соединение закрыто");
            }
            catch (Exception ex)
            {
                Log($"Ошибка закрытия соединения: {ex.Message}");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtIpAddress.Clear();
            txtPort.Clear();
            txtMessage.Clear();
            txtLog.Clear();
        }

        private void Log(string message)
        {
            txtLog.AppendText($"{DateTime.Now}: {message}{Environment.NewLine}");
        }
    }
}
