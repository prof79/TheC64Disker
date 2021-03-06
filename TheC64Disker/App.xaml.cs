﻿//----------------------------------------------------------------------------
// <copyright file="App.xaml.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      Interaction logic for App.xaml.
// </description>
// <version>v1.0.0 2018-06-13T00:43:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker
{
    using System.Windows;
    using Bootstrappers;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Overridden Methods

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootstrapper =
                new TheC64DiskerBootstrapper();

            bootstrapper.Run();
        }

        #endregion
    }
}
