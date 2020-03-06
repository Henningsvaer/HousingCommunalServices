using System;
using System.Collections.Generic;
using Npgsql;

namespace HousingCommunalServicesClassLibrary
{
    internal class PostgreSQLDatabaseResearcher : SQLDatabaseResearcher
    {
        // Определить размер бд.
        public override decimal DefineDatabaseSize()
        {
            throw new NotImplementedException();
        }

        // Определить тип бд.
        public override string DefineDatabaseTypeOf()
        {
            throw new NotImplementedException();
        }

        // Найти бд на диске.
        public override string SearchDatabaseInHardDrive()
        {
            // 1.

            throw new NotImplementedException();
        }
    }
}
