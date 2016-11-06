using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.MsSqlServer
{
    public static class PrimaryKeyGetter
    {
        public static string GetTablePrimaryKeyName(string tableName, string dbName)
        {
            //SELECT KU.table_name as tablename,column_name as primarykeycolumn FROM WindowsLearner.INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC INNER JOIN WindowsLearner.INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY' AND TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME and ku.table_name='Cources' ORDER BY KU.TABLE_NAME, KU.ORDINAL_POSITION;
            var connectionStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            if (string.IsNullOrEmpty(connectionStr)) connectionStr = "data source=(local); user Id=sa; password=P@ssw0rd;";

            var connector = new SqlServerConnector();
            connector.OpenConnection(new SqlServerConnection(connectionStr));

            string primaryKey = string.Empty;

            if (connector != null)
            {
                SqlCommand command = new SqlCommand("SELECT KU.table_name as tablename, column_name as primarykeycolumn FROM " + dbName + ".INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC INNER JOIN " + dbName + ".INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY' AND TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME and ku.table_name='" + tableName + "' ORDER BY KU.TABLE_NAME, KU.ORDINAL_POSITION",
                    connector.Connection.connection);
                var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                
                while (reader.Read())
                {
                    primaryKey = reader[1].ToString();
                }
            }

            connector.CloseConnection();

            return primaryKey;
        }

    }
}
