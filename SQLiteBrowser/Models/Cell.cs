using System;
using Xamarin.Forms;

namespace SQLiteBrowser.Models
{
    public class Cell
    {
        object Item;
        private Column Column;

        public Cell(object item, Column column)
        {
            Column = column;
            Item = item;

            if (item != null && column != null)
            {
                if(column.CLRType == typeof(int))
                {
                    Alignment = TextAlignment.End;
                }
                else
                {
                    Alignment = TextAlignment.Center;
                }
            }
        }

        public string DisplayText => Item?.ToString();

        //This is the only property that means the models depend on Xamarin Forms
        //Consider moving
        public TextAlignment Alignment { get; set; }
        public bool IsVisible => Item != null;

        public override string ToString()
        {
            return DisplayText;
        }
    }
}
