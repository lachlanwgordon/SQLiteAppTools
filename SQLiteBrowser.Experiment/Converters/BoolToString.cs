using System;
using System.Globalization;
using SQLiteBrowser.Experiment.Converters;
using Xamarin.Forms;


namespace SQLiteBrowser.Experiment.Converters
{
    [ValueConversion(typeof(bool), typeof(string))]
    public class BoolToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool == false)
            {
                return default(string);
            }

            var input = (bool)value;
            var param = (object)parameter;

            // TODO: Put your value conversion logic here.

            return default(string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}