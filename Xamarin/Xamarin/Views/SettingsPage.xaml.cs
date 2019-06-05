using System;
using Xamarin.Extension;
using Xamarin.ViewModels;

namespace Xamarin.Views
{
    public partial class SettingsPage
    {
        private readonly SettingsViewModel _viewModel;
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SettingsViewModel(this);
        }

        private void SelectPicker(object sender, EventArgs eventArgs)
        {
            _viewModel.SelectStyle(sender);
        }
        private void Image_Clicked(object sender, EventArgs e)
        {
            _ = PopupPhoto.FadeIn();
        }
        private async void GalleryClicked(object sender, EventArgs eventArgs)
        {
            await PopupPhoto.FadeOut();
            _viewModel.SelectGallery();
        }
        private async void CameraClicked(object sender, EventArgs eventArgs)
        {
            await PopupPhoto.FadeOut();
            _viewModel.SelectCamera();
        }
        protected override bool OnBackButtonPressed()
        {
            _ = PopupPhoto.FadeOut();
            return true;
        }

    }
}