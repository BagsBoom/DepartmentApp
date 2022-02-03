using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// add mysql connection
using MySql.Data.MySqlClient;

namespace DepartmentApp
{
    class DataBase
    {
        public static string connectionSql()
        {
            string MyConnectionString = "Server=localhost;Database=department;Uid=root;Pwd=;";
            return MyConnectionString;
        }

        public static string connectionSqlLogin()
        {
            string MyConnectionString = "Server=localhost;Database=login;Uid=root;Pwd=;";
            return MyConnectionString;
        }
    }
}
