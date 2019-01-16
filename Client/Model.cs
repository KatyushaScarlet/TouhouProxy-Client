using System.Linq;
using System.Text;

namespace Client
{
    public class Model
    {
        public const string Client_Arrive_Handshake = "CAH";
        public const string Server_Proxy_Start = "SPS";
        public static byte[] Heart_Beat = { 0x0E, 0x03, 0xE4, 0x00, 0x00, 0x00, 0x03, 0x01, 0x00, 0x00 };

        public static byte[] Encode(params object[] args)
        {
            string output = "";
            for (int i = 0; i < args.Length; i++)
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
