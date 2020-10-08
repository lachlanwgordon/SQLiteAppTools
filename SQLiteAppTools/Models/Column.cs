using System;
using SQLite;
using SQLiteAppTools.Extensions;
using static SQLite.SQLite3;

namespace SQLiteAppTools.Models
{
    public class Column
    {
		[Column("name")]
		public string Name { get; set; }

		[Column ("type")]
		public string SQLType { get; set; }
		public ColType ColType { get; set; }
		public Type CLRType { get; set; }
        public bool IsDate { get; internal set; }
        public bool IsInitialized { get; internal set; }
        public Table Table { get; set; }

        public int MaxLength { get; private set; }

		public void CheckForMaxLength(object val)
        {
            if (MaxLength == 0)
            {
                MaxLength = Name.Length;

            }

            if(CLRType == typeof(string))
            {
                var str = (string)val;
                if (str != null && str.Length > MaxLength)
                    MaxLength = str.Length;
            }
            else if (IsDate)
            {
                MaxLength = val.ToString().Length;
            }
            else if(CLRType == typeof(int) || CLRType == typeof(long))
            {
                var len = ((int)val).Length();
                if (len > MaxLength)
                    MaxLength = len;
            }

        }
    }
}
