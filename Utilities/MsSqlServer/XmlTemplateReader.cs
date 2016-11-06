using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Utilities.MsSqlServer
{
    public class XmlTemplateReader
    {
        /// <summary>
        /// Read the insert stored procedure template from the xml file and replace the placeholder values.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public string ReadInsertStoredProcedure(string tableName, string dbName)
        {
            var connectionStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            if (string.IsNullOrEmpty(connectionStr)) connectionStr = "data source=(local); user Id=sa; password=P@ssw0rd;";

            var xmlPath = GetXmlFilePath();
            if (!string.IsNullOrEmpty(xmlPath))
            {
                var xdoc = XDocument.Load(xmlPath);
                var insertSp = xdoc.Descendants("SpsTemplate").Descendants("InsertStoredProcedure").FirstOrDefault();
                var columnsName = new TableColumnsGetter(connectionStr, tableName, dbName).GetTableColumnsNames();

                var insertParams = new InsertParameters();
                var insertParamsWithDataTypes = insertParams.GetParameters(tableName, dbName);
                var insertParamsWithoutTypes = insertParams.GetParametersWithoutDataTypes(tableName, dbName);

                if (insertSp != null)
                {
                    var insertSpTemplate = insertSp.Value;
                    insertSpTemplate = insertSpTemplate.Replace("@TableName", tableName);
                    insertSpTemplate = insertSpTemplate.Replace("@ScriptDateTime", DateTime.Now.ToString());
                    insertSpTemplate = insertSpTemplate.Replace("@InsertParameters", insertParamsWithDataTypes);
                    insertSpTemplate = insertSpTemplate.Replace("@Columns", columnsName);
                    insertSpTemplate = insertSpTemplate.Replace("@SelectParameters", insertParamsWithoutTypes);
                    insertSpTemplate = insertSpTemplate.Replace("@PrimaryIDName", PrimaryKeyGetter.GetTablePrimaryKeyName(tableName, dbName));

                    return insertSpTemplate;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Read the update stored procedure template from the xml file and replace the placeholder values.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public string ReadUpdateStoredProcedure(string tableName, string dbName)
        {
            var connectionStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            if (string.IsNullOrEmpty(connectionStr)) connectionStr = "data source=(local); user Id=sa; password=P@ssw0rd;";

            var xmlPath = GetXmlFilePath();

            var xdoc = XDocument.Load(xmlPath);
            var updateSp = xdoc.Descendants("SpsTemplate").Descendants("UpdateStoredProcedure").FirstOrDefault(); //e => e.Name == "UpdateStoredProcedure");
            if (updateSp != null)
            {
                var updateSpTemplate = updateSp.Value;

                var updateParams = new UpdateParameters();
                var updateParamsWithDataTypes = updateParams.GetParameters(tableName, dbName);
                var primaryIdName = PrimaryKeyGetter.GetTablePrimaryKeyName(tableName, dbName);
                var updateColumnsWithEqual = UpdateProcedureAssigner.GetUpdateProcedureParametersWithEqual(tableName, dbName);

                updateSpTemplate = updateSpTemplate.Replace("@TableName", tableName);
                updateSpTemplate = updateSpTemplate.Replace("@ScriptDateTime", DateTime.Now.ToString());
                updateSpTemplate = updateSpTemplate.Replace("@UpdateParameters", updateParamsWithDataTypes);
                updateSpTemplate = updateSpTemplate.Replace("@UpdateColumnsParameters", updateColumnsWithEqual);
                updateSpTemplate = updateSpTemplate.Replace("@PrimaryIDName", primaryIdName);
                updateSpTemplate = updateSpTemplate.Replace("@PrimaryIDParameter", "@" + primaryIdName);

                return updateSpTemplate;
            }

            return string.Empty;
        }

        /// <summary>
        /// Read the select stored procedure template from the xml file and replace the placeholder values.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public string ReadSelectStoredProcedure(string tableName, string dbName)
        {
            var connectionStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            if (string.IsNullOrEmpty(connectionStr)) connectionStr = "data source=(local); user Id=sa; password=P@ssw0rd;";

            var xmlPath = GetXmlFilePath();
            var xdoc = XDocument.Load(xmlPath);
            var selectSp = xdoc.Descendants("SpsTemplate").Descendants("SelectStoredProcedure").FirstOrDefault();

            if (selectSp != null)
            {
                var selectStoredProcedure = selectSp.Value;

                var selectParams = new SelectParameters();
                var selectParamsWithDataTypes = selectParams.GetParameters(tableName, dbName);
                var selectParam = selectParams.GetParametersWithoutDataTypes(tableName, dbName);

                selectStoredProcedure = selectStoredProcedure.Replace("@TableName", tableName);
                selectStoredProcedure = selectStoredProcedure.Replace("@ScriptDateTime", DateTime.Now.ToString());
                selectStoredProcedure = selectStoredProcedure.Replace("@SelectParameters", selectParamsWithDataTypes);
                selectStoredProcedure = selectStoredProcedure.Replace("@PrimaryIDName", selectParam.Replace("@",""));
                selectStoredProcedure = selectStoredProcedure.Replace("@PrimaryIDParameter", selectParam);

                return selectStoredProcedure;
            }

            return string.Empty;
        }

        /// <summary>
        /// Read the select all stored procedure template from the xml file and replace the placeholder values.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public string ReadSelectAllStoredProcedure(string tableName, string dbName)
        {
            var connectionStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            if (string.IsNullOrEmpty(connectionStr)) connectionStr = "data source=(local); user Id=sa; password=P@ssw0rd;";

            var xmlPath = GetXmlFilePath();
            var xdoc = XDocument.Load(xmlPath);
            var selectSp = xdoc.Descendants("SpsTemplate").Descendants("SelectAllStoredProcedure").FirstOrDefault();

            if (selectSp != null)
            {
                var selectAllSp = selectSp.Value;
                var primaryKeyName = PrimaryKeyGetter.GetTablePrimaryKeyName(tableName, dbName);

                selectAllSp = selectAllSp.Replace("@TableName", tableName);
                selectAllSp = selectAllSp.Replace("@ScriptDateTime", DateTime.Now.ToString());
                selectAllSp = selectAllSp.Replace("@PrimaryIDName", primaryKeyName);
                return selectAllSp;
            }

            return string.Empty;
        }

        /// <summary>
        /// Read the select all stored procedure template from the xml file and replace the placeholder values.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public string ReadDeleteStoredProcedure(string tableName, string dbName)
        {
            var connectionStr = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            if (string.IsNullOrEmpty(connectionStr)) connectionStr = "data source=(local); user Id=sa; password=P@ssw0rd;";

            var xmlPath = GetXmlFilePath();
            var xdoc = XDocument.Load(xmlPath);
            var selectSp = xdoc.Descendants("SpsTemplate").Descendants("DeleteStoredProcedure").FirstOrDefault();

            if (selectSp != null)
            {
                var deleteSp = selectSp.Value;
                var primaryKeyName = PrimaryKeyGetter.GetTablePrimaryKeyName(tableName, dbName);

                deleteSp = deleteSp.Replace("@TableName", tableName);
                deleteSp = deleteSp.Replace("@ScriptDateTime", DateTime.Now.ToString());
                deleteSp = deleteSp.Replace("@DeleteParameters", "@" + primaryKeyName + " int");
                deleteSp = deleteSp.Replace("@PrimaryIDName", primaryKeyName);
                deleteSp = deleteSp.Replace("@PrimaryIDParameter", "@" + primaryKeyName);
                return deleteSp;
            }

            return string.Empty;
        }

        private string GetXmlFilePath()
        {
            var fileName = ConfigurationManager.AppSettings["StoredProceduresTemplates"];
            if (string.IsNullOrEmpty(fileName)) fileName = "SpTemplates.xml";
            var fullLocation = Assembly.GetExecutingAssembly().Location;

            if (fullLocation != null)
            {
                var xmlPath = fullLocation.Substring(0, fullLocation.LastIndexOf("\\")) + "\\" + fileName;
                return xmlPath;
            }
            return string.Empty;
        }

    }
}
