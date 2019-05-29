using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Models.Models;
using Xamarin.Services;

namespace Xamarin.ViewModels
{
    public class DishesViewModel : BaseViewModel
    {
        private readonly DishesService _dishesService = new DishesService();
        public ObservableCollection<Dish> Dishes { get; }
        public Command LoadDishesCommand { get; }
        public DishesViewModel()
        {
            Title = "My Dishes";
            Dishes = new ObservableCollection<Dish>();
            LoadDishesCommand = new Command( ExecuteLoadDishesCommand);
        }


        private void ExecuteLoadDishesCommand()
        {
             if (IsBusy)
                return;

             IsBusy = true;
             var list = Db.Dishes.Select(i => new Dish
             {
                 Id = i.Id,
                 Name = i.Name,
                 Description = i.Description,
                 Image = i.Image,
                 Sum = i.Sum,
                 ImageSource = i.Image != null ? ImageSource.FromStream(() => new MemoryStream(i.Image)) : ImageSource.FromFile("EmptyImage.jpg"),
                 Ingredients = i.Ingredients
             }).ToList();

             Dishes.Clear();
             foreach (var dish in list)
                 Dishes.Add(dish);
             IsBusy = false;

        }

        public async Task<HttpResponseMessage> GetDishes()
        {

            var dishes = await _dishesService.GetDishes();

            if (dishes == null)
            {
                LoadDishesCommand.Execute(null);
                return null;
            }
            Db.Database.EnsureCreated();
            var enumerable = dishes as Dish[] ?? dishes.ToArray();
            var list1 = enumerable.ToList();
            var list2 = Db.Dishes.ToList();
            var listId = list1.Select(d => d.Id).Except(list2.Select(d => d.Id)).ToList();
            foreach (var id in listId)
            {
                var item = enumerable.FirstOrDefault(d => d.Id == id);
                if (item != null) Db.Dishes.Add(item);
            }

            Db.SaveChanges();
            LoadDishesCommand.Execute(null);
            return new HttpResponseMessage(new HttpStatusCode());
        }

    }
}