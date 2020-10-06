using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using static SQLite.SQLiteConnection;

namespace SQLiteBrowser.Models
{
    public class Row
    {
        List<ColumnInfo> ColumnInfos;
        public List<Cell> Cells { get; set; } = new List<Cell>();
        private IEnumerable<object> items;


        public Row(List<ColumnInfo> columnInfos)
        {
            ColumnInfos = columnInfos;
            foreach (var item in columnInfos)
            {
                Cells.Add(new Cell(item));
            }
        }

        public Row(IEnumerable<object> items, List<ColumnInfo> columnInfos)
        {
            this.items = items;
            ColumnInfos = columnInfos;

            int index = 0;
            foreach (var item in items)
            {

                if(item != null)
                {
                    Cells.Add(new Cell(item));
                }
                else
                {
                    if(columnInfos[index] != null)
                        Cells.Add(new Cell(""));
                    else
                        Cells.Add(new Cell(item));
                }

                index++;
            }
        }
    }
}
