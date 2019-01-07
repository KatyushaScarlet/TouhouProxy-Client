using System.Linq;
using System.Text;

namespace Client
{
    public class Model
    {
        public const string Client_Arrive_Haneshake = "Client_Arrive_Handshake";
        public const string Client_Second_Handshake = "Client_Second_Handshake";
        public const string Client_Change_Port = "Client_Change_Port";
        public const string Client_Heartbeat = "Client_Heartbeat";

        public const string Server_New_Port = "Server_New_Port";
        public const string Server_Proxy_Start = "Server_Proxy_Start";

        public static byte[] Encode(params object[] args)
        {
            string output = "";
            for (int i = 0; i < args.Count(); i++)
            {
                output += args[i].ToString() + "|";
            }
            return Encoding.UTF8.GetBytes(output);
        }

        public static string[] Decode(int count, byte[] args)
        {
            string input = Encoding.UTF8.GetString(args, 0, count);
            string[] output = input.Split('|');
            return output;
        }

        //static string GetIP()//获取本机IP
        //{
        //    string ip = "";
        //    try
        //    {
        //        WebRequest wr = WebRequest.Create("http://whatismyip.akamai.com");
        //        Stream s = wr.GetResponse().GetResponseStream();
        //        StreamReader sr = new StreamReader(s, Encoding.Default);
        //        ip = sr.ReadToEnd();
        //        sr.Close();
        //        s.Close();
        //    }
        //    catch
        //    {
        //        ip = Dns.GetHostEntry(Dns.GetHostName()).ToString();
        //    }
        //    return ip;
        //}
    }
}
