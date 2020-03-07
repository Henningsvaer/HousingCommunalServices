using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace HousingCommunalServicesClassLibrary.GoogleAPI
{
    enum ResponseCommand
    {
        CREATE = 0,
        UPDATE = 1,
        READ = 2,
    }

    public sealed class GoogleAPIAccountManager: IDisposable
    {
        // Результат создания/обновления листа
        // Сервер | БД  | Размер в ГБ | Дата обновления
        // serv1  | db1 | 2.5         | 15.03.2020

        public string ApplicationName { get; }
        public string CredentialFileName { get; }
        // Данные таблицы.
        public string ServerName { get; }
        public string DatabaseName { get; }
        public string DatabaseSize { get; }
        public DateTime LastUpdate { get; }

        // Параметры запроса.
        // Передать в конструкторе.
        // Id таблицы.
        private string spreadsheetId = "1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms";
        // Лист!ячейка1:ячейкаN
        private string range = "Class Data!A2:D";
        private string[] Scopes = 
            { SheetsService.Scope.Spreadsheets, SheetsService.Scope.SpreadsheetsReadonly };

        private GoogleAPIAccountManager()
        { }

        public GoogleAPIAccountManager(
            string applicationName, string credentialFileName = "credentials.json")
        {
            ApplicationName = applicationName;
            CredentialFileName = credentialFileName;
        }

        public void CreateSheetInGoogleTables()
        {
            var request = MakeConnectionToGoogleAccount(ResponseCommand.CREATE);
            if (request == null)
                throw new NullReferenceException();


        }

        public void UpdateSheetInGoogleTables()
        {

        }

        public IList<IList<object>> ReadonlySheetInGoogleTables()
        {
            var request = MakeConnectionToGoogleAccount(ResponseCommand.READ);
            if (request == null)
                throw new NullReferenceException();

            // Выполняем запрос.
            ValueRange response = request.Execute();
            IList<IList<object>> values = response.Values;

            if (values != null && values.Count > 0)
            {
                return values;
            }
            else
            {
                throw new InvalidDataException("No data found.");
            }
        }

        // Соединение с гугл аккаунтом.
        private SpreadsheetsResource.ValuesResource.GetRequest MakeConnectionToGoogleAccount
            (ResponseCommand responseCommand)
        {
            // Учетные данные.
            UserCredential credential;

            using (var stream =
                new FileStream(CredentialFileName, FileMode.Open, FileAccess.Read))
            {
                // Файл token.json сохраняет маркеры доступа и обновления пользователя. 
                // Создается автоматически, когда поток авторизации завершается в первый раз.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Создаем Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Построение запроса.
            SpreadsheetsResource.ValuesResource.GetRequest request = null;
            switch (responseCommand)
            {
                case ResponseCommand.CREATE:
                    break;
                case ResponseCommand.UPDATE:
                    break;
                case ResponseCommand.READ:
                    request = service.Spreadsheets.Values.Get(spreadsheetId, range);
                    break;
                default:
                    return null;
            }
            return request;
        }

        public void Dispose()
        {
            
        }
    }
}
