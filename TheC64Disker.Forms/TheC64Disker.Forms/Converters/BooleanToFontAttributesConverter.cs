//----------------------------------------------------------------------------
// <copyright file="BooleanToFontAttributesConverter.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      A Xamarin Forms value converter converting booleans to font weights.
// </description>
// <version>v1.1.0 2018-08-10T22:01:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.Converters
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    /// <summary>
    /// A WPF value converter converting booleans to font weights.
    /// </summary>
    public class BooleanToFontAttributesConverter : IValueConverter
    {
        #region IValueConverter

        /// <summary>
        /// Converts a boolean value to a WPF/XAML font weight
        /// (true = bold, false = normal).
        /// </summary>
        /// <param name="value">
        /// The value to convert from.
        /// This has to be a <see cref="Boolean"/> value.
        /// </param>
        /// <param name="targetType">
        /// The type to convert to.
        /// This has to be the type of <see cref="FontAttributes"/>.
        /// </param>
        /// <param name="parameter">
        /// Not used.
        /// </param>
        /// <param name="culture">
        /// The current application culture. Not used.
        /// </param>
        /// <returns>
        /// A <see cref="FontAttributes"/> value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <paramref name="value"/> and
        /// <paramref name="targetType"/> are not of the correct types.
        /// </exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                if (targetType == typeof(FontAttributes))
                {
                    return booleanValue ? FontAttributes.Bold : FontAttributes.None;
                }
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Converting back from font weight to boolean is currently unsupported.
        /// </summary>
        /// <param name="value">
        /// Not used.
        /// </param>
        /// <param name="targetType">
        /// Not used.
        /// </param>
        /// <param name="parameter">
        /// Not used.
        /// </param>
        /// <param name="culture">
        /// Not used.
        /// </param>
        /// <returns>
        /// Not applicable.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
