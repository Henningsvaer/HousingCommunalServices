using Npgsql;
using System;

namespace HousingCommunalServicesClassLibrary
{
    public sealed class HousingCommunalServicesManager
    {
        private readonly string _connectionString;
        private string _host { get; }
        private string _username { get; }
        private string _password { get; }
        private string _database { get; }

        public HousingCommunalServicesManager()
        {
            _host = "localhost";
            _username = "postgres";
            _password = "root";
            _database = "postgres";
            _connectionString = $"Host=localhost;Username=postgres;Password=root;Database=postgres";
        }

        public HousingCommunalServicesManager(string host, string username, 
            string password, string database)
        {
            if (string.IsNullOrEmpty(host)      || string.IsNullOrEmpty(username) ||
                string.IsNullOrEmpty(password)  || string.IsNullOrEmpty(database))
                throw new ArgumentNullException();

            _host = host;
            _username = username;
            _password = password;
            _database = database;

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
