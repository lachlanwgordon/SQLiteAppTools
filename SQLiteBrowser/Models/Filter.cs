using System;
namespace SQLiteBrowser.Models
{
    public class Filter
    {
        public string PropertyName { get; set; }
        public string Value { get; set; }
        public Operator Operator { get; set; }
    }

    public enum Operator
    {
        Equals = 0,
        Contains = 1,
        NotEquals,
    }

}
