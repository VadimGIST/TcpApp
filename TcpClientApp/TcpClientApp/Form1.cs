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
                Log("���������� �����������");
            }
            catch (Exception ex)
            {
                Log($"������ �����������: {ex.Message}");
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(txtMessage.Text);
                stream.Write(data, 0, data.Length);
                Log("��������� ����������");
            }
            catch (Exception ex)
            {
                Log($"������ ��������: {ex.Message}");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                stream.Close();
                client.Close();
                Log("���������� �������");
            }
            catch (Exception ex)
            {
                Log($"������ �������� ����������: {ex.Message}");
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
