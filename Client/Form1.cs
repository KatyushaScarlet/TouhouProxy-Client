using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Client
{
    public partial class Form1 : Form
    {
        RemoteClient server = null;

        public Form1()
        {
            InitializeComponent();

            cbxServerIP.Items.Add("118.25.48.106");
            cbxServerIP.Items.Add("127.0.0.1");
            cbxServerIP.SelectedIndex = 0;
            //TODO 检测版本
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //button1.Enabled = false;
            //tbxServerIP.Enabled = false;
            //tbxServerPort.Enabled = false;
            //tbxLocalPort.Enabled = false;

            string serverIP = cbxServerIP.SelectedItem.ToString();

            if (server!=null)
            {
                server.removeForward();
                server = null;
            }

            try
            {
                server = new RemoteClient(serverIP);
                server.GetPortEvent += SetNewPort;//绑定回调事件
                MessageBox.Show("连接成功，请点击获取端口", "成功");

                tbxProxyAddress.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"错误");
                server = null;
                tbxProxyAddress.Text = "";
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (server != null)
                {
                    server.GetNewPort();
                }
                else
                {
                    MessageBox.Show("未连接到服务器");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
                server = null;
                tbxProxyAddress.Text = "";
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://touhou.su");
        }

        private void tbxProxyAddress_DoubleClick(object sender, EventArgs e)
        {
            if (tbxProxyAddress.Text!="")
            {
                tbxProxyAddress.Copy();
                MessageBox.Show("复制成功！", "成功");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //启动游戏
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //修改游戏路径
        }
    }
}
