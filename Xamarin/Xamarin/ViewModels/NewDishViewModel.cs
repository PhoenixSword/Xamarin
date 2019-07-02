using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
    public class NewDishViewModel : BaseViewModel
    {
        private readonly DishesService _dishesService = new DishesService();
        private MediaFile SelectedImageFile { get; set; }
        private readonly NewDishPage _page;
        private readonly Dish _currentDish;

        private string _id;
        public string Id
        {
            get => _id;
            set
            {
                if (_id == value || value == null)
                    return;
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
                if (_name == value || value == null)
                    return;
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description == value || value == null)
                    return;
                _description = value;
                OnPropertyChanged();
            }
        }

        private byte[] _image;
        public byte[] Image
        {
            get => _image;
            set
            {
                if (_image == value || value == null)
                    return;
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
                if (_imageSource == value || value == null)
                    return;
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Ingredient> _ingredients;
        public ObservableCollection<Ingredient> Ingredients
        {
            get => _ingredients;
            set
            {
                if (_ingredients == value)
                    return;
                _ingredients = value;
                OnPropertyChanged();
            }
        }

        private int _listHeight;
        public int ListHeight
        {
            get => _listHeight;
            set
            {
                if (_listHeight == value)
                    return;
                _listHeight = value;
                OnPropertyChanged();
            }
        }
        public ICommand SaveCommand => new Command(Save);
        public ICommand AddCommand => new Command(Add);
        public ICommand DeleteCommand => new Command(Delete);
        public ICommand DeleteIngredientCommand => new Command(DeleteIngredient);
        public ICommand ImageCommand => new Command(async() => await ImageAsync());

        public NewDishViewModel(NewDishPage page)
        {
            Title = "Add Dish";
            Name = "Dish name";
            Description = "This is an dish description.";
            Image = null;
            ImageSource = ImageSource.FromFile("EmptyImage.jpg");
            Ingredients = new ObservableCollection<Ingredient>();
            _page = page;
            ListHeight = 0;
        }

        public NewDishViewModel(NewDishPage page, DishDetailViewModel dishDetail)
        {
            Title = "Edit Dish";

            Id = dishDetail.Dish.Id;
            Name = dishDetail.Dish.Name;
            Description = dishDetail.Dish.Description;
            Image = dishDetail.Dish.Image;
            ImageSource = dishDetail.Dish.ImageSource;
            Ingredients = new ObservableCollection<Ingredient>();
            foreach (Ingredient item in dishDetail.Dish.Ingredients)
            {
                Ingredients.Add(item);
            }

            _currentDish = new Dish
            {
                Id = dishDetail.Dish.Id,
                Sum = dishDetail.Dish.Sum,
                Name = dishDetail.Dish.Name,
                Description = dishDetail.Dish.Description,
                Image = dishDetail.Dish.Image,
                ImageSource = dishDetail.Dish.ImageSource,
                Ingredients = dishDetail.Dish.Ingredients
            };
         
            _page = page;
            ListHeight = Ingredients.Count * 60;
            var deleteButton = new ToolbarItem { Text = "Delete", IconImageSource = "DeleteIcon.png" };
            deleteButton.Command = DeleteCommand;
            _page.ToolbarItems.Add(deleteButton);
        }

        public void SelectImage(Stream stream)
        {
            Image = GetImageStreamAsBytes(stream);
        }

        public byte[] GetImageStreamAsBytes(Stream input)
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
            double sum = 0;
            foreach (var item in Ingredients)
            {
                sum += item.Price;
            }

            var dish = new Dish
            {
                Id = Id,
                Description = Description,
                Ingredients = Ingredients.ToList(),
                Name = Name,
                Image = Image,
                ImageSource = ImageSource,
                Sum = sum,
                ProfileId = Application.Current.Properties["id"].ToString()
            };
            if (string.IsNullOrEmpty(Name)) return;
            if (string.IsNullOrEmpty(Id))
                Db.Dishes.Add(dish);
            else
            {
                var listIngredients = Db.Ingredients.Where(t => t.DishId == Id).Except(Ingredients);

                foreach (var item in listIngredients)
                {
                    Db.Ingredients.Remove(item);
                }
                Db.Update(dish);
            }
            Db.SaveChanges();
            _dishesService.UpdateDishes(dish);
            _page.Navigation.PopAsync();
        }

        public void Delete()
        {
            Db.Dishes.Remove(_currentDish);
            Db.SaveChanges();
            _dishesService.DeleteDishes(Id);
            _page.Navigation.PopAsync();
        }

        public void Add()
        {
            Ingredients.Add(new Ingredient { Name = "", Count = "0", Price = 0 });
            ListHeight += 60;
            //await ScrollView.ScrollToAsync(StackLayout, ScrollToPosition.End, true);
        }

        public void DeleteIngredient(object sender)
        {
            var button = sender as Image;
            var ingredient = button?.BindingContext as Ingredient;

            ListHeight -= 60;
            Ingredients.Remove(ingredient);
        }

        private async Task ImageAsync()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await _page.DisplayAlert("Not supported", "Not supported", "Ok");
                return;
            }

            var mediaOptions = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Medium
            };

            SelectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

            if (SelectedImageFile == null) return;
            SelectImage(SelectedImageFile.GetStream());

            ImageSource = ImageSource.FromStream(() => SelectedImageFile.GetStream());
        }

    }
}