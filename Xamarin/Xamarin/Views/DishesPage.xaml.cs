using System.Threading.Tasks;
using Xamarin.ViewModels;

namespace Xamarin.Views
{
    public partial class DishesPage
    {
        private bool _firstStart = true;
        private readonly DishesViewModel _viewModel;
        public DishesPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new DishesViewModel(this);
        }

        protected override void OnAppearing()
        {
            if (_firstStart)
            {
                Task.Run(() => _viewModel.GetDishes(true)).ContinueWith(res =>
                {
                     //Loading.IsVisible = false;
                    _firstStart = false;
                    return true;
                });
            }
            else
            {
                _viewModel.LoadDishesCommand.Execute(null);
            }
            base.OnAppearing();
        }
    }
}