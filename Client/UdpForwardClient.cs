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
            this.server = server;
            //TODO 摸鱼中，下次再写


            Thread heartBeat = new Thread(HeartBeat);
            heartBeat.Start();
        }

        private void HeartBeat()
        {
            while (true)
            {
                if (udpClient != null && server != null)
                {
                    //UDP心跳包，用于保持连接
                    udpClient.Send(Model.Client_Heart_Beat, Model.Client_Heart_Beat.Length, server);
                }
                //循环发送
                Thread.Sleep(100);
            }
        }
    }
}
