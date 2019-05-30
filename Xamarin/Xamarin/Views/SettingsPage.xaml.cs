using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Extension;
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
            if (Application.Current.Properties.TryGetValue("Style", out var result))
            {
                Toggle.IsToggled = Convert.ToBoolean((int)Application.Current.Properties["Style"]);
            }
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            _viewModel.Save();
        }

        private void Image_Clicked(object sender, EventArgs e)
        {
            PopupPhoto.FadeIn();
        }
        private async void GalleryClicked(object sender, EventArgs eventArgs)
        {
            PopupPhoto.FadeOut();
            await CrossMedia.Current.Initialize();
            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                var mediaOptions = new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Medium
                };
                SelectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);
            }

            if (SelectedImageFile == null) return;
            _viewModel.SelectImage(SelectedImageFile.GetStream());

            SelectedImage.Source = ImageSource.FromStream(() => SelectedImageFile.GetStream());
        }
        private async void CameraClicked(object sender, EventArgs eventArgs)
        {
            PopupPhoto.FadeOut();
            await CrossMedia.Current.Initialize();
            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                var cameraOptions = new StoreCameraMediaOptions
                {
                    Directory = "my_images",
                    Name = $"{DateTime.Now:dd.MM.yyyy_hh.mm.ss}.jpg"
                };

                SelectedImageFile = await CrossMedia.Current.TakePhotoAsync(cameraOptions);
            }
            if (SelectedImageFile == null) return;
            _viewModel.SelectImage(SelectedImageFile.GetStream());

            SelectedImage.Source = ImageSource.FromStream(() => SelectedImageFile.GetStream());

        }
        private void Switch_OnToggled(object sender, ToggledEventArgs e)
        {
            MessagingCenter.Send<object, int>(this, "Style", e.Value ? 1 : 0);
        }

        protected override bool OnBackButtonPressed()
        {
            PopupPhoto.FadeOut();
            return true;
        }
    }
}