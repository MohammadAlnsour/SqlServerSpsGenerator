using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.MsSqlServer
{
    public class TableColumn
    {
        public string ColumnName { get; set; }
        public ColumnsTypes ColumnType { get; set; }
        public string ColumnLimit { get; set; }
        public bool IsPrimaryKey { get; set; }

    }

}
