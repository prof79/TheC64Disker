//----------------------------------------------------------------------------
// <copyright file="BooleanToActiveStringConverter.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      A Xamarin Forms value converter converting booleans to
//      the word "Active" or an empty string.
// </description>
// <version>v0.9.2x 2018-08-11T15:17:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.Converters
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    public class BooleanToActiveStringConverter
        : IValueConverter
    {
        #region Interface IValueConverter

        /// <inheritdoc />      
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                if (targetType == typeof(string))
                {
                    // TODO: Localization
                    return booleanValue ? "Active" : String.Empty;
                }
            }

            return null;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                if (targetType == typeof(bool))
                {
                    // TODO: Localization
                    if (String.Equals(stringValue, "Active", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion
    }
}
