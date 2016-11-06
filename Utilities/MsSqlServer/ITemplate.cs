using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.MsSqlServer
{
    public interface ITemplate
    {
        string InsertProcedure { get; set; }
        string UpdateProcedure { get; set; }
        string DeleteProcedure { get; set; }
        string SelectProcedure { get; set; }
        string SelectAllProcedure { get; set; }
    }

}
