using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using SQLite;
using SQLiteAppTools.Extensions;
using SQLiteAppTools.Models;

namespace SQLiteAppTools
{
    public static class SQLiteAppTools
    {
        public static string DatabasePath { get; set; }
        public static void Init(string databasePath)
        {
            DatabasePath = databasePath;
        }
    }

    public class TableService
    {
        readonly SQLiteAsyncConnection Connection;
        readonly SQLiteConnection ConnectionSync;

        public TableService() : this(SQLiteAppTools.DatabasePath) { }

        public TableService(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                if(string.IsNullOrEmpty(SQLiteAppTools.DatabasePath))
                    throw new InvalidOperationException("Database path not set. Please pass a database to SQLiteAppTools.Init(path) or pass a path to new BrowserPage(path)");
                path = SQLiteAppTools.DatabasePath;
            }
            Connection = new SQLiteAsyncConnection(path);
            ConnectionSync = new SQLiteConnection(path);
        }

        public async Task<List<Table>> GetAll()
        {
            var tables = await Connection.QueryAsync<Table>("select * from sqlite_master where type = 'table';");
            return tables;
        }

        public async Task<Table> LoadData(Table table)
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
