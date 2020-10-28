using System;
using Xamarin.Forms;

namespace SQLiteAppTools.Models
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
                if(Column.IsDate || Column.CLRType == typeof(DateTime))
                {
                    DateTime date;
                    if(Column.CLRType == typeof(int) || Column.CLRType == typeof(long))
                    {
                        date = new DateTime((long)Item);
                    }
                    else
                    {
                        date = (DateTime)Item;
                    }
                    return date.ToString("s");

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

        public bool IsUrl
        {
            get
            {
                Uri uriResult;
                bool result = Uri.TryCreate(DisplayText, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                return result;
            }
        }

        public override string ToString()
        {
            return DisplayText;
        }

    }
    
}
