using System;
using Xamarin.Forms;

namespace SQLiteBrowser.ViewModels
{
    public class Property
    {
        public ColumnHeader ColumnHeader { get; set; }  
        public Type Type => Value.GetType();
        public int Thing { get; set; }
        public object Value { get; set; }
        public string Text => Value != null ? Value.ToString() : "null" ;
        public TextAlignment TextAlignment => Value.GetType().IsNumericType() ? TextAlignment.End : TextAlignment.Start;
    }

    public static class TypeChecking
    {
        public static bool IsNumericType(this object o)
        {
            var type = o.GetType();
            var typeCode = Type.GetTypeCode(type);
            switch (typeCode)
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                case TypeCode.DateTime:
                    return true;
                default:
                    return false;
            }
        }
    }
}