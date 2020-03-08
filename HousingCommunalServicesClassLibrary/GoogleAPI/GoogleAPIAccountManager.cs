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
    public enum ResponseCommand
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
        // Лист!ячейка1:ячейкаN
        private string range = "Лист1!A1:D";
        private string[] Scopes = 
            { SheetsService.Scope.Spreadsheets, SheetsService.Scope.SpreadsheetsReadonly };

        // Значения для таблицы.
        private ValueRange valueRange;

        private GoogleAPIAccountManager()
        { }

        public GoogleAPIAccountManager(
            string applicationName, string credentialFileName = "credentials.json")
        {
            ApplicationName = applicationName;
            CredentialFileName = credentialFileName;

            // Убрать вот это.
            valueRange = new ValueRange();
            var oblist = new List<object>() { "My Cell Text" };
            valueRange.Values = new List<IList<object>> { oblist };
        }

        // Создание таблицы.
        public Spreadsheet CreateSheetInGoogleTables()
        {
            var request = MakeConnectionToGoogleAccount<SpreadsheetsResource.CreateRequest>
                (ResponseCommand.CREATE,"");

            if (request == null)
                throw new NullReferenceException();

            // Выполняем запрос.
            Spreadsheet newSpreadsheet = request.Execute();
            return newSpreadsheet;
        }

        // Обновление таблицы.
        public UpdateValuesResponse UpdateSheetInGoogleTables(string spreadsheetId)
        {
            var request = MakeConnectionToGoogleAccount<SpreadsheetsResource.ValuesResource.UpdateRequest>
                (ResponseCommand.UPDATE, spreadsheetId);
               

            if (request == null)
                throw new NullReferenceException();

            // Выполняем запрос.
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            UpdateValuesResponse result = request.Execute();
            return result;
        }

        // Прочтение данных из таблицы.
        public IList<IList<object>> ReadonlySheetInGoogleTables(string spreadsheetId)
        {
            var request = MakeConnectionToGoogleAccount<SpreadsheetsResource.ValuesResource.GetRequest>
                (ResponseCommand.READ, spreadsheetId);

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
        private T MakeConnectionToGoogleAccount<T>
            (ResponseCommand responseCommand, string spreadsheetId) where T : class
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

            T request = null;
            // Построение запроса.
            switch (responseCommand)
            {
                case ResponseCommand.CREATE:
                    request = service.Spreadsheets.Create(new Spreadsheet()) as T;
                    break;
                case ResponseCommand.UPDATE:
                    request = service.Spreadsheets.Values.Update(valueRange, 
                        spreadsheetId, range) as T;
                    break;
                case ResponseCommand.READ:
                    request = service.Spreadsheets.Values.Get(spreadsheetId, range) as T;
                    break;
            }
            return request;
        }

        public void Dispose()
        {
            
        }
    }
}
