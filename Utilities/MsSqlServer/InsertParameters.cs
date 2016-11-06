using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.MsSqlServer
{
    public class InsertParameters : IParameters
    {
        /// <summary>
        /// Gets the insert parameters from columns names and data types.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public string GetParameters(string tableName, string dbName)
        {
            var connectionStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            if (string.IsNullOrEmpty(connectionStr)) connectionStr = "data source=(local); user Id=sa; password=P@ssw0rd;";
            var columnsGetter = new TableColumnsGetter(connectionStr, tableName, dbName);
            var tableColumnsList = columnsGetter.GetTableColumns().ToList();
            // var tableColumns = columnsGetter.GetTableColumnsNames();
            var primaryKeyName = PrimaryKeyGetter.GetTablePrimaryKeyName(tableName, dbName);

            string insertParameters = string.Empty;
            var columnsCount = tableColumnsList.Count();
            for (int index = 0; index <= columnsCount - 1; index++)
            {
                if (tableColumnsList[index].ColumnName.ToLower() != primaryKeyName.ToLower())
                {
                    insertParameters += "@" + tableColumnsList[index].ColumnName + " " + tableColumnsList[index].ColumnType.ToString();
                    var columnLimit = !string.IsNullOrEmpty(tableColumnsList[index].ColumnLimit) ? "(" + tableColumnsList[index].ColumnLimit + ")" : string.Empty;
                    insertParameters += " " + columnLimit;
                    if (index < columnsCount - 1)
                    {
                        insertParameters += " ,";
                    }
                }
            }

            return insertParameters;
        }

        /// <summary>
        /// Get the parameters from columns names without the primary key.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public string GetParametersWithoutDataTypes(string tableName, string dbName)
        {
            var connectionStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            if (string.IsNullOrEmpty(connectionStr)) connectionStr = "data source=(local); user Id=sa; password=P@ssw0rd;";
            var columnsGetter = new TableColumnsGetter(connectionStr, tableName, dbName);
            var tableColumnsList = columnsGetter.GetTableColumns().ToList();
            //var tableColumns = columnsGetter.GetTableColumnsNames();

            string insertParameters = string.Empty;
            var columnsCount = tableColumnsList.Count();

            for (int index = 0; index <= columnsCount - 1; index++)
            {
                insertParameters += "@" + tableColumnsList[index].ColumnName;
                if (index < columnsCount - 1)
                {
                    insertParameters += ", ";
                }
            }

            return insertParameters;
        }

    }
}
