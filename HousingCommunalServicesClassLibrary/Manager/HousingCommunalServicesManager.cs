using HousingCommunalServicesClassLibrary.XML;
using Npgsql;
using System;

namespace HousingCommunalServicesClassLibrary
{
    // Получение информации о бд.
    public sealed class HousingCommunalServicesManager : IDisposable
    {
        private readonly string _connectionString;
        private readonly string _host;
        private readonly string _username;
        private readonly string _password;
        private readonly string _database;

        public string DatabaseName { get => _database; }
        private HousingCommunalServicesManager()
        { }

        public HousingCommunalServicesManager(User user)
        {
            _host = user.Host;
            _username = user.Username;
            _password = user.Password;
            _database = user.Database;

            // Инит. connectionString.
            _connectionString = $"Host={_host};Username={_username};Password={_password};Database={_database}";
        }

        public string GetDatabaseSize()
        {
            var size = MakeSQLCommand($"SELECT pg_size_pretty(pg_database_size('{_database}'));");
            return size;
        }

        public string GetDatabaseVersion()
        {
            var version = MakeSQLCommand("SELECT version()");
            return version;
        }

        public void Report(IReport report, string message)
        {
            report.Display(message);
        }

        private string MakeSQLCommand(string request)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var command = new NpgsqlCommand(request, connection);

            var result = command.ExecuteScalar().ToString();
            return result;
        }

        public void Dispose()
        {
            
        }
    }
}
