using Xamarin.Models.Models;

namespace Xamarin.ViewModels
{
    public class DishDetailViewModel : BaseViewModel
    {
        public Dish Dish { get; set; }
        public DishDetailViewModel(Dish dish = null)
        {
            Title = dish?.Name;
            Dish = dish;
        }
    }
}
