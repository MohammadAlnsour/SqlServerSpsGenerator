using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.MsSqlServer
{
    public interface IParameters
    {
        string GetParameters(string tableName, string dbName);
        string GetParametersWithoutDataTypes(string tableName, string dbName);
    }

}
