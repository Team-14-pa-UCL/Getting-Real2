using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace UMOVEWPF.Converters
{
    /// <summary>
    /// Konverterer null til Visibility.Collapsed og ikke-null til Visibility.Visible.
    /// Bruges til at skjule/visse UI-elementer baseret på binding.
    /// </summary>
    public class NullToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Returnerer Collapsed hvis value er null, ellers Visible.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Konverterer en bool til en farve (rød hvis kritisk, grøn ellers).
    /// Bruges til at farve tekst eller indikatorer i UI.
    /// </summary>
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCritical)
            {
                return isCritical ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Green);
            }
            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Konverterer en enum-type til en liste af værdier, så de kan bruges i ComboBox.
    /// </summary>
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

    /// <summary>
    /// Konverterer batteriniveau (double) til en farve til brug i ProgressBar.
    /// Grøn: >60%, Gul: 30-60%, Rød: 13-30%, Sort: <13%.
    /// </summary>
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