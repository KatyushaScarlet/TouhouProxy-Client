using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace Client
{
    public partial class Form1 : Form
    {
        RemoteClient server = null;
        XMLConfig config = new XMLConfig("config.xml");

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
                MessageBox.Show("连接成功，请点击获取端口", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                tbxProxyAddress.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            //TODO 告知服务器断开
            System.Environment.Exit(0);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://jq.qq.com/?_wv=1027&k=5Gdv5PQ");
        }

        private void tbxProxyAddress_DoubleClick(object sender, EventArgs e)
        {
            if (tbxProxyAddress.Text!="")
            {
                tbxProxyAddress.Copy();
                MessageBox.Show("复制成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (config.GetGamePath()=="")
            {
                SetGamePath();
            }
            Process.Start(config.GetGamePath());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetGamePath();
        }

        private void SetGamePath()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "游戏文件(*.exe)|*.exe";
            dialog.ValidateNames = true;
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;

            while (dialog.ShowDialog()!=DialogResult.OK)
            {
                MessageBox.Show("请选择有效的文件", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (config.SetGamePath(dialog.FileName))
            {
                MessageBox.Show("保存成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("配置文件读取失败，请重新启动", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
