// MainWindow.xaml.cs

namespace at.markusegger.Application.TheC64Disker
{
    using System;
    using Prism;
    using Prism.Ioc;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.WPF;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FormsApplicationPage
    {
        public MainWindow()
        {
            InitializeComponent();

            Xamarin.Forms.Forms.Init();

            LoadApplication(new Forms.App(new WpfInitializer()));
        }
    }

    public class WpfInitializer : IPlatformInitializer
    {
        #region Interface IPlatformInitializer

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // TODO: Register platform-specific implementations here
        }

        #endregion
    }
}
