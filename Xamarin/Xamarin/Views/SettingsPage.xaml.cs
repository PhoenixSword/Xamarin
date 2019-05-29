using System;
using System.ComponentModel;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.ViewModels;

namespace Xamarin.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class SettingsPage
    {
        private readonly SettingsViewModel _viewModel;
        private MediaFile SelectedImageFile { get; set; }

        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SettingsViewModel();
            SelectedImage.Source = _viewModel.Profile.ImageSource;
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            _viewModel.Save();
        }

        private async void Image_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Not supported", "Not supported", "Ok");
                return;
            }

            var mediaOptions = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Medium
            };

            SelectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

            if (SelectedImageFile == null) return;
            _viewModel.SelectImage(SelectedImageFile.GetStream());

            SelectedImage.Source = ImageSource.FromStream(() => SelectedImageFile.GetStream());
        }

        private void Switch_OnToggled(object sender, ToggledEventArgs e)
        {
            MessagingCenter.Send<object, int>(this, "Style", e.Value ? 1 : 0);
        }
    }
}