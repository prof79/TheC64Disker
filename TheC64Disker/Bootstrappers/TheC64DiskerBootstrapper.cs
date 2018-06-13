//----------------------------------------------------------------------------
// <copyright file="TheC64DiskerBootstrapper.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      The Prism bootstrapper for the TheC64Disker WPF application.
// </description>
// <version>v0.9.2 2018-06-13T01:14:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.Bootstrappers
{
    using System.Windows;
    using Microsoft.Practices.Unity;
    using Prism.Unity;

    /// <summary>
    /// The Prism bootstrapper for the TheC64Disker WPF application.
    /// </summary>
    public class TheC64DiskerBootstrapper : UnityBootstrapper
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Overridden Methods

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<Interfaces.IShell, Views.ShellView>(
                new ContainerControlledLifetimeManager());
        }

        protected override DependencyObject CreateShell()
            => Container.TryResolve<Interfaces.IShell>() as DependencyObject;

        protected override void InitializeShell()
            => App.Current.MainWindow.Show();

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            // Register the disk selection view module
            var diskSelectionModuleType = typeof(Modules.DiskSelectionModule);

            var diskSelectionModule = new Prism.Modularity.ModuleInfo
            {
                ModuleName = diskSelectionModuleType.Name,
                ModuleType = diskSelectionModuleType.AssemblyQualifiedName,
            };

            ModuleCatalog.AddModule(diskSelectionModule);
        }

        #endregion
    }
}
