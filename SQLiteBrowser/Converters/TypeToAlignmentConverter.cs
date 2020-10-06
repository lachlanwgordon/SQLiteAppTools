using System;
using System.Globalization;
using Xamarin.Forms;

namespace SQLiteBrowser.Converters
{
    public class TypeToAlignmentConverter : IValueConverter
    {
        public TypeToAlignmentConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = (Type)value;
            if (input == typeof(int) || input == typeof(double) || input == typeof(long))
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
