using System;
using System.Globalization;
using Xamarin.Forms;

namespace SQLiteAppTools.Converters
{
    public class IntToAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = (int)value;
            if (input == 3)
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