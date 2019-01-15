using System;
using System.Net.Sockets;

namespace Client
{
    public class RemoteServer
    {
        public delegate void GetPortEventHandler(string ip, int port);
        public event GetPortEventHandler GetPortEvent;

        private const int BufferSize = 1024;
        private byte[] buffer;
        private TcpClient client;
        private NetworkStream streamToServer;
        private string serverIP = "";

        public RemoteServer(string serverIP,int serverPort)
        {
            //try
            //{
                client = new TcpClient();
                client.Connect(serverIP, serverPort);      // 与服务器连接
                this.serverIP = serverIP;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    return;
            //}
            buffer = new byte[BufferSize];

            streamToServer = client.GetStream();
        }

        private void SendMessage(byte[] messageSend)
        {
            //try
            //{
                streamToServer.Write(messageSend, 0, messageSend.Length); // 发往服务器
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            lock (streamToServer)
            {
                AsyncCallback callBack = new AsyncCallback(ReadComplete);
                streamToServer.BeginRead(buffer, 0, BufferSize, callBack, null);
            }
        }

        public void GetNewPort()
        {
            byte[] messageSend = Model.Encode(Model.Client_Arrive_Handshake);
            SendMessage(messageSend);
            //获取新端口
        }

        // 读取完成时的回调方法
        private void ReadComplete(IAsyncResult ar)
        {
            int bytesRead;

            try
            {
                lock (streamToServer)
                {
                    bytesRead = streamToServer.EndRead(ar);
                }

                //TODO 服务端断开事件
                if (bytesRead == 0) ;

                string[] messageArrive = Model.Decode(bytesRead, buffer);
                Array.Clear(buffer, 0, buffer.Length);      // 清空缓存，避免脏读

                if (messageArrive[0] == Model.Server_Proxy_Start)
                {
                    //传回分配的端口
                    int remotePort = Convert.ToInt32(messageArrive[1]);

                    //回传端口号
                    if (GetPortEvent!=null)
                    {
                        GetPortEvent(serverIP, remotePort);
                    }

                    //TODO 发送UDP心跳包
                }

                lock (streamToServer)
                {
                    AsyncCallback callBack = new AsyncCallback(ReadComplete);
                    streamToServer.BeginRead(buffer, 0, BufferSize, callBack, null);
                }
            }
            catch (Exception ex)
            {
                if (streamToServer != null)
                    streamToServer.Dispose();
                client.Close();

                Console.WriteLine(ex.Message);
            }
        }
    }
}
