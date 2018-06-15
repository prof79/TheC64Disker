//----------------------------------------------------------------------------
// <copyright file="DiskSelectionViewModel.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      View-model for the disk selection view.
// </description>
// <version>v0.8.3 2018-06-15T20:39:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using at.markusegger.Application.TheC64Disker.Interactivity.InteractionRequest;
    using Models;
    using Utility;

    public class DiskSelectionViewModel : BindableBase
    {
        #region Fields

        private readonly IEventAggregator _eventAggregator;

        private DelegateCommand _activateCommand;
        private DelegateCommand _aboutCommand;
        private DelegateCommand _debugCommand;

        private InteractionRequest<INotification> _aboutNotificationRequest;
        private InteractionRequest<INotification> _customNotificationRequest;
        private InteractionRequest<IConfirmation> _customConfirmationRequest;

        private DiskImage _selectedItem;
        private string _statusMessage;
        private string _lastError;

        #endregion

        #region Constructors

        public DiskSelectionViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _eventAggregator
                .GetEvent<Events.DiskImageEnumerationError>()
                .Subscribe(OnDiskImageError, ThreadOption.UIThread);
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
        {
            get
            {
                try
                {
                    return
                        TheC64Helper
                          .GetDiskImages()
                          .Where(image => !image.IsC64MiniDefaultImage)
                          .OrderBy(image => image.Name);
                }
                catch (Exception ex)
                {
                    _eventAggregator
                        .GetEvent<Events.DiskImageEnumerationError>()
                        .Publish(ex);

                    return null;
                }
            }
        }

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

        public string LastError
        {
            get => _lastError;

            set => SetProperty(ref _lastError, value);
        }

        #endregion

        #region Methods

        #endregion

        #region Event Handlers

        private void OnDiskImageError(Exception ex)
        {
            var nl = Environment.NewLine;

            LastError = ex.Message;

            const string title = "Error";

            var message =
                $"There has been an unrecoverable error while looking for C64 disk images under {TheC64Helper.DriveRoot}.{nl}{nl}"
                + $"Details: {LastError}{nl}{nl}"
                + "The program will now exit.";

            var errorNotification = new Error
            {
                Title = title,
                Content = message
            };

            CustomNotificationRequest.Raise(errorNotification);

            // Close application via eventing.
            _eventAggregator
                .GetEvent<Events.CloseEvent>()
                .Publish(nameof(Interfaces.IShell));
        }

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
                switch (SelectedItem.CopyToDefaultImage(OnOverwriteImage, ex => LastError = ex.Message))
                {
                    case OperationResult.Success:

                        // Clear errors
                        LastError = null;

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

                        SelectedItem = null;

                        StatusMessage = LastError;

                        const string errorTitle = "Error";

                        var errorMessage =
                            $"Error copying \"{SelectedItem?.FullName}\" to \"{TheC64Helper.TheC64MiniDiskName}\".{nl}{LastError}";

                        var errorNotification = new Error
                        {
                            Title = errorTitle,
                            Content = errorMessage
                        };

                        CustomNotificationRequest.Raise(errorNotification);

                        break;

                    case OperationResult.Cancelled:

                        SelectedItem = null;

                        LastError = "Copy cancelled.";

                        StatusMessage = LastError;

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

            var x = DiskImages?.Count();
        }

        #endregion
    }
}
