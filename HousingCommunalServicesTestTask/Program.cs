using System;
using System.IO;
using HousingCommunalServicesClassLibrary;
using HousingCommunalServicesClassLibrary.GoogleAPI;
using HousingCommunalServicesClassLibrary.XML;

namespace HousingCommunalServicesTestTask
{
    internal sealed class Program
    {
        static decimal KBToGB(string size)
        {
            string temp = string.Empty;
            for(int i = 0; size[i] != ' ' && i != size.Length - 1; i++ )
            {
                temp += size[i];
            }

            decimal decimalSize = 0;
            try
            {
                decimalSize = decimal.Parse(temp);
            }
            catch { Console.WriteLine($"{temp} невозможно преобразовать в ГБ."); return decimalSize; }

            decimal divide = 1048576;
            return decimalSize / divide;
        }

        static private decimal GetTotalFreeSpace()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady == true)
                {
                    return d.TotalFreeSpace;
                }
            }
            return -1;
        }

        static void Main(string[] args)
        {
            // Файл с настройками пользователей.
            var fullPath = Path.GetFullPath(@"users_config.xml");
            User[] users;

            try
            {
                users = XMLReader.Read(fullPath);
            }
            catch
            {
                throw new ArgumentException();
            }

            // Инит. новое соединение.
            using var manager = new HousingCommunalServicesManager(users[0]);
            manager.Report(new ConsoleReport(), "Приложение запущено.");

            // Получение версии бд.
            var version = manager.GetDatabaseVersion();
            manager.Report(new ConsoleReport(), 
                $"PostgreSQL version: {version}");

            // Получение размера бд.
            var size = manager.GetDatabaseSize();
            manager.Report(new ConsoleReport(),
                $"PostgreSQL {manager.DatabaseName} db size: {KBToGB(size).ToString()} GB");

            // Id таблицы.
            string spreadsheetId = "1fMYWuwhjYpYou3XrlZYWnuMXcxTiXEfrG9xWGK9wIUQ";

            // Примечание: Название приложения не на англ. == ошибка.
            var googleAccManager = new GoogleAPIAccountManager
                (
                    "Anton App",
                    users[0].Database,
                    KBToGB(size).ToString(),
                   (GetTotalFreeSpace() / 1073741824).ToString()
                );
            var result = googleAccManager.UpdateSheetInGoogleTables(spreadsheetId);

            return;

            var collection = googleAccManager.ReadonlySheetInGoogleTables(spreadsheetId);

            foreach(var item in collection)
            {
                Console.WriteLine($"{item[0]} | {item[1]} | {item[2]} | {item[3]}");
            }
        }
    }
}
