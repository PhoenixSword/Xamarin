using Xamarin.Models.Models;
using Xamarin.ViewModels.Base;

namespace Xamarin.ViewModels
{
    public class DishDetailViewModel : BaseViewModel
    {
        public Dish Dish { get; private set; }

        public DishDetailViewModel(Dish dish = null)
        {
            Title = dish?.Name;
            Dish = dish;
        }
    }
}
