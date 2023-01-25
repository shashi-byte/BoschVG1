using System;
using NPoco;

namespace BoschVG1.DAL
{
    public class DatabaseConnection
    {
        public string ConnectionString => _ConnectionString;
        public DatabaseType SqlDatabaseType => _SqlDatabaseType;

        // [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static string _ConnectionString;
        private static DatabaseType _SqlDatabaseType;

        public DatabaseConnection(string connectionString)
        {
            _SqlDatabaseType = DatabaseType.SqlServer2008;

            if (string.IsNullOrEmpty(connectionString))
                return;

            _ConnectionString = connectionString;
        }

        public Database GetDatabase()
        {
            return new Database(_ConnectionString, DatabaseType.SqlServer2008, System.Data.SqlClient.SqlClientFactory.Instance);// Database(ConnectionString, SqlDatabaseType);
        }
    }
}
