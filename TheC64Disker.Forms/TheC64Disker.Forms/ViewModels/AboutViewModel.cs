//----------------------------------------------------------------------------
// <copyright file="AboutViewModel.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      View-model (data, operations, commands) for the about view.
// </description>
// <version>v0.8.0 2018-06-15T02:27:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.ViewModels
{
    using System;
    using System.Reflection;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Navigation;
    using Xamarin.Forms;
    using global::TheC64Disker.Forms.ViewModels;

    /// <summary>
    /// View-model (data, operations, commands) for the about view.
    /// </summary>
    public class AboutViewModel : ViewModelBase
    {
        #region Fields

        private readonly Assembly _assembly =
            Assembly.GetExecutingAssembly();

        private readonly IEventAggregator _eventAggregator;

        private DelegateCommand _okCommand;
        private DelegateCommand<string> _hyperlinkCommand;

        #endregion

        #region Constructors

        public AboutViewModel(
            INavigationService navigationService,
            IEventAggregator eventAggregator)
            : base(navigationService)
        {
            _eventAggregator = eventAggregator;

            Title = "About";
        }

        #endregion

        #region Properties

        public string License
            => @"MIT License

Copyright (c) 2018 Markus M. Egger

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the ""Software""), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
";

        public string ProductName
            => _assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product;

        public string Version
            => _assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                ?? _assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;

        public string Copyright
            => _assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;

        public string ProductAndVersion
            => $"{ProductName} v{Version}";

        public string ProductAndCopyright
            => $"{ProductName} v{Version}, {Copyright}";

        public DelegateCommand OkCommand
            => _okCommand
                ?? (_okCommand =
                    new DelegateCommand(
                        () =>
                            _eventAggregator
                                .GetEvent<Events.CloseEvent>()
                                .Publish(nameof(AboutViewModel))));

        public DelegateCommand<string> HyperlinkCommand
            => _hyperlinkCommand
                ?? (_hyperlinkCommand =
                    new DelegateCommand<string>(
                        (uri) =>
                            Device.OpenUri(new Uri(uri))));

        #endregion

        #region Methods

        #endregion
    }
}
