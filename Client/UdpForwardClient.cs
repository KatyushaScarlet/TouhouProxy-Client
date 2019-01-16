using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    class UdpForwardClient
    {
        private string serverIP = "";
        private int serverPort = 0;
        private int localPort = 10800;
        private UdpClient udpClient = null;
        private IPEndPoint serverEndPoint = null;
        Thread heartBeat = null;

        private bool flagClose = false;

        public UdpForwardClient(string ip,int port)
        {
            IPEndPoint server = new IPEndPoint(IPAddress.Parse(ip), port);
            udpClient = new UdpClient(0);
            //解决UDP报错问题，详见 https://www.cnblogs.com/liuslayer/p/7867239.html
            uint IOC_IN = 0x80000000;
            uint IOC_VENDOR = 0x18000000;
            uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
            udpClient.Client.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);

            this.serverEndPoint = server;
            this.serverIP = ip;
            this.serverPort = port;
            //发送心跳
            heartBeat = new Thread(HeartBeat);
            heartBeat.Start();
            //开始转发
            udpClient.BeginReceive(new AsyncCallback(ReadComplete), null);
            //发起握手
            byte[] message = Model.Encode(Model.Client_Arrive_Handshake);
            for (int i = 0; i < 10; i++)
            {
                udpClient.Send(message, message.Length, serverEndPoint);
            }
        }

        private void ReadComplete(IAsyncResult ar)
        {
            if (flagClose)
            {
                return;//销毁转发
            }

            IPEndPoint newEndPoint = null;
            byte[] buffer = udpClient.EndReceive(ar, ref newEndPoint);

            if (buffer.Length > 0)
            {
                //if (localEndPoint == null/* && !localEndPoint.Equals(serverEndPoint)*/)
                //{
                //    if (!newEndPoint.Address.Equals(serverEndPoint))
                //    {
                //        localEndPoint = newEndPoint;//设定本地
                //    }
                //}
                //else 
                if (newEndPoint.Equals(serverEndPoint))
                {
                    udpClient.Send(buffer, buffer.Length, new IPEndPoint(IPAddress.Parse("127.0.0.1"), localPort));
                }
                else /*if (newEndPoint.Equals(localEndPoint))*/
                {
                    udpClient.Send(buffer, buffer.Length, serverEndPoint);
                }
            }
            //完成时调用自身
            udpClient.BeginReceive(new AsyncCallback(ReadComplete), null);
        }

        private void HeartBeat()
        {
            while (true)
            {
                if (udpClient != null && serverEndPoint != null)
                {
                    //UDP心跳包，用于保持连接
                    udpClient.Send(Model.Heartbeat, Model.Heartbeat.Length, serverEndPoint);
                }
                //循环发送
                Thread.Sleep(10000);
            }
        }

        public void Close()//结束心跳
        {
            heartBeat.Abort();
            flagClose = true;
        }
    }
}
