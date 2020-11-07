using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using SQLiteAppTools.Extensions;
using SQLiteAppTools.Models;
using static SQLite.SQLite3;

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


        //A lot of this method is based on ExecuteDeferredQuery https://github.com/praeclarum/sqlite-net/blob/master/src/SQLite.cs
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
                var rows = new List<Row>();
                var stmt = PrepareStatement(command);

                while (Step((SQLitePCL.sqlite3_stmt)stmt) == Result.Row)
                {
                    var cells = new List<Cell>();
                    for (int i = 0; i < table.Columns.Count(); i++)
                    {
                        var column = table.Columns[i];
                        if (!column.IsInitialized)
                        {
                            InitializaColumn(table, stmt, i, column);
                        }
                        var val = ReadCell(command, stmt, i, column);

                        cells.Add(new Cell(val, column));
                    }
                    rows.Add(new Row(cells, table));
                }

                table.Rows = rows;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Couldn't load data from {table.Name}\n{ex} \n {ex.StackTrace}");
                throw;
            }

            return table;
        }

        private static object PrepareStatement(SQLiteCommand command)
        {
            var prepareMethod = command.GetType().GetMethod("Prepare", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            if (prepareMethod == null)
                throw new MissingMethodException("Could not find Prepare() on SQLiteCommand, are you using a different version of SQLite-Net-PCL?");

            var stmt = prepareMethod.Invoke(command, null);
            //var stmt = command.Prepare();//If public can avoid the reflection
            return stmt;
        }

        private static object ReadCell(SQLiteCommand command, object stmt, int i, Column column)
        {
            var readColMethod = command.GetType().GetMethod("ReadCol", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            if (readColMethod == null)
                throw new MissingMethodException("Could not find ReadCol() on SQLiteCommand, are you using a different version of SQLite-Net-PCL?");

            var val = readColMethod.Invoke(command, new object[] { stmt, i, column.ColType, column.CLRType });
            //var val = command.ReadCol(stmt, i, colType, clrType);//if public can avoid the reflection
            return val;
        }

        private static void InitializaColumn(Table table, object stmt, int i, Column column)
        {
            column.Table = table;
            column.IsInitialized = true;
            column.ColType = ColumnType((SQLitePCL.sqlite3_stmt)stmt, i);
            column.CLRType = GetTypeFromColType(column.ColType);
            if (column.Name.ToLower().Contains("date") && column.ColType == ColType.Integer)
            {
                column.IsDate = true;
                column.CLRType = typeof(long);
            }
        }

        public static Type GetTypeFromColType(ColType colType)
        {
            switch (colType)
            {
                case ColType.Integer:
                    return typeof(int);
                case ColType.Float:
                    return typeof(float);
                case ColType.Text:
                    return typeof(string);
                case ColType.Blob:
                    return typeof(byte[]);//not sure about this...
                case ColType.Null:
                    return typeof(object);//Happens whenever there is a null in the db,
                default:
                    return typeof(object);//When does this happen?
            }
        }

    }
}
