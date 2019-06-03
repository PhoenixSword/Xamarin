using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private readonly List<string> _styleItems = new List<string>
        {
            "Orange Theme", "Blue Theme", "Pink Theme"
        };
        //private List<AvatarItem> _avatarItems = new List<AvatarItem>
        //{
        //    new AvatarItem {Id = AvatarType.Gallery, Image = "ImageIcon.png", Title = "Gallery"},
        //    new AvatarItem {Id = AvatarType.Camera, Image = "CameraIcon.png", Title = "Camera"}

        //};
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SettingsViewModel();
            SelectedImage.Source = _viewModel.Profile.ImageSource;
            //AvatarPicker.ItemsSource = _avatarItems;
            StylePicker.ItemsSource = _styleItems;
            StylePicker.SelectedIndex = Application.Current.Properties.TryGetValue("Style", out _) ? int.Parse(Application.Current.Properties["Style"].ToString()) : 0;
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            _viewModel.Save();
        }

        private void Image_Clicked(object sender, EventArgs e)
        {
            PopupPhoto.FadeIn();
            //AvatarPicker.Focus();
        }
        private async void GalleryClicked(object sender, EventArgs eventArgs)
        {
            await PopupPhoto.FadeOut();
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
            await PopupPhoto.FadeOut();
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
        private void SelectStyle(object sender, EventArgs e)
        {
            var index = ((Picker)sender).SelectedIndex;
            MessagingCenter.Send<object, int>(this, "Style", index);
        }

        protected override bool OnBackButtonPressed()
        {
            PopupPhoto.FadeOut();
            return true;
        }

    }
}