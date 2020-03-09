using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using SQLite;
using static SQLite.SQLite3;
using static SQLite.SQLiteConnection;
using Sqlite3Statement = System.IntPtr;



namespace SQLiteBrowser.Models
{
    public class Table
    {
        public string type { get; set; }
        public string name { get; set; }
        public string tbl_name { get; set; }
        public int rootpage { get; set; }
        public string sql { get; set; }
        public IEnumerable<ColumnInfo> ColumnInfos { get; set; }
        public IEnumerable<IEnumerable<object>> Rows { get; set; } = new List<List<object>>();

        public static List<Table> GetAll(SQLiteConnection connection)
        {
            var tables = connection.Query<Table>("select * from sqlite_master where type = 'table';");

            foreach (var table in tables)
            {
                table.LoadData(connection);
            }
            return tables;
        }


        public IEnumerable<IEnumerable<object>> LoadData(SQLiteConnection connection)
        {
            var columns = connection.GetTableInfo(name);
            ColumnInfos = columns;

            var command = new SQLiteCommand(connection);
            command.CommandText = $"select * from '{name}'";
            IEnumerable<IEnumerable<object>> items;
            try
            {
                items = command.ExecuteDeferredQuery(this, connection);
                Rows = items;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Couldn't load data from {name}\n{ex} \n {ex.StackTrace}");
                items = new List<List<object>> { new List<object> { "Error loading data"} };
            }
            
            return items;
        }
    }


    public static class SQLiteExtensions
    {

        public static IEnumerable<IEnumerable<object>> ExecuteDeferredQuery(this SQLiteCommand command, Table table, SQLiteConnection connection)
        {
            if (connection.Trace)
            {
                connection.Tracer?.Invoke("Executing Query: " + command);
            }

            var items = new List<List<object>>();

            var prepareMethod = command.GetType().GetMethod("Prepare", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            if (prepareMethod == null)
                throw new MissingMethodException("Could not find Prepare() on SQLiteCommand, are you using a different version of SQLite-Net-PCL?");

            var stmt = prepareMethod.Invoke(command, null);
            //var stmt = (Sqlite3Statement)prepareMethod.Invoke(command, null);
            //var stmt = command.Prepare();//If public can avoid the reflection
            
            while (SQLite3.Step((SQLitePCL.sqlite3_stmt) stmt) == SQLite3.Result.Row)
            {
                var row = new List<object>();
                for (int i = 0; i < table.ColumnInfos.Count(); i++)
                {
                    var colType = SQLite3.ColumnType((SQLitePCL.sqlite3_stmt)stmt, i);
                    var clrType = GetTypeFromColType(colType);

                    var readColMethod = command.GetType().GetMethod("ReadCol", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                    if (readColMethod == null)
                        throw new MissingMethodException("Could not find ReadCol() on SQLiteCommand, are you using a different version of SQLite-Net-PCL?");

                    var val = readColMethod.Invoke(command, new object[] { stmt, i, colType, clrType });
                    //var val = command.ReadCol(stmt, i, colType, clrType);//if public can avoid the reflection
                    row.Add(val);
                }
                items.Add(row);
            }

            return items;
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
