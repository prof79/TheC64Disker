﻿//----------------------------------------------------------------------------
// <copyright file="DiskSelectionViewModel.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      View-model for the disk selection view.
// </description>
// <version>v0.9.0 2018-06-15T22:16:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Navigation;
    using global::TheC64Disker.Forms.ViewModels;
    using at.markusegger.Application.TheC64Disker.Interactivity.InteractionRequest;
    using Models;
    using Utility;
    using System.Threading.Tasks;
    using Prism.Services;
    using System.Threading;

    public class DiskSelectionViewModel : ViewModelBase
    {
        #region Fields

        private readonly IEventAggregator _eventAggregator;
        private readonly IPageDialogService _pageDialogService;

        private DelegateCommand _activateCommand;
        private DelegateCommand _refreshCommand;
        private DelegateCommand _aboutCommand;
        private DelegateCommand _debugCommand;

        private InteractionRequest<INotification> _aboutNotificationRequest;
        private InteractionRequest<INotification> _customNotificationRequest;
        private InteractionRequest<IConfirmation> _customConfirmationRequest;

        private IEnumerable<DiskImage> _diskImages;
        private DiskImage _selectedItem;
        private string _statusMessage;
        private string _lastError;

        #endregion

        #region Constructors

        public DiskSelectionViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator,
            IPageDialogService pageDialogService)
            : base(navigationService)
        {
            _eventAggregator = eventAggregator;

            // TODO: UIThread Option problem
            _eventAggregator
                .GetEvent<Events.DiskImageEnumerationError>()
                .Subscribe(OnDiskImageError /*, ThreadOption.UIThread*/);

            _pageDialogService = pageDialogService;

            Title = "Disk Selection";

            InitInteractionRequestHandlers();
        }

        #endregion

        #region Properties

        public bool DebuggingEnabled
#if DEBUG
            => true;
#else
            => false;
#endif

        public bool ShowHelp
            => DiskImages == null
                || DiskImages?.Count() == 0;

        public bool CanActivate
            => SelectedItem != null
                && SelectedItem?.IsActive == false;

        public DelegateCommand ActivateCommand
            => _activateCommand
                ?? (_activateCommand =
                    new DelegateCommand(
                        OnActivateCommand,
                        () => CanActivate));

        public DelegateCommand RefreshCommand
            => _refreshCommand
                ?? (_refreshCommand =
                    new DelegateCommand(
                        RefreshImages));

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
                if (_diskImages == null)
                {
                    try
                    {
                        _diskImages =
                            TheC64Helper
                              .GetDiskImages()
                              .Where(image => !image.IsC64MiniDefaultImage)
                              .OrderBy(image => image.Name);

                        RaisePropertyChanged(nameof(ShowHelp));
                    }
                    catch (Exception ex)
                    {
                        _eventAggregator
                            .GetEvent<Events.DiskImageEnumerationError>()
                            .Publish(ex);
                    }
                }

                return _diskImages;
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

        private void InitInteractionRequestHandlers()
        {
            CustomNotificationRequest.Raised
                += HandleInteractionRequest;

            CustomConfirmationRequest.Raised
                += HandleInteractionRequest;
        }

        #endregion

        #region Event Handlers

        private async void HandleInteractionRequest(object sender, InteractionRequestedEventArgs e)
        {
            if (e == null)
            {
                return;
            }

            if (e.Context is IConfirmation confirmation)
            {
                confirmation.Confirmed =
                    await _pageDialogService
                        .DisplayAlertAsync(
                            confirmation.Title,
                            confirmation.Content.ToString(),
                            "Yes",
                            "No");
            }
            else if (e.Context is INotification notification)
            {
                await _pageDialogService
                    .DisplayAlertAsync(
                        notification.Title,
                        notification.Content.ToString(),
                        "OK");
            }
            else
            {
                throw new InvalidOperationException(
                    $"{nameof(e.Context)} is neither {nameof(INotification)} nor {nameof(IConfirmation)}.");
            }
        }

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

        private void RefreshImages()
        {
            // Setting the backing field to null will force the
            // property to re-initialize on next access.
            _diskImages = null;

            // Now notify the consumers to update their bindings.
            // This will cause a property access and thus a new
            // load operation.
            RaisePropertyChanged(nameof(DiskImages));
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
                        RefreshImages();

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

        #pragma warning disable AsyncFixer01 // Unnecessary async/await usage
        private async void OnAboutCommand()
        {
            // TODO: Interaction replacement
            //AboutNotificationRequest.Raise(
            //    new Notification
            //    {
            //        Title = "About TheC64Disker"
            //        // Content will be filled by the interaction trigger
            //        // and content type
            //    }
            //);

            await NavigationService.NavigateAsync("NavigationPage/AboutView");
        }
        #pragma warning restore AsyncFixer01 // Unnecessary async/await usage

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

            // Test notifications

            var warning = new Warning
            {
                Title = "No Selection",
                Content = "You must select an item first."
            };

            //CustomNotificationRequest.Raise(warning);

            var confirmation = new Confirmation
            {
                Title = "Test confirmation",
                Content = "Do you confirm?"
            };

            //CustomConfirmationRequest.Raise(confirmation);

            var error = new Error
            {
                Title = "Error",
                Content = "Unhandled exception in test error."
            };

            //CustomNotificationRequest.Raise(error);

            CustomNotificationRequest.Raise(notification,
                (n1) =>
                {
                    if (SelectedItem != null)
                    {
                        CustomNotificationRequest.Raise(new Notification
                        {
                            Title = "Selection",
                            Content = $"You selected: {SelectedItem.FullName}"
                        },
                        (n2) => CustomNotificationRequest.Raise(warning,
                            (n3) => CustomConfirmationRequest.Raise(confirmation,
                                (n4) => CustomNotificationRequest.Raise(error)))
                        );
                    }
                }
            );

            var provokeErrorOnRemoval = DiskImages?.Count();
        }

        #endregion
    }
}
