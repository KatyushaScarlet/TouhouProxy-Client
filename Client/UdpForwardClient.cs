using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    class UdpForwardClient
    {
        private int port = 0;
        private UdpClient udpClient = null;
        private IPEndPoint server = null;

        public UdpForwardClient(string ip,int port)
        {
            IPEndPoint server = new IPEndPoint(IPAddress.Parse(ip), port);
            udpClient = new UdpClient(0);
            this.server = server;
            //发送心跳
            Thread heartBeat = new Thread(HeartBeat);
            heartBeat.Start();
            //开始接收
            udpClient.BeginReceive(new AsyncCallback(ReadComplete), null);
        }

        private void ReadComplete(IAsyncResult ar)
        {
            //TODO 摸鱼中，下次再写
        }

        private void HeartBeat()
        {
            while (true)
            {
                if (udpClient != null && server != null)
                {
                    //UDP心跳包，用于保持连接
                    udpClient.Send(Model.Heart_Beat, Model.Heart_Beat.Length, server);
                }
                //循环发送
                Thread.Sleep(100);
            }
        }

        //public void Close()
        //{

        //}
    }
}
