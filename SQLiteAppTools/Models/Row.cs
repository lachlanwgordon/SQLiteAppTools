using System;
using System.Collections.Generic;
using System.Linq;

namespace SQLiteAppTools.Models
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

        internal bool Matches(string searchTerm)
        {
            if (Cells.Any(x => x.Matches(searchTerm)))
                return true;
            return false;
        }
    }
}
