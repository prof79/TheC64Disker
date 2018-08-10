//----------------------------------------------------------------------------
// <copyright file="MyNotificationWindow.xaml.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      Interaction logic for MyNotificationWindow.xaml.
// </description>
// <version>v0.5.0 2018-06-14T21:04:00+02</version>
//
// Based on:
//
// Prism's DefaultConfirmationWindow
// https://github.com/PrismLibrary/Prism/tree/master/Source/Wpf/Prism.Wpf/Interactivity/DefaultPopupWindows
//
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.Views.Popups
{
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using at.markusegger.Application.TheC64Disker.Interactivity.InteractionRequest;
    using Prism.Interactivity.InteractionRequest;

    /// <summary>
    /// Interaction logic for MyNotificationWindow.xaml.
    /// </summary>
    public partial class MyNotificationWindow : Window
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="MyNotificationWindow"/>.
        /// </summary>
        public MyNotificationWindow()
            => InitializeComponent();

        #endregion

        #region Properties

        /// <summary>
        /// Sets or gets the <see cref="INotification"/> shown by this window.
        /// </summary>
        public INotification Notification
        {
            get => DataContext as INotification;

            set
            {
                DataContext = value;

                NotificationIcon.Source = LoadIcon();
            }
        }

        #endregion

        #region Methods

        private ImageSource LoadIcon()
        {
            var iconHandle = System.Drawing.SystemIcons.Information.Handle;

            if (Notification is Warning)
            {
                iconHandle = System.Drawing.SystemIcons.Warning.Handle;
            }
            else if (Notification is Error)
            {
                iconHandle = System.Drawing.SystemIcons.Error.Handle;
            }

            var bitmapSource =
                Imaging.CreateBitmapSourceFromHIcon(
                    iconHandle,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

            //bitmapSource.Freeze();

            return bitmapSource;
        }

        #endregion

        #region Event Handlers

        private void OKButton_Click(object sender, RoutedEventArgs e)
            => Close();

        #endregion
    }
}
