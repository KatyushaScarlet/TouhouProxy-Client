using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Linq;
using System.Text;

namespace Client
{
    class UdpForwardClient
    {
        //private string serverIP = "";
        //private int serverPort = 0;

        private UdpClient mainUdpClient = null;
        private IPEndPoint serverEndPoint = null;
        Thread heartBeat = null;
        //<编号，转发对象>
        private Dictionary<int, UdpForwardSubClient> clientList = new Dictionary<int, UdpForwardSubClient>();

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
            serverEndPoint = server;
            //serverIP = ip;
            //serverPort = port;
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
            byte[] buffer = mainUdpClient.EndReceive(ar, ref newEndPoint);//原始数据

            if (buffer.Length > 0)
            {
                string[] messageArrive = Model.Decode(buffer.Length, buffer);//解码数据
                //TODO 重写
                if (newEndPoint.Equals(serverEndPoint) && messageArrive[0] == Model.Game_Data_Forward)//如果来自服务端，并且数据包头符合
                {
                    //获取到达用户的id
                    int index = int.Parse(messageArrive[1]);
                    //byte[] message = Encoding.UTF8.GetBytes(messageArrive[2]);//TODO 根据位数直接截取buffer
                    byte[] message = Model.ByteSplit(buffer, 9);//直接截取buffer位置

                    if (clientList.ContainsKey(index))
                    {
                        //如果转发已经建立，则调用已创建的UdpClient进行转发
                        clientList[index].SendMessage(message);
                    }
                    else
                    {
                        //如果转发没有建立，则新建一个
                        UdpForwardSubClient subClient = new UdpForwardSubClient(index);
                        //绑定回调事件
                        subClient.ReceiveMessageFromUdpClientEvent += ReceiveMessag;

                        clientList.Add(index, subClient);
                        subClient.SendMessage(message);
                    }
                }
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
            //接收本地UdpClient的回传数据，并转发至服务端
            byte[] message = Model.ByteSplice(Model.Encode(Model.Game_Data_Forward, string.Format("{0:0000}", index)), buffer);//序号格式化为4位，在后方带上buffer
            mainUdpClient.Send(message, message.Length, serverEndPoint);

        }

        private void HeartBeat()//心跳
        {
            while (true)
            {
                if (mainUdpClient != null && serverEndPoint != null)
                {
                    //UDP心跳包，用于保持连接
                    mainUdpClient.Send(Model.Heartbeat, Model.Heartbeat.Length, serverEndPoint);
                }
                //循环发送
                Thread.Sleep(10000);
            }
        }

        public void Close()
        {
            heartBeat.Abort();//结束心跳
            flagClose = true;
        }
    }
}
