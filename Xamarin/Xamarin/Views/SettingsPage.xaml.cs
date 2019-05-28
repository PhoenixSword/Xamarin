using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Models;
using Xamarin.Services;
using Xamarin.ViewModels;

namespace Xamarin.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class SettingsPage
    {
        private readonly SettingsViewModel viewModel;
        private MediaFile selectedImageFile { get; set; }

        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new SettingsViewModel();

            selectedImage.Source = viewModel.Profile.ImageSource;
        }
        private void Save_Clicked(object sender, EventArgs e)
        {
            viewModel.Save();
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

            selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

            if (selectedImageFile == null) return;
            viewModel.SelectImage(selectedImageFile.GetStream());

            selectedImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
        }
    }
}