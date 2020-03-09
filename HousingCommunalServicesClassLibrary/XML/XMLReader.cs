using System;
using System.Xml;

namespace HousingCommunalServicesClassLibrary.XML
{
    public static class XMLReader
    {
        public static User[] Read(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException();

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);

            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;

            int childsCount = xRoot.ChildNodes.Count;
            User[] users = new User[childsCount];

            int currentUserNumber = 0;
            // обход всех узлов в корневом элементе
            foreach (XmlNode xnode in xRoot)
            {
                // обходим все дочерние узлы элемента user
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    switch (childnode.Name)
                    {
                        case "host":
                            users[currentUserNumber].HostName = childnode.InnerText;
                            break;
                        case "username":
                            users[currentUserNumber].Username = childnode.InnerText;
                            break;
                        case "password":
                            users[currentUserNumber].Password = childnode.InnerText;
                            break;
                        case "database":
                            users[currentUserNumber].DatabaseName = childnode.InnerText;
                            break;
                        case "freediskspace":
                            users[currentUserNumber].FreeDiskSpace = childnode.InnerText;
                            break;
                        case "range":
                            users[currentUserNumber].Range = childnode.InnerText;
                            break;
                        default:
                            break;
                    }
                }
                currentUserNumber++;
            }
            return users;
        }
    }
}
