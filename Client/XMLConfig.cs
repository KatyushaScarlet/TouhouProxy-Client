using System.IO;
using System.Xml.Linq;

namespace Client
{
    public class XMLConfig
    {
        private string userPath = "";

        public XMLConfig(string filePath = "")
        {
            //判断配置文件是否存在，若不存在，则创建
            userPath = filePath;
            if (!File.Exists(filePath))
            {
                XElement root = new XElement("TouhouProxyConfig");
                XElement game = new XElement("Game");
                game.SetElementValue("Path", "");

                root.Add(game);
                root.Save(filePath);
            }
        }

        public string GetGamePath()
        {
            string output = "";
            if (File.Exists(userPath))
            {
                XDocument document = XDocument.Load(userPath);
                XElement root = document.Root;
                XElement game = root.Element("Game");
                XElement path = game.Element("Path");
                //读取路径
                output = path.Value;
            }
            return output;
        }

        public bool SetGamePath(string gamePath)
        {
            bool output = false;
            if (File.Exists(userPath))
            {
                XDocument document = XDocument.Load(userPath);
                XElement root = document.Root;
                XElement game = root.Element("Game");
                XElement path = game.Element("Path");
                //设置新的路径并保存
                path.Value = gamePath;
                root.Save(userPath);
                output = true;
            }
            return output;
        }
    }
}
