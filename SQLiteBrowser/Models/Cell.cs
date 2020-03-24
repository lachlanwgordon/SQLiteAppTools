using System;
using System.Collections.Generic;
using static SQLite.SQLiteConnection;

namespace SQLiteBrowser.Models
{
    public class Cell
    {
        object Item;

        public Cell(object item)
        {
            Item = item;
        }

        public String DisplayText => Item?.ToString();
        public bool IsVisible => Item != null;
    }
}
