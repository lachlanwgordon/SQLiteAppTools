using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using SQLite;
using SQLiteAppTools.Extensions;
using SQLiteAppTools.Models;

namespace SQLiteAppTools.Services
{
    public static class TableService
    {
        private static string Path;
        private static SQLiteAsyncConnection Connection;
        private static SQLiteConnection ConnectionSync;

        public static void Init(string path)
        {
            Path = path;
            Connection = new SQLiteAsyncConnection(Path);
            ConnectionSync = new SQLiteConnection(Path);

        }
        public static async Task<List<Table>> GetAll()
        {
            var tables = await Connection.QueryAsync<Table>("select * from sqlite_master where type = 'table';");
            return tables;
        }

        public static async Task<Table> LoadData(Table table)
        {
            var query = "pragma table_info(\"" + table.Name + "\")";
            var columns = await Connection.QueryAsync<Column>(query);
            table.Columns = columns;

            var command = new SQLiteCommand(ConnectionSync)
            {
                CommandText = $"select * from '{table.Name}'"
            };

            try
            {
                var rows = command.ExecuteDeferredQuery(table, ConnectionSync);
                table.Rows = rows;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Couldn't load data from {table.Name}\n{ex} \n {ex.StackTrace}");
            }

            return table;
        }
    }
}
