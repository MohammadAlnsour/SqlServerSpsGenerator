using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Utilities.MsSqlServer
{
    public class SqlServerConnector : IConnector<SqlServerConnection>
    {
       // private SqlServerConnection sqlConn;
        public void OpenConnection(SqlServerConnection dbServerProps)
        {
            Connection = new SqlServerConnection(dbServerProps.ConnectionString);
        }
        public void CloseConnection()
        {
            if (Connection != null)
            {
                Connection.connection.Close();
            }
        }

        public SqlServerConnection Connection { get; set; }

    }
}
