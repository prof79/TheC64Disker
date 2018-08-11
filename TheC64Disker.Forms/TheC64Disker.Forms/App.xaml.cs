// App.xaml.cs

using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace at.markusegger.Application.TheC64Disker.Forms
{
    using Prism;
    using Prism.Ioc;
    using Xamarin.Forms;
    using Prism.DryIoc;
    using at.markusegger.Application.TheC64Disker.Views;

    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        #pragma warning disable AsyncFixer01 // Unnecessary async/await usage
        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/DiskSelectionView");
        }
        #pragma warning restore AsyncFixer01 // Unnecessary async/await usage

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<DiskSelectionView>();
            containerRegistry.RegisterForNavigation<AboutView>();
        }
    }
}
