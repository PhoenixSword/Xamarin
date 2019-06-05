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
                Loading.IsVisible = true;
                Task.Run(() => _viewModel.GetDishes());
            }
            else
            {
                _viewModel.LoadDishesCommand.Execute(null);
            }
            base.OnAppearing();

            if (!_firstStart) return;
            Loading.IsVisible = false;
            _firstStart = false;
        }
    }
}