//----------------------------------------------------------------------------
// <copyright file="DiskSelectionModule.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      Disk selection view Prism module.
// </description>
// <version>v0.9.1 2018-06-13T01:14:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.Modules
{
    using Prism.Modularity;
    using Prism.Regions;

    /// <summary>
    /// Disk selection view Prism module.
    /// </summary>
    public class DiskSelectionModule : IModule
    {
        #region Fields

        private readonly IRegionManager _regionManager;

        #endregion

        #region Constructors

        public DiskSelectionModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #endregion

        #region IModule

        public void Initialize()
        {
            //var x = nameof(DiskSelection)
            _regionManager.RegisterViewWithRegion(
                nameof(Views.DiskSelectionView),
                typeof(Views.DiskSelectionView));
        }

        #endregion
    }
}
