using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.MsSqlServer
{
    public class DbTablesGetter
    {
        private SqlServerConnector connector;
        private string _dbName;
        //private ServerDBs _serverdbs;

        public DbTablesGetter(string conStr, string dbName)
        {
            _dbName = dbName;
            connector = new SqlServerConnector();
            connector.OpenConnection(new SqlServerConnection(conStr));
        }

        public IEnumerable<string> GetDbTables()
        {
            List<string> tables = new List<string>();
            if (connector != null)
            {
                SqlCommand command = new SqlCommand("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG='" + _dbName + "'",
                    connector.Connection.connection);
                var reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                while (reader.Read())
                {
                    tables.Add(reader[0].ToString());
                }
            }
            if (connector != null) connector.CloseConnection();
            return tables;
        }

    }
}
