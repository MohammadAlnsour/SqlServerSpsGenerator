using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.MsSqlServer
{
    public class SqlServerConnection : IDataSource
    {
        private string _ConnectionString;
        public string ConnectionString
        {
            get
            {
                if (!string.IsNullOrEmpty(_ConnectionString))
                {
                    return _ConnectionString;
                }
                throw new Exception("Please add a connection string named 'default' to your configuration file.");
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    var connectionStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
                    if (string.IsNullOrEmpty(ConnectionString))
                    {
                        throw new Exception("Please add a connection string named 'default' to your configuration file.");
                    }
                }
                _ConnectionString = value;
            }
        }

        public SqlConnection connection;

        public SqlServerConnection(string connString)
        {
            if (!string.IsNullOrEmpty(connString))
            {
                this.ConnectionString = connString;
            }
            connection = new SqlConnection(this.ConnectionString);
            try
            {
                connection.Open();
            }
            catch (SqlException exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }
        }

    }

}
