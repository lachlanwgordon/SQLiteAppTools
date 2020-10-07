using System;
using System.Globalization;
using Xamarin.Forms;

namespace SQLiteBrowser.Converters
{
    public class TypeToAlignmentConverter : IValueConverter
    {
        public static TypeToAlignmentConverter Instance { get; } = new TypeToAlignmentConverter();
        public TypeToAlignmentConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = (Type)value;
            if (input == typeof(int) || input == typeof(double) || input == typeof(long) ||input == typeof(decimal) || input == typeof(float) )
                return TextAlignment.End;
            else
                return TextAlignment.Start;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
