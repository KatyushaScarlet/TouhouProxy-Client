using System;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class UdpForwardSubClient
    {
        //UDP消息回传
        public delegate void ReceiveMessageFromUdpClient(int index, byte[] buffer);
        public event ReceiveMessageFromUdpClient ReceiveMessageFromUdpClientEvent;

        private UdpClient udpClient = null;
        private IPEndPoint localEndpoint = null;

        private int Index = -1;
        public int index { get => index; }

        private int localPort = 10800;
        private bool flagClose = false;

        public UdpForwardSubClient(int index)
        {
            udpClient = new UdpClient(0);
            //解决UDP报错问题，详见 https://www.cnblogs.com/pasoraku/p/5612105.html
            //在linux上无此问题
            uint IOC_IN = 0x80000000;
            uint IOC_VENDOR = 0x18000000;
            uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
            udpClient.Client.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
            //
            Index = index;
            localEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), localPort);

            udpClient.BeginReceive(new AsyncCallback(ReadComplete), null);
        }

        private void ReadComplete(IAsyncResult ar)
        {
            if (flagClose)
            {
                return;//销毁转发
            }

            IPEndPoint newEndPoint = null;
            byte[] buffer = udpClient.EndReceive(ar, ref newEndPoint);
            //TODO 
            if (buffer.Length > 0)
            {
                //回传数据（带上index）
                if (ReceiveMessageFromUdpClientEvent!=null)
                {
                    ReceiveMessageFromUdpClientEvent(Index, buffer);
                }
            }

            //完成时调用自身
            udpClient.BeginReceive(new AsyncCallback(ReadComplete), null);
        }

        public void SendMessage(byte[] message)
        {
            if (localEndpoint!=null)
            {
                udpClient.Send(message, message.Length, localEndpoint);
            }
        }

        public void Close()//关闭
        {
            flagClose = true;
        }
    }
}
