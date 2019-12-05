using System;
using System.Globalization;
using SQLiteBrowser.Experiment.Converters;
using Xamarin.Forms;


namespace SQLiteBrowser.Experiment.Converters
{
    [ValueConversion(typeof(int), typeof(double))]
    public class StringLengthToLabelWidth : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int == false)
            {
                return default(double);
            }

            var input = (int)value;

            if (parameter is double == false)
            {
                return default(double);
            }

            var param = (double)parameter;

            // TODO: Put your value conversion logic here.


            return (double) (input * param);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}