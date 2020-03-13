using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using static SQLite.SQLiteConnection;

namespace SQLiteBrowser.Models
{
    public class Row
    {
        public Double Width => Cells.Count * 100;
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

        public Cell Cell0 => Cells.ElementAtOrDefault(0) ?? BlankCell;
        public Cell Cell1 => Cells.ElementAtOrDefault(1) ?? BlankCell;
        public Cell Cell2 => Cells.ElementAtOrDefault(2) ?? BlankCell;
        public Cell Cell3 => Cells.ElementAtOrDefault(3) ?? BlankCell;
        public Cell Cell4 => Cells.ElementAtOrDefault(4) ?? BlankCell;
        public Cell Cell5 => Cells.ElementAtOrDefault(5) ?? BlankCell;
        public Cell Cell6 => Cells.ElementAtOrDefault(6) ?? BlankCell;
        public Cell Cell7 => Cells.ElementAtOrDefault(7) ?? BlankCell;
        public Cell Cell8 => Cells.ElementAtOrDefault(8) ?? BlankCell;
        public Cell Cell9 => Cells.ElementAtOrDefault(9) ?? BlankCell;
        public Cell Cell10 => Cells.ElementAtOrDefault(10) ?? BlankCell;
        public Cell Cell11 => Cells.ElementAtOrDefault(11) ?? BlankCell;
        public Cell Cell12 => Cells.ElementAtOrDefault(12) ?? BlankCell;
        public Cell Cell13 => Cells.ElementAtOrDefault(13) ?? BlankCell;
        public Cell Cell14 => Cells.ElementAtOrDefault(14) ?? BlankCell;
        public Cell Cell15 => Cells.ElementAtOrDefault(15) ?? BlankCell;
        public Cell Cell16 => Cells.ElementAtOrDefault(16) ?? BlankCell;
        public Cell Cell17 => Cells.ElementAtOrDefault(17) ?? BlankCell;

        public Cell BlankCell = new Cell(null);
    }
}
