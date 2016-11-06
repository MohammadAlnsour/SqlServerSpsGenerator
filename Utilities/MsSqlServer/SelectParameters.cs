using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Utilities.MsSqlServer
{
    public class SelectParameters : IParameters
    {
        public string GetParameters(string tableName, string dbName)
        {
            var primaryKeyName = PrimaryKeyGetter.GetTablePrimaryKeyName(tableName, dbName);
            string selectParameters = "@" + primaryKeyName + " int";
            return selectParameters;
        }

        public string GetParametersWithoutDataTypes(string tableName, string dbName)
        {
            var primaryKeyName = PrimaryKeyGetter.GetTablePrimaryKeyName(tableName, dbName);
            string selectParameters = "@" + primaryKeyName;
            return selectParameters;
        }

    }
}
