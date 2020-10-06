using System;
using System.Collections.Generic;
using SQLite;

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
