using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerStoredProceduresGenerator
{
    class ConnectionStringRefresher
    {
        public static void RefreshConnectionStringInConfigurationFile(string newConnectionSting)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            connectionStringsSection.ConnectionStrings["default"].ConnectionString = newConnectionSting;
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");
        }
    }
}
