using System;
using System.Globalization;
using System.Windows.Data;

namespace UMOVEWPF.Converters
{
    public class EnumToListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumType = value as Type;
            if (enumType == null && value != null)
                enumType = value.GetType();
            if (enumType != null && enumType.IsEnum)
                return Enum.GetValues(enumType);
            if (parameter is Type t && t.IsEnum)
                return Enum.GetValues(t);
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 