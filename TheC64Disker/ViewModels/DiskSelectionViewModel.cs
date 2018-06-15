﻿//----------------------------------------------------------------------------
// <copyright file="DiskSelectionViewModel.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      View-model for the disk selection view.
// </description>
// <version>v0.8.0 2018-06-15T03:07:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Prism.Commands;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using Prism.Regions;
    using at.markusegger.Application.TheC64Disker.Interactivity.InteractionRequest;
    using Models;
    using Utility;

    public class DiskSelectionViewModel : BindableBase
    {
        #region Fields

        private readonly IRegionManager _regionManager;

        private DelegateCommand _activateCommand;
        private DelegateCommand _aboutCommand;
        private DelegateCommand _debugCommand;

        private InteractionRequest<INotification> _aboutNotificationRequest;
        private InteractionRequest<INotification> _customNotificationRequest;
        private InteractionRequest<IConfirmation> _customConfirmationRequest;

        private DiskImage _selectedItem;
        private string _statusMessage;

        #endregion

        #region Constructors

        public DiskSelectionViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        #endregion

        #region Properties

        public bool DebuggingEnabled
#if DEBUG
            => true;
#else
            => false;
#endif

        public bool CanActivate
            => SelectedItem != null
                && SelectedItem?.IsActive == false;

        public DelegateCommand ActivateCommand
            => _activateCommand
                ?? (_activateCommand =
                    new DelegateCommand(
                        OnActivateCommand,
                        () => CanActivate));

        public DelegateCommand AboutCommand
            => _aboutCommand
                ?? (_aboutCommand =
                    new DelegateCommand(
                        OnAboutCommand));

        public DelegateCommand DebugCommand
            => _debugCommand
                ?? (_debugCommand =
                    new DelegateCommand(
                        OnDebugCommand));

        public InteractionRequest<INotification> AboutNotificationRequest
            => _aboutNotificationRequest
                ?? (_aboutNotificationRequest =
                    new InteractionRequest<INotification>());

        public InteractionRequest<INotification> CustomNotificationRequest
            => _customNotificationRequest
                ?? (_customNotificationRequest =
                    new InteractionRequest<INotification>());

        public InteractionRequest<IConfirmation> CustomConfirmationRequest
            => _customConfirmationRequest
                ?? (_customConfirmationRequest =
                    new InteractionRequest<IConfirmation>());

        public IEnumerable<DiskImage> DiskImages
            => TheC64Helper.GetDiskImages().Where(image => !image.IsC64MiniDefaultImage);

        public DiskImage SelectedItem
        {
            get => _selectedItem;

            set => SetProperty(ref _selectedItem, value,
                    () =>
                    {
                        ActivateCommand.RaiseCanExecuteChanged();

                        StatusMessage = SelectedItem?.FullName;
                    });
        }

        public string StatusMessage
        {
            get => _statusMessage;

            set => SetProperty(ref _statusMessage, value);
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
                var warning = new Warning
                {
                    Title = "No Selection",
                    Content = "You must select an item first."
                };

                CustomNotificationRequest.Raise(warning);
            }
            else
            {
                switch (SelectedItem.CopyToDefaultImage(OnOverwriteImage))
                {
                    case OperationResult.Success:

                        // Success requires updating the list.
                        RaisePropertyChanged(nameof(DiskImages));

                        const string successTitle = "Success";

                        var successMessage =
                            $"Image successfully copied to \"{TheC64Helper.TheC64MiniDiskName}\".";

                        var successNotification = new Notification
                        {
                            Title = successTitle,
                            Content = successMessage
                        };

                        CustomNotificationRequest.Raise(successNotification);

                        break;

                    case OperationResult.Error:

                        const string errorTitle = "Error";

                        var errorMessage =
                            $"Error copying \"{SelectedItem?.FullName}\" to \"{TheC64Helper.TheC64MiniDiskName}\".";

                        var errorNotification = new Error
                        {
                            Title = errorTitle,
                            Content = errorMessage
                        };

                        CustomNotificationRequest.Raise(errorNotification);

                        break;

                    case OperationResult.Cancelled:
                        // No action required on cancellation.
                        break;
                }
            }
        }

        private bool OnOverwriteImage()
        {
            const string title = "Overwrite File";

            var message =
                $"The file \"{TheC64Helper.DriveRoot.Combine(TheC64Helper.TheC64MiniDiskName)}\" already exists. Do you want to overwrite it?";

            var confirmation = new Confirmation
            {
                Title = title,
                Content = message
            };

            CustomConfirmationRequest.Raise(confirmation);

            return confirmation.Confirmed;
        }

        private void OnAboutCommand()
        {
            AboutNotificationRequest.Raise(
                new Notification
                {
                    Title = "About TheC64Disker"
                    // Content will be filled by the interaction trigger
                    // and content type
                }
            );
        }

        private void OnDebugCommand()
        {
            var nl = Environment.NewLine;

            var rootPath = TheC64Helper.DriveRoot;

            var message =
                $"Root path: {rootPath}{nl}Exists? {rootPath.Exists()}{nl}Is Directory? {rootPath.IsDirectory()}{nl}Is File? {rootPath.IsFile()}";

            var notification = new Notification
            {
                Title = "Root Path Info",
                Content = message
            };

            CustomNotificationRequest.Raise(notification);

            if (SelectedItem != null)
            {
                CustomNotificationRequest.Raise(new Notification
                {
                    Title = "Selection",
                    Content = $"You selected: {SelectedItem.FullName}"
                });
            }

            // Test notifications

            var warning = new Warning
            {
                Title = "No Selection",
                Content = "You must select an item first."
            };

            CustomNotificationRequest.Raise(warning);

            var confirmation = new Confirmation
            {
                Title = "Test confirmation",
                Content = "Do you confirm?"
            };

            CustomConfirmationRequest.Raise(confirmation);

            CustomNotificationRequest.Raise(new Error
            {
                Title = "Error",
                Content = "Unhandled exception in test error."
            });
        }

        #endregion
    }
}
