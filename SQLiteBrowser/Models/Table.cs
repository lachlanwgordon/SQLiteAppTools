using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SQLite;
using SQLiteBrowser.Extensions;
using static SQLite.SQLite3;
using static SQLite.SQLiteConnection;

namespace SQLiteBrowser.Models
{
    public class Table
    {
		[Column("name")]
        public string Name { get; set; }
        public string sql { get; set; }
        public List<Column> Columns { get; set; }
        public List<Row> Rows { get; set; } = new List<Row>();
    }
}
