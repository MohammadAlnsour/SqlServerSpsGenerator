using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.MsSqlServer
{
    public class TableColumnsGetter
    {
        private string _tableName;
        private string _databaseName;
        private string _connectionString;
        private string _primaryKeyName;

        public TableColumnsGetter(string conStr, string tableName, string dbName)
        {
            _tableName = tableName;
            _databaseName = dbName;
            _connectionString = conStr;
            _primaryKeyName = PrimaryKeyGetter.GetTablePrimaryKeyName(_tableName, _databaseName);
        }
        public IEnumerable<TableColumn> GetTableColumns()
        {
            List<TableColumn> columns = new List<TableColumn>();

            var connector = new SqlServerConnector();
            connector.OpenConnection(new SqlServerConnection(_connectionString));

            SqlCommand command = new SqlCommand("SELECT * FROM " + _databaseName + ".INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'" + _tableName + "'",
                connector.Connection.connection);
            var reader = command.ExecuteReader(CommandBehavior.CloseConnection);

            while (reader.Read())
            {
                if (reader[3].ToString().ToLower() != _primaryKeyName.ToLower())
                {
                    columns.Add(new TableColumn()
                    {
                        ColumnName = reader[3].ToString(),
                        ColumnLimit = reader[8] != DBNull.Value ? reader[8].ToString() : "",
                        ColumnType = GetColumnType(reader[7].ToString())
                    });
                }
            }

            connector.CloseConnection();
            return columns;
        }
        public string GetTableColumnsNames()
        {
            string columns = string.Empty;
            var connector = new SqlServerConnector();
            connector.OpenConnection(new SqlServerConnection(_connectionString));


            SqlCommand command = new SqlCommand("SELECT * FROM " + _databaseName + ".INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'" + _tableName + "'",
                connector.Connection.connection);
            var reader = command.ExecuteReader(CommandBehavior.CloseConnection);

            //columns += "(";
            while (reader.Read())
            {
                if (reader[3].ToString().ToLower() != _primaryKeyName.ToLower())
                {
                    columns += reader[3].ToString() + ",";
                }
            }
            columns = columns.Substring(0, columns.Length - 1);
            //columns += ")";

            connector.CloseConnection();
            return columns;
        }

        private ColumnsTypes GetColumnType(string columnType)
        {
            switch (columnType.ToLower())
            {
                case "int": return ColumnsTypes.Int;
                    break;
                case "nvarchar": return ColumnsTypes.Nvarchar;
                    break;
                case "datetime": return ColumnsTypes.Datetime;
                    break;
                case "varchar": return ColumnsTypes.Varchar;
                    break;
                case "bit": return ColumnsTypes.Bit;
                    break;
                case "varbinary": return ColumnsTypes.Varbinary;
                    break;
                case "bigint": return ColumnsTypes.Bigint;
                    break;
                case "binary": return ColumnsTypes.Binary;
                    break;
                case "Char": return ColumnsTypes.Char;
                    break;
                case "decimal": return ColumnsTypes.Decimal;
                    break;
                case "float": return ColumnsTypes.Float;
                    break;
                case "image": return ColumnsTypes.Image;
                    break;
                case "money": return ColumnsTypes.Money;
                    break;
                case "nchar": return ColumnsTypes.Nchar;
                    break;
                case "ntext": return ColumnsTypes.Ntext;
                    break;
                case "numeric": return ColumnsTypes.Numeric;
                    break;
                case "real": return ColumnsTypes.Real;
                    break;
                case "smalldatetime": return ColumnsTypes.Smalldatetime;
                    break;
                case "smallint": return ColumnsTypes.Smallint;
                    break;
                case "smallmoney": return ColumnsTypes.Smallmoney;
                    break;
                case "sql_variant": return ColumnsTypes.Sql_variant;
                    break;
                case "text": return ColumnsTypes.Text;
                    break;
                case "time": return ColumnsTypes.Time;
                    break;
                case "timestamp": return ColumnsTypes.Timestamp;
                    break;
                case "tinyint": return ColumnsTypes.Tinyint;
                    break;
                case "xml": return ColumnsTypes.Xml;
                    break;
                default: return ColumnsTypes.Nvarchar;
                    break;
            }
        }

    }
}
