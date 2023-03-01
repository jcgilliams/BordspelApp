using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace BordspelApp_DAL
{
    public static class DatabaseConnection
    {
        public static string Connectionstring(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
