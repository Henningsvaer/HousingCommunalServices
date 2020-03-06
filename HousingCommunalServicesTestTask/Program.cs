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
                throw new Exception();
            }

            var manager = new HousingCommunalServicesManager(user);
            var consoleReport = new ConsoleReport();

            manager.Report(consoleReport, "Приложение запущено.");

            var version = manager.GetDatabaseVersion();
            manager.Report(consoleReport, version);

            var size = manager.GetDatabaseSize();
            manager.Report(consoleReport, size);
        }
    }
}
