using System;
using System.Globalization;
using SQLiteBrowser.Experiment.Converters;
using Xamarin.Forms;


namespace SQLiteBrowser.Experiment.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    public class IntToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int == false)
            {
                return default(string);
            }

            var input = (int)value;
            var param = (double)parameter;

            // TODO: Put your value conversion logic here.

            return default(string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}