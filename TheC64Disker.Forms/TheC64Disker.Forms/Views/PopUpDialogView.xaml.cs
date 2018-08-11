// PopUpDialogView.xaml.cs

namespace at.markusegger.Application.TheC64Disker.Views
{
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    [ContentProperty("DialogContent")]
    public partial class PopUpDialogView : ContentView
	{
        #region Events

        public event EventHandler DialogClosed;
        public event EventHandler DialogShow;
        public event EventHandler DialogClosing;
        public event EventHandler DialogShowing;

        #endregion

        #region Bindable Properties

        public static readonly BindableProperty HeaderTitleProperty =
            BindableProperty
                .Create("HeaderTitle",
                typeof(string),
                typeof(PopUpDialogView),
                String.Empty,
                BindingMode.TwoWay);

        public string HeaderTitle
        {
            get => (string) GetValue(HeaderTitleProperty);

            set => SetValue(HeaderTitleProperty, value);
        }

        #endregion

        #region Constructors

        public PopUpDialogView()
        {
            InitializeComponent();

            PopUpBgLayout.GestureRecognizers.Add(
                new TapGestureRecognizer
                {
                    Command = new Command(HideDialog)
                });

            PopUpDialogClose.GestureRecognizers.Add(
                new TapGestureRecognizer
                {
                    Command = new Command(HideDialog)
                });
        }

        #endregion

        #region Methods

        public void ShowDialog()
        {
            ShowDialogAnimation(PopUpDialogLayout, PopUpBgLayout);
        }

        public void HideDialog()
        {
            HideDialogAnimation(PopUpDialogLayout, PopUpBgLayout);
        }

        #endregion

        #region Properties

        public View DialogContent
        {
            get { return ContentView.Content; }

            set { ContentView.Content = value; }
        }

        #endregion

        #region Event Handlers

        protected virtual void OnDialogClosed(EventArgs e)
        {
            DialogClosed?.Invoke(this, e);
        }

        protected virtual void OnDialogShow(EventArgs e)
        {
            DialogShow?.Invoke(this, e);
        }

        protected virtual void OnDialogClosing(EventArgs e)
        {
            DialogClosing?.Invoke(this, e);
        }

        protected virtual void OnDialogShowing(EventArgs e)
        {
            DialogShowing?.Invoke(this, e);
        }

        #endregion

        #region Animation Helper Methods

        private void ShowDialogAnimation(VisualElement dialog, VisualElement bg)
        {
            dialog.TranslationY = bg.Height;

            bg.IsVisible = true;

            dialog.IsVisible = true;

            // ANIMATIONS

            var showBgAnimation = OpacityAnimation(bg, 0, 0.5);

            var showDialogAnimation = TransLateYAnimation(dialog, bg.Height, 0);

            // EXECUTE ANIMATIONS

            this.Animate("showBg", showBgAnimation, 16, 200, Easing.Linear, (d, f) => { });

            this.Animate("showMenu", showDialogAnimation, 16, 200, Easing.Linear,
                (d, f) =>
                {
                    OnDialogShow(new EventArgs());
                }
            );

            OnDialogShowing(new EventArgs());
        }

        private void HideDialogAnimation(VisualElement dialog, VisualElement bg)
        {
            // ANIMATIONS

            var hideBgAnimation = OpacityAnimation(bg, 0.5, 0);

            var showDialogAnimation = TransLateYAnimation(dialog, 0, bg.Height);

            // EXECUTE ANIMATIONS

            this.Animate("hideBg", hideBgAnimation, 16, 200, Easing.Linear, (d, f) => { });

            this.Animate("hideMenu", showDialogAnimation, 16, 200, Easing.Linear, (d, f) =>

            {
                bg.IsVisible = false;

                dialog.IsVisible = false;

                dialog.TranslationY = PopUpBgLayout.Height;

                OnDialogClosed(new EventArgs());
            });

            OnDialogClosing(new EventArgs());
        }

        private static Animation TransLateYAnimation(VisualElement element, double from, double to)
        {
            return new Animation(d => { element.TranslationY = d; }, from, to);
        }

        private static Animation TransLateXAnimation(VisualElement element, double from, double to)
        {
            return new Animation(d => { element.TranslationX = d; }, from, to);
        }

        private static Animation OpacityAnimation(VisualElement element, double from, double to)
        {
            return new Animation(d => { element.Opacity = d; }, from, to);
        }

        #endregion
    }
}
