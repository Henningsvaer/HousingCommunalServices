using System;
using System.IO;
using HousingCommunalServicesClassLibrary;
using HousingCommunalServicesClassLibrary.GoogleAPI;
using HousingCommunalServicesClassLibrary.XML;

namespace HousingCommunalServicesTestTask
{
    internal sealed class Program
    {
        static void Main(string[] args)
        {
            // Исправить.
            var path = @"C:\Users\DNS\Documents\GitHub\HousingCommunalServices\HousingCommunalServicesTestTask\Config.xml";
            User[] users;

            try
            {
                users = XMLReader.Read(path);
            }
            catch
            {
                throw new ArgumentException();
            }
            // Так как нам нужен консольный вывод.
            var consoleReport = new ConsoleReport();

            // Инит. новое соединение.
            using var manager = new HousingCommunalServicesManager(users[0]);
            manager.Report(consoleReport, "Приложение запущено.");

            // Получение версии бд.
            var version = manager.GetDatabaseVersion();
            manager.Report(consoleReport, $"PostgreSQL version: {manager.DatabaseName} -- {version}");

            // Получение размера бд.
            var size = manager.GetDatabaseSize();
            manager.Report(consoleReport, $"PostgreSQL db size: {manager.DatabaseName} -- {size}");


            // Передать в конструкторе.
            // Id таблицы.
            string spreadsheetId = "1fMYWuwhjYpYou3XrlZYWnuMXcxTiXEfrG9xWGK9wIUQ";
            
            // Название приложения не на англ. == ошибка.
            var googleAccManager = new GoogleAPIAccountManager("Anton App");
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
