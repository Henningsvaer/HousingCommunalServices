using System;
using HousingCommunalServicesClassLibrary;

namespace HousingCommunalServicesTestTask
{
    internal sealed class Program
    {
        static void Main(string[] args)
        {
            var manager = new HousingCommunalServicesManager();
            var consoleReport = new ConsoleReport();

            manager.Report(consoleReport, "Приложение запущено.");

            var version = manager.GetDatabaseVersion();
            manager.Report(consoleReport, version);

            var size = manager.GetDatabaseSize();
            manager.Report(consoleReport, size);
        }
    }
}
