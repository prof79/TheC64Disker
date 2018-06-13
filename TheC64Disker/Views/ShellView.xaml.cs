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

    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window, Interfaces.IShell
    {
        public ShellView()
            => InitializeComponent();
    }
}
