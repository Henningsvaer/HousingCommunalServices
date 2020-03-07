using System;
using HousingCommunalServicesClassLibrary;
using HousingCommunalServicesClassLibrary.XML;

namespace HousingCommunalServicesTestTask
{
    internal sealed class Program
    {
        static void Main(string[] args)
        {
            // Исправить.
            var path = @"C:\Users\DNS\Documents\GitHub\HousingCommunalServices\HousingCommunalServicesTestTask\Config.xml";
            User user;

            try
            {
                user = XMLReader.Read(path);
            }
            catch
            {
                throw new ArgumentException();
            }
            // Так как нам нужен консольный вывод.
            var consoleReport = new ConsoleReport();

            // Инит. новое соединение.
            using var manager = new HousingCommunalServicesManager(user);
            manager.Report(consoleReport, "Приложение запущено.");

            // Получение версии бд.
            var version = manager.GetDatabaseVersion();
            manager.Report(consoleReport, version);

            // Получение размер бд.
            var size = manager.GetDatabaseSize();
            manager.Report(consoleReport, size);



        }
    }
}
