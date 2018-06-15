//----------------------------------------------------------------------------
// <copyright file="MyConfirmationWindow.xaml.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      Interaction logic for MyConfirmationWindow.xaml.
// </description>
// <version>v0.5.0 2018-06-14T22:44:00+02</version>
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
    using Prism.Interactivity.InteractionRequest;

    /// <summary>
    /// Interaction logic for MyConfirmationWindow.xaml.
    /// </summary>
    public partial class MyConfirmationWindow : Window
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="MyConfirmationWindow"/>.
        /// </summary>
        public MyConfirmationWindow()
        {
            InitializeComponent();

            ConfirmationIcon.Source = LoadIcon();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Sets or gets the <see cref="IConfirmation"/> shown by this window.
        /// </summary>
        public IConfirmation Confirmation
        {
            get => DataContext as IConfirmation;

            set => DataContext = value;
        }

        #endregion

        #region Methods

        private ImageSource LoadIcon()
        {
            var iconHandle = System.Drawing.SystemIcons.Question.Handle;

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

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Confirmation.Confirmed = true;

            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Confirmation.Confirmed = false;

            Close();
        }

        #endregion
    }
}
