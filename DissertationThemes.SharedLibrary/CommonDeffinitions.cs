using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DissertationThemes.SharedLibrary
{
    /// <summary>
    /// Common deffinitions for DB
    /// </summary>
    internal static class CommonDeffinitions
    {
        /// <summary>
        /// Connection string for DataBase
        /// </summary>
        public static string DbConnectionString = $"Data Source=DESKTOP-MIIMUU5\\MSSQLSERVER01;Initial Catalog=DisertationThemes;Integrated Security=True;Trust Server Certificate=True";
        /// <summary>
        /// Prefix for API request URL
        /// </summary>
        public static string ApiUrlPrefix = "https://localhost:7258/";
    }
}
