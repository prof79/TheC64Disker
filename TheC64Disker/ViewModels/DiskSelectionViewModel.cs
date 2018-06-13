//----------------------------------------------------------------------------
// <copyright file="DiskSelectionViewModel.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      View-model for the disk selection view.
// </description>
// <version>v0.8.0 2018-06-13T01:22:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Prism.Commands;
    using Prism.Mvvm;
    using Models;
    using Utility;

    /// <summary>
    /// View-model (data, operations, commands) for the disk selection view.
    /// </summary>
    public class DiskSelectionViewModel : BindableBase
    {
        #region Fields

        private DelegateCommand _activateCommand;
        private DelegateCommand _debugCommand;

        private DiskImage _selectedItem;

        #endregion

        #region Constructors

        #endregion

        #region Properties

        public bool CanActivate
            => SelectedItem != null
                && SelectedItem?.IsActive == false;

        public bool CanDebug
            => true;

        public DelegateCommand ActivateCommand
            => _activateCommand
                ?? (_activateCommand =
                    new DelegateCommand(
                        OnActivateCommand,
                        () => CanActivate));

        public DelegateCommand DebugCommand
            => _debugCommand
                ?? (_debugCommand =
                    new DelegateCommand(
                        OnDebugCommand,
                        () => CanDebug));

        public IEnumerable<DiskImage> DiskImages
            => TheC64Helper.GetDiskImages().Where(image => !image.IsC64MiniDefaultImage);

        public DiskImage SelectedItem
        {
            get => _selectedItem;

            set => SetProperty(ref _selectedItem, value, () => ActivateCommand.RaiseCanExecuteChanged());
        }

        #endregion

        #region Methods

        #endregion

        #region Event Handlers

        private void OnActivateCommand()
        {
            var nl = Environment.NewLine;

            if (SelectedItem == null)
            {
                System.Windows.MessageBox.Show(
                    "You must select an item first.",
                    "No Selection",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Warning);
            }
            else
            {
                if (SelectedItem?.CopyToDefaultImage(OnOverwriteImage) == true)
                {
                    // Success requires updateing the list.
                    RaisePropertyChanged(nameof(DiskImages));

                    System.Windows.MessageBox.Show(
                        $"Image successfully copied to \"{TheC64Helper.TheC64MiniDiskName}\".",
                        "Success",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Information);
                }
                else
                {
                    System.Windows.MessageBox.Show(
                        $"Error copying \"{SelectedItem?.FullName}\" to \"{TheC64Helper.TheC64MiniDiskName}\".",
                        "Error",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Error);
                }
            }
        }

        private void OnDebugCommand()
        {
            var nl = Environment.NewLine;

            var rootPath = TheC64Helper.DriveRoot;

            var message =
                $"Root path: {rootPath}{nl}Exists? {rootPath.Exists()}{nl}Is Directory? {rootPath.IsDirectory()}{nl}Is File? {rootPath.IsFile()}";

            System.Windows.MessageBox.Show(
                message,
                "Root Path Info",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);

            if (SelectedItem != null)
            {
                System.Windows.MessageBox.Show(
                    $"You selected: {SelectedItem.FullName}",
                    "Selection",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Information);
            }
        }

        private bool OnOverwriteImage()
        {
            var message =
                $"The file \"{TheC64Helper.DriveRoot.Combine(TheC64Helper.TheC64MiniDiskName)}\" already exists. Do you want to overwrite it?";

            var result =
                System.Windows.MessageBox.Show(
                    message,
                    "Overwrite File",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Question);

            return result == System.Windows.MessageBoxResult.Yes;
        }

        #endregion
    }
}
