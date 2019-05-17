using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

using Xamarin.Models;
using Xamarin.Views;

namespace Xamarin.ViewModels
{
    public class DishesViewModel : BaseViewModel
    {
        public ObservableCollection<Dish> Dishes { get; set; }
        public Command LoadDishesCommand { get; set; }
        public DishesViewModel()
        {
            Title = "My Dishes";
            Dishes = new ObservableCollection<Dish>();
            LoadDishesCommand = new Command(async () => await ExecuteLoadDishesCommand());
        }


        public async Task ExecuteLoadDishesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);

            using (ApplicationContext db = new ApplicationContext(dbPath))
            {
                var list = db.Dishes.Select(i => new Dish
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    Image = i.Image,
                    ImageSource = i.Image != null ? ImageSource.FromStream(() => new MemoryStream(i.Image)) : ImageSource.FromFile("EmptyImage.jpg")
                }).ToList();
                foreach (var dish in list)
                    Dishes.Add(dish);
            }

            IsBusy = false;
        }
    }
}