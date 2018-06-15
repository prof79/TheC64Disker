//----------------------------------------------------------------------------
// <copyright file="BooleanToVisibilityConverter.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      Value converter to convert from Boolean to Visibility.
// </description>
// <version>v0.8.0 2018-06-15T02:55:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Value converter to convert from <see cref="Boolean"/> to <see cref="Visibility"/>
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                if (targetType == typeof(Visibility))
                {
                    return
                        (booleanValue ? Visibility.Visible : Visibility.Collapsed);
                }
            }

            throw new InvalidOperationException(
                $"Combination of {nameof(value)}/{nameof(targetType)} not supported by {nameof(BooleanToVisibilityConverter)}.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(
                $"{nameof(ConvertBack)} not supported by {nameof(BooleanToVisibilityConverter)}.");
        }

        #endregion
    }
}
