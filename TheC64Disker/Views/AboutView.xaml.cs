//----------------------------------------------------------------------------
// <copyright file="AboutView.xaml.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      Interaction logic for AboutView.xaml.
// </description>
// <version>v0.8.0 2018-06-15T02:26:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.Views
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using Prism.Events;

    /// <summary>
    /// Interaction logic for AboutView.xaml.
    /// </summary>
    public partial class AboutView : UserControl
    {
        #region Fields

        private readonly IEventAggregator _eventAggregator;

        #endregion

        #region Constructors

        public AboutView(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            _eventAggregator = eventAggregator;

            _eventAggregator
                .GetEvent<Events.CloseEvent>()
                .Subscribe(CloseViaParent, ThreadOption.UIThread);
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #endregion

        #region Event Handlers

        private void Link_RequestNavigate(object sender, RequestNavigateEventArgs e)
            => Process.Start(e.Uri.AbsoluteUri);

        private void CloseViaParent(object arg)
        {
            // Filter by simply comparing name conventions.
            // If AboutViewModel requested closing it's ok,
            // otherwise it isn't.
            if (!arg.ToString().StartsWith(nameof(AboutView)))
            {
                return;
            }

            var parent = Parent;

            while (parent is FrameworkElement frameworkElement)
            {
                if (frameworkElement is Window window)
                {
                    window.Close();

                    break;
                }

                parent = frameworkElement.Parent;
            }
        }

        #endregion
    }
}
