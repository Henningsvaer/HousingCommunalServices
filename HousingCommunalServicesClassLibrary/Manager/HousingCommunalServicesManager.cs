using HousingCommunalServicesClassLibrary.XML;
using Npgsql;
using System;

namespace HousingCommunalServicesClassLibrary
{
    public sealed class HousingCommunalServicesManager
    {
        private readonly string _connectionString;
        private readonly string _host;
        private readonly string _username;
        private readonly string _password;
        private readonly string _database;

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
            return $"PostgreSQL db size: {_database} -- {size}";
        }

        public string GetDatabaseVersion()
        {
            var version = MakeSQLCommand("SELECT version()");
            return $"PostgreSQL version: {version}";
        }

        public void Report(IReport report, string message)
        {
            report.Display(message);
        }

        private string MakeSQLCommand(string command)
        {
            using var con = new NpgsqlConnection(_connectionString);
            con.Open();

            var sql = command;

            using var cmd = new NpgsqlCommand(sql, con);

            var result = cmd.ExecuteScalar().ToString();
            return result;
        }
    }
}
