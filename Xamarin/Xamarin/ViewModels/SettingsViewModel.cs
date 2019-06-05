using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Models.Models;
using Xamarin.Services;
using Xamarin.ViewModels.Base;
using Xamarin.Views;

namespace Xamarin.ViewModels
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class SettingsViewModel : BaseViewModel
    {
        readonly DishesService _dishesService = new DishesService();
        private string _id;
        private string Id
        {
            get => _id;
            set
            {
                if (_id == value || value == null) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value || value == null) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        private byte[] _image;
        public byte[] Image
        {
            get => _image;
            set
            {
                if (_image == value || value == null) return;
                _image = value;
                OnPropertyChanged();
            }
        }
        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get => _imageSource;
            set
            {
                if (_imageSource == value || value == null) return;
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        private SettingsPage _page;

        public ObservableCollection<Category> CategoryList;

        private MediaFile _selectedImageFile;
        public MediaFile SelectedImageFile
        {
            get => _selectedImageFile;
            set
            {
                if (_selectedImageFile == value || value == null) return;
                _selectedImageFile = value;
                OnPropertyChanged();
            }
        }

        private int _selectedIndexPicker;
        public int SelectedIndexPicker
        {
            get => _selectedIndexPicker;
            set
            {
                if (_selectedIndexPicker == value) return;
                _selectedIndexPicker = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectStyleCommand => new Command(SelectStyle);
        public ICommand SaveCommand => new Command(Save);

        public SettingsViewModel(SettingsPage page)
        {
            _page = page;
            Title = "Settings";
            Id = Application.Current.Properties["id"].ToString();
            Name = Application.Current.Properties["name"].ToString();
            Image = (byte[]) Application.Current.Properties["image"];
            if (Id != null)
            {
                ImageSource = Image != null
                    ? ImageSource.FromStream(() => new MemoryStream(Image))
                    : ImageSource.FromFile("Profile.jpg");
            }
            else
            {
                Name = "Unknown";
                ImageSource = ImageSource.FromFile("Profile.jpg");
            }
            CategoryList = new ObservableCollection<Category>
            {
                new Category{Id = 0, Name = "Orange Theme"},
                new Category{Id = 1, Name = "Blue Theme"},
                new Category{Id = 2, Name = "Pink Theme"}
            };
            SelectedIndexPicker = Application.Current.Properties.TryGetValue("Style", out _) ? int.Parse(Application.Current.Properties["Style"].ToString()) : 0;

            MessagingCenter.Send<object, Profile>(this, MessageKeys.Settings, new Profile
            {
                Id = Id, Image = Image, Name = Name, ImageSource = ImageSource
            });
        }


        public async void SelectGallery()
        {
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
            Image = GetImageStreamAsBytes(SelectedImageFile.GetStream());
            ImageSource = ImageSource.FromStream(() => SelectedImageFile.GetStream());
        }

        public async void SelectCamera()
        {
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
            Image = GetImageStreamAsBytes(SelectedImageFile.GetStream());
            ImageSource = ImageSource.FromStream(() => SelectedImageFile.GetStream());
        }
        public void SelectStyle(object sender)
        {
            var index = ((Picker)sender).SelectedIndex;
            MessagingCenter.Send<object, int>(_page, MessageKeys.Style, index);
        }

        private byte[] GetImageStreamAsBytes(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(Name)) return;
            var profile = new Profile
            {
                Id = Id,
                Image = Image,
                Name = Name,
                ImageSource = ImageSource
            };
            MessagingCenter.Send<object, Profile>(this, MessageKeys.Settings, profile);

            _dishesService.UpdateProfile(profile);
        }
    }
}