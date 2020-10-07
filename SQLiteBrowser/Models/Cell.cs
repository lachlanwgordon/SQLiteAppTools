using System;
using Xamarin.Forms;

namespace SQLiteBrowser.Models
{
    public class Cell
    {
        object Item;
        public Column Column { get; set; }

        public Cell(object item, Column column)
        {
            Column = column;
            Item = item;
            column.CheckForMaxLength(item);

        }

        public string DisplayText
        {
            get
            {
                if(Column.IsDate)
                {
                    return new DateTime((long)Item).ToString();
                }
                return Item?.ToString();
            }
        }

        internal bool Matches(string searchTerm)
        {
            if (DisplayText != null && DisplayText.ToLower().Contains(searchTerm.ToLower()))
                return true;
            return false;
        }

        //This is the only property that means the models depend on Xamarin Forms
        //Consider moving
        //public int Alignment { get; set; }
        //public TextAlignment Alignment { get; set; }
        public bool IsVisible => Item != null;

        public override string ToString()
        {
            return DisplayText;
        }
    }
    
}
