using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using SQLiteBrowser.Models;
using static SQLite.SQLite3;

namespace SQLiteBrowser.Extensions
{
    public static class SQLiteExtensions
    {
        //A slight variation of the same method in https://github.com/praeclarum/sqlite-net/blob/master/src/SQLite.cs
        //But I need one that doesn't need a TableMapping, because I don't have Classes
        //There's some dodgy reflection in here to call a private method
        //This method is gradually gaining more and more knowledge of my models, rather than being purely DBish
        //Perhaps a refactor is coming.
        public static List<Row> ExecuteDeferredQuery(this SQLiteCommand command, Table table, SQLiteConnection connection)
        {
            if (connection.Trace)
            {
                connection.Tracer?.Invoke("Executing Query: " + command);
            }

            var rows = new List<Row>();

            var prepareMethod = command.GetType().GetMethod("Prepare", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            if (prepareMethod == null)
                throw new MissingMethodException("Could not find Prepare() on SQLiteCommand, are you using a different version of SQLite-Net-PCL?");

            var stmt = prepareMethod.Invoke(command, null);
            //var stmt = command.Prepare();//If public can avoid the reflection

            while (SQLite3.Step((SQLitePCL.sqlite3_stmt)stmt) == SQLite3.Result.Row)
            {
                var cells = new List<Cell>();
                for (int i = 0; i < table.Columns.Count(); i++)
                {
                    var column = table.Columns[i];
                    var colType = SQLite3.ColumnType((SQLitePCL.sqlite3_stmt)stmt, i);
                    var clrType = GetTypeFromColType(colType);

                    //These are set a lot of times
                    //TODO: Optimise?
                    column.ColType = colType;
                    column.CLRType = clrType;

                    var readColMethod = command.GetType().GetMethod("ReadCol", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                    if (readColMethod == null)
                        throw new MissingMethodException("Could not find ReadCol() on SQLiteCommand, are you using a different version of SQLite-Net-PCL?");

                    var val = readColMethod.Invoke(command, new object[] { stmt, i, colType, clrType });
                    //var val = command.ReadCol(stmt, i, colType, clrType);//if public can avoid the reflection

                    cells.Add(new Cell(val, column));
                }
                rows.Add(new Row(cells, table));
            }

            return rows;
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