using System;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        RemoteClient server = null;

        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //button1.Enabled = false;
            //tbxServerIP.Enabled = false;
            //tbxServerPort.Enabled = false;
            //tbxLocalPort.Enabled = false;

            string serverIP = tbxServerIP.Text;
            int serverPort = Convert.ToInt32(Convert.ToInt32(tbxServerPort.Text));

            if (server!=null)
            {
                server = null;
            }

            try
            {
                server = new RemoteClient(serverIP, serverPort);
                server.GetPortEvent += SetNewPort;//绑定回调事件
                MessageBox.Show("连接成功，请获取转发端口", "成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"错误");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (server!=null)
            {
                server.GetNewPort();
            }
            else
            {
                MessageBox.Show("未连接到服务器");
            }
        }

        private void SetNewPort(string ip,int port)
        {
            if (tbxProxyAddress.InvokeRequired)
            {
                RemoteClient.GetPortEventHandler getPortEventHandler = new RemoteClient.GetPortEventHandler(SetNewPort);
                tbxProxyAddress.Invoke(getPortEventHandler, new object[] { ip, port });
            }
            else
            {
                this.tbxProxyAddress.Text = ip + ":"+ port.ToString();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
