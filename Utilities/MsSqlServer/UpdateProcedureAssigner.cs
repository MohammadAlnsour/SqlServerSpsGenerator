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
    public static class UpdateProcedureAssigner
    {
        public static string GetUpdateProcedureParametersWithEqual(string tableName, string dbName)
        {
            var connectionStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            if (string.IsNullOrEmpty(connectionStr)) connectionStr = "data source=(local); user Id=sa; password=P@ssw0rd;";

            var connector = new SqlServerConnector();
            connector.OpenConnection(new SqlServerConnection(connectionStr));

            var primaryKeyName = PrimaryKeyGetter.GetTablePrimaryKeyName(tableName, dbName);

            SqlCommand command = new SqlCommand("SELECT * FROM " + dbName + ".INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'" + tableName + "'",
                connector.Connection.connection);
            var reader = command.ExecuteReader(CommandBehavior.CloseConnection);

            string updateParametersWithEqual = string.Empty;

            while (reader.Read())
            {
                if (reader[3].ToString().ToLower() != primaryKeyName.ToLower())
                {
                    updateParametersWithEqual += reader[3].ToString() + " = @" + reader[3].ToString() + ",";
                }
                //columns.Add(new TableColumn()
                //{
                //    ColumnName = reader[3].ToString(),
                //    ColumnLimit = reader[8] != DBNull.Value ? reader[8].ToString() : "",
                //    ColumnType = GetColumnType(reader[7].ToString())
                //});
            }
            updateParametersWithEqual = updateParametersWithEqual.Substring(0, updateParametersWithEqual.Length - 1);
            connector.CloseConnection();
            return updateParametersWithEqual;
        }

    }

}
