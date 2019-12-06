using System;
using System.Collections.Generic;
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
            var width = Math.Max((double)(input * param), 50);

            return (width);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    [ValueConversion(typeof(List<int>), typeof(double))]
    public class StringLengthsToLabelWidth : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<int> == false)
            {
                return default(double);
            }

            var input = (IEnumerable<int>)value;

            if (parameter is double == false)
            {
                return default(double);
            }

            var param = (double)parameter;

            // TODO: Put your value conversion logic here.
            double total = 0;
            foreach (var length in input)
            {
                total += Math.Max((double)(length * param), 50);
            }



            return total;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}