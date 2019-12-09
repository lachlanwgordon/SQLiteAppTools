using System;
using System.Globalization;
using SQLiteBrowser.Experiment.Converters;
using Xamarin.Forms;



namespace SQLiteBrowser.Experiment.Converters
{
    [ValueConversion(typeof(object), typeof(object))]
    public class TypeToTextAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Type == false)
            {
                return default(object);
            }

            var input = (Type)value;

            // TODO: Put your value conversion logic here.

            TextAlignment alignment = TextAlignment.Start;

            if(input == typeof(int) || input == typeof(double) || input == typeof(DateTime) || input == typeof(decimal) || input == typeof(TimeSpan))
                    alignment = TextAlignment.End;


            return alignment;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}