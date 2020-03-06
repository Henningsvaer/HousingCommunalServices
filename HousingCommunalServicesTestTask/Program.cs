using System;
using HousingCommunalServicesClassLibrary;
using Npgsql;

namespace HousingCommunalServicesTestTask
{
    internal sealed class Program
    {
        static void Main(string[] args)
        {
            var manager = new HousingCommunalServicesManager();
            manager.ConsoleReport();

            var cs = "Host=localhost;Username=postgres;Password=s$cret;Database=testdb";

            using var con = new NpgsqlConnection(cs);
            con.Open();

            var sql = "SELECT version()";

            using var cmd = new NpgsqlCommand(sql, con);

            var version = cmd.ExecuteScalar().ToString();
            Console.WriteLine($"PostgreSQL version: {version}");
        }
    }
}
