using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.MsSqlServer
{
    public class ServerDBs
    {
        private SqlServerConnector connector;
        public ServerDBs(string connectionString)
        {
            connector = new SqlServerConnector();
            connector.OpenConnection(new SqlServerConnection(connectionString));
        }

        public IEnumerable<string> GetServerDBs()
        {
            List<string> dbsNames = new List<string>();
            if (connector != null)
            {
                SqlCommand command = new SqlCommand("SELECT name FROM master.dbo.sysdatabases where name not in('master','tempdb', 'model', 'msdb', 'ReportServer','ReportServerTempDB')",
                    connector.Connection.connection);
                var reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                while (reader.Read())
                {
                  dbsNames.Add(reader[0].ToString());  
                }
            }
            return dbsNames;
        }

    }
}
