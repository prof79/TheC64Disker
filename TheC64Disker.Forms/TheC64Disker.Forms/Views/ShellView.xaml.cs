//----------------------------------------------------------------------------
// <copyright file="ShellView.xaml.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      Interaction logic for ShellView.xaml.
// </description>
// <version>v1.0.0 2018-06-13T00:06:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.Views
{
    using System.Windows;
    using Prism.Events;

    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window, Interfaces.IShell
    {
        #region Fields

        private readonly IEventAggregator _eventAggregator;

        #endregion

        #region Constructors

        public ShellView(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            _eventAggregator = eventAggregator;

            _eventAggregator
                .GetEvent<Events.CloseEvent>()
                .Subscribe(OnCloseEvent, ThreadOption.UIThread);
        }

        #endregion

        #region Event Handlers

        private void OnCloseEvent(object arg)
        {
            // Simple filter based on class/interface names.
            if (arg.ToString().Equals(nameof(Interfaces.IShell)))
            {
                Close();
            }
        }

        #endregion
    }
}
