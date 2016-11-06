using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public interface IConnector<T> where T : IDataSource
    {
        void OpenConnection(T dbServerProps);
        void CloseConnection();

    }

}
