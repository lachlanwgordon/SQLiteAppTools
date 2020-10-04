using System;
using System.Collections.Generic;
using System.Linq;
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


        public Dictionary<string, string> Properties => Item.GetType().GetProperties().ToDictionary(x => x.Name, x => "Test");

        public override string ToString()
        {
            return DisplayText;
        }
    }
}
