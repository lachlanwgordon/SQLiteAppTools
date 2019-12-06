using System;
using System.Globalization;
using SQLiteBrowser.Experiment.Converters;
using Xamarin.Forms;


namespace SQLiteBrowser.Experiment.Converters
{
    [ValueConversion(typeof(double), typeof(bool))]
    public class DoubleToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double == false)
            {
                return default(bool);
            }

            var input = (double)value;
            var param = (object)parameter;

            // TODO: Put your value conversion logic here.

            return default(bool);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}