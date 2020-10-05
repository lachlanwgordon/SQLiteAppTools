using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using static SQLite.SQLiteConnection;

namespace SQLiteBrowser.Models
{
    public class Cell
    {
        object Item;

        public Cell(object item)
        {
            Item = item;

            if(item != null)
            {
                if(int.TryParse(item.ToString(), out int intValue))
                {
                    Alignment = TextAlignment.End;
                }
                else
                {
                    Alignment = TextAlignment.Center;
                }
            }
        }

        public String DisplayText => Item?.ToString();
        public TextAlignment Alignment { get; set; }
        public bool IsVisible => Item != null;


        public Dictionary<string, string> Properties => Item.GetType().GetProperties().ToDictionary(x => x.Name, x => "Test");

        public override string ToString()
        {
            return DisplayText;
        }
    }
}
