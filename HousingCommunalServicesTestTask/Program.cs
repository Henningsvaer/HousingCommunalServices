using System;
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

            // Получение размер бд.
            var size = manager.GetDatabaseSize();
            manager.Report(consoleReport, $"PostgreSQL db size: {manager.DatabaseName} -- {size}");

            var x = new GoogleAPIAccountManager("bla-bla app");

        }
    }
}
