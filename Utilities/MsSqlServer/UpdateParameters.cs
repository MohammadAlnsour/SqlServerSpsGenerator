using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Utilities.MsSqlServer
{
    public class UpdateParameters : IParameters
    {
        public string GetParameters(string tableName, string dbName)
        {
            var connectionStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            if (string.IsNullOrEmpty(connectionStr)) connectionStr = "data source=(local); user Id=sa; password=P@ssw0rd;";

            var columnsGetter = new TableColumnsGetter(connectionStr, tableName, dbName);
            var tableColumnsList = columnsGetter.GetTableColumns().ToList();
            //var tableColumns = columnsGetter.GetTableColumnsNames();

            string updateParameters = string.Empty;
            var columnsCount = tableColumnsList.Count();

            for (int index = 0; index <= columnsCount - 1; index++)
            {
                updateParameters += "@" + tableColumnsList[index].ColumnName + " " + tableColumnsList[index].ColumnType.ToString();
                var columnLimit = !string.IsNullOrEmpty(tableColumnsList[index].ColumnLimit) ? "(" + tableColumnsList[index].ColumnLimit + ")" : string.Empty;
                updateParameters += " " + columnLimit;
                if (index < columnsCount - 1)
                {
                    updateParameters += " ,";
                }
            }

            return updateParameters;
        }
        public string GetParametersWithoutDataTypes(string tableName, string dbName)
        {
            throw new NotImplementedException();
        }
        

    }
}
