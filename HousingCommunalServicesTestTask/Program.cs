using System;
using System.IO;
using System.Threading;
using HousingCommunalServicesClassLibrary;
using HousingCommunalServicesClassLibrary.GoogleAPI;
using HousingCommunalServicesClassLibrary.XML;

namespace HousingCommunalServicesTestTask
{
    internal sealed class Program
    {
        static void Main(string[] args)
        {
            // Время задержки перед повторным обновлением.
            int secondsTimeout = 10;
            // Примечание: Название приложения не на англ. == ошибка.
            string appName = "APP NAME";
            // Id таблицы для обновления.
            string spreadsheetId = "";
            
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

            while (true)
            {
                foreach (var user in users)
                {
                    // Инит. новое соединение.
                    using var manager = new HousingCommunalServicesManager(user);
                    manager.Report(new ConsoleReport(), "Идет соединение...");

                    // Получение версии бд.
                    var version = manager.GetDatabaseVersion();
                    manager.Report(new ConsoleReport(),
                        $"PostgreSQL версия: {version}");

                    // Получение размера бд.
                    var size = manager.GetDatabaseSize();
                    manager.Report(new ConsoleReport(),
                        $"PostgreSQL {manager.DatabaseName} размер бд: {KBToGB(size).ToString()} ГБ");

                    var googleAccManager = new GoogleAPIAccountManager
                        (
                            appName,
                            user,
                            KBToGB(size).ToString()
                        );

                    // Если таблицы нет, то создадим её.
                    if(string.IsNullOrEmpty(spreadsheetId))
                    {
                        var newSpreadSheet = googleAccManager.CreateSheetInGoogleTables();
                        spreadsheetId = newSpreadSheet.SpreadsheetId;
                    }

                    // Вывод ссылки на таблицу в консоль.
                    manager.Report(new ConsoleReport(),
                        $"https://docs.google.com/spreadsheets/d/{spreadsheetId}/edit#gid=0");

                    var result = googleAccManager.UpdateSheetInGoogleTables(spreadsheetId);
                }
                // Ждем и продолжаем цикл.
                Wait(secondsTimeout);
            }
        }
        private static decimal KBToGB(string size)
        {
            string temp = string.Empty;
            for (int i = 0; size[i] != ' ' && i != size.Length - 1; i++)
            {
                temp += size[i];
            }

            decimal decimalSize;
            try
            {
                decimalSize = decimal.Parse(temp);
            }
            catch
            {
                Console.WriteLine($"{temp} невозможно преобразовать в ГБ.");
                return -1;
            }

            decimal divide = 1048576;
            return decimalSize / divide;
        }

        private static void Wait(int secondsTimeout)
        {
            Thread.Sleep(secondsTimeout * 1000);
        }
    }
}
