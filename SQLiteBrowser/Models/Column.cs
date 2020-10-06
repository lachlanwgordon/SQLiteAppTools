using System;
using SQLite;
using static SQLite.SQLite3;

namespace SQLiteBrowser.Models
{
    public class Column
    {
		[Column("name")]
		public string Name { get; set; }

		[Column ("type")]
		public string SQLType { get; set; }
		public ColType ColType { get; set; }
		public Type CLRType { get; set; }
	}
}
