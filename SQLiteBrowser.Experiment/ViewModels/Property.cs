using System;

namespace SQLiteBrowser.ViewModels
{
    public class Property
    {
        public ColumnHeader ColumnHeader { get; set; }  
        public Type Type => Value.GetType();
        public int Thing { get; set; }
        public object Value { get; set; }
        public string Text => Value != null ? Value.ToString() : "null" ;
    }
}