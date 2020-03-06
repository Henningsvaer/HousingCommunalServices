
namespace HousingCommunalServicesClassLibrary
{
    internal abstract class SQLDatabaseResearcher
    {
        // Найти бд на диске.
        public abstract string SearchDatabaseInHardDrive();

        // Определить тип бд.
        public abstract string DefineDatabaseTypeOf();

        // Определить размер бд.
        public abstract decimal DefineDatabaseSize();
    }
}
