using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace UMOVEWPF.Converters
{
    public class ProgressBarColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double level)
            {
                if (level >= 60)
                    return new SolidColorBrush(Colors.Green);
                if (level >= 30)
                    return new SolidColorBrush(Colors.Gold);
                if (level >= 13)
                    return new SolidColorBrush(Colors.Red);
                return new SolidColorBrush(Colors.Black);
            }
            return new SolidColorBrush(Colors.Gray);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 