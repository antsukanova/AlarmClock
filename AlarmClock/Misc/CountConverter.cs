using System;
using System.Globalization;
using System.Windows.Data;

namespace AlarmClock.Misc
{
    public class CountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value == null ? null : $"Alarm clocks: {(int) value - 1}";

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
