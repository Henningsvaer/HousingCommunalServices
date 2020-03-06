using System;
using System.Xml;

namespace HousingCommunalServicesClassLibrary.XML
{
    public static class XMLReader
    {
        public static User Read(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException();

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);

            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
            User user = new User();

            // обход всех узлов в корневом элементе
            foreach (XmlNode xnode in xRoot)
            {
                // обходим все дочерние узлы элемента user
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    switch (childnode.Name)
                    {
                        case "host":
                            user.Host = childnode.InnerText;
                            break;
                        case "username":
                            user.Username = childnode.InnerText;
                            break;
                        case "password":
                            user.Password = childnode.InnerText;
                            break;
                        case "database":
                            user.Database = childnode.InnerText;
                            break;
                    }
                }
            }
            return user;
        }
    }
}
