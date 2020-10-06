using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using static SQLite.SQLiteConnection;

namespace SQLiteBrowser.Models
{
    public class Row
    {
        Table Table;
        public List<Cell> Cells { get; set; } = new List<Cell>();

        public Row(List<Cell> cells, Table table)
        {
            Cells = cells;
            Table = table;
        }
    }
}
