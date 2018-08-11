//----------------------------------------------------------------------------
// <copyright file="DiskSelectionView.xaml.cs"
//      company="Markus M. Egger">
//      Copyright (C) 2018 Markus M. Egger. All rights reserved.
// </copyright>
// <author>Markus M. Egger</author>
// <description>
//      Interaction logic for DiskSelectionView.xaml.
// </description>
// <version>v0.9.0 2018-06-15T22:27:00+02</version>
//----------------------------------------------------------------------------

namespace at.markusegger.Application.TheC64Disker.Views
{
    using Xamarin.Forms;

    /// <summary>
    /// Interaction logic for DiskSelectionView.xaml
    /// </summary>
    public partial class DiskSelectionView : ContentPage
    {
        public DiskSelectionView()
            => InitializeComponent();

        private void ContentPage_Appearing(object sender, System.EventArgs e)
        {
            //DisplayAlert("Test Alert", "Extra long test dljfdljfsljfl fjölafj lköf lökaj flkj ldjlsjfsd dsfflsdfksdfjlsdf slls slkkd", "OK");

            PopUpView.ShowDialog();
        }
    }
}
