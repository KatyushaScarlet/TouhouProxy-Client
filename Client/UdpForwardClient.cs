using System;
using System.Collections.Generic;
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

        private UdpClient mainUdpClient = null;
        private IPEndPoint serverEndPoint = null;
        Thread heartBeat = null;
        //<编号，分配的UDP对象>
        private Dictionary<int, UdpClient> clientList = null;

        private bool flagClose = false;

        public UdpForwardClient(string ip, int port)
        {
            IPEndPoint server = new IPEndPoint(IPAddress.Parse(ip), port);
            mainUdpClient = new UdpClient(0);
            //解决UDP报错问题，详见 https://www.cnblogs.com/pasoraku/p/5612105.html
            //在linux上无此问题
            uint IOC_IN = 0x80000000;
            uint IOC_VENDOR = 0x18000000;
            uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
            mainUdpClient.Client.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
            //
            this.serverEndPoint = server;
            this.serverIP = ip;
            this.serverPort = port;
            //发送心跳
            heartBeat = new Thread(HeartBeat);
            heartBeat.Start();
            //开始转发
            mainUdpClient.BeginReceive(new AsyncCallback(ReadComplete), null);
            //发起握手，发送10次，防止丢包
            byte[] message = Model.Encode(Model.Client_Arrive_Handshake);
            for (int i = 0; i < 10; i++)
            {
                mainUdpClient.Send(message, message.Length, serverEndPoint);
            }
        }

        private void ReadComplete(IAsyncResult ar)
        {
            //处理从服务端收到的数据
            if (flagClose)
            {
                return;//销毁转发
            }

            IPEndPoint newEndPoint = null;
            byte[] buffer = mainUdpClient.EndReceive(ar, ref newEndPoint);

            if (buffer.Length > 0)
            {


                //TODO 重写

                //if (newEndPoint.Equals(serverEndPoint))
                //{
                //    mainUdpClient.Send(buffer, buffer.Length, new IPEndPoint(IPAddress.Parse("127.0.0.1"), localPort));
                //}
                //else /*if (newEndPoint.Equals(localEndPoint))*/
                //{
                //    mainUdpClient.Send(buffer, buffer.Length, serverEndPoint);
                //}
            }
            //完成时调用自身
            mainUdpClient.BeginReceive(new AsyncCallback(ReadComplete), null);
        }

        private void ReceiveMessag(int index, byte[] buffer)
        {
            //接收本地UdpClient的回传数据


        }

        private void HeartBeat()
        {
            while (true)
            {
                if (mainUdpClient != null && serverEndPoint != null)
                {
                    //UDP心跳包，用于保持连接
                    mainUdpClient.Send(Model.Heartbeat, Model.Heartbeat.Length, serverEndPoint);
                }
                //循环发送
                Thread.Sleep(1000);
            }
        }

        public void Close()//结束心跳
        {
            heartBeat.Abort();
            flagClose = true;
        }
    }
}
