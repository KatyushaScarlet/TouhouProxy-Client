namespace Client
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tbxProxyAddress = new System.Windows.Forms.TextBox();
            this.cbxServerIP = new System.Windows.Forms.ComboBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器IP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "中转地址";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 112);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 40);
            this.button1.TabIndex = 5;
            this.button1.Text = "①连接服务器";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(128, 112);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(96, 40);
            this.button2.TabIndex = 6;
            this.button2.Text = "②获取端口";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tbxProxyAddress
            // 
            this.tbxProxyAddress.Location = new System.Drawing.Point(96, 72);
            this.tbxProxyAddress.Name = "tbxProxyAddress";
            this.tbxProxyAddress.Size = new System.Drawing.Size(128, 21);
            this.tbxProxyAddress.TabIndex = 9;
            this.tbxProxyAddress.DoubleClick += new System.EventHandler(this.tbxProxyAddress_DoubleClick);
            // 
            // cbxServerIP
            // 
            this.cbxServerIP.FormattingEnabled = true;
            this.cbxServerIP.Location = new System.Drawing.Point(96, 40);
            this.cbxServerIP.Name = "cbxServerIP";
            this.cbxServerIP.Size = new System.Drawing.Size(128, 20);
            this.cbxServerIP.TabIndex = 10;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkColor = System.Drawing.Color.Red;
            this.linkLabel1.Location = new System.Drawing.Point(40, 16);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(161, 12);
            this.linkLabel1.TabIndex = 12;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Touhou Porxy 实验版本 v0.2";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(16, 160);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(96, 40);
            this.button3.TabIndex = 13;
            this.button3.Text = "③启动游戏";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(128, 160);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(96, 40);
            this.button4.TabIndex = 14;
            this.button4.Text = "修改游戏路径";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(242, 214);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.cbxServerIP);
            this.Controls.Add(this.tbxProxyAddress);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox tbxProxyAddress;
        private System.Windows.Forms.ComboBox cbxServerIP;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}

