using System;
using Xamarin.ViewModels;

namespace Xamarin.Views
{
    public partial class NewDishPage
    {
        private readonly NewDishViewModel _viewModel;
        public NewDishPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new NewDishViewModel(this);
        }

        public NewDishPage(DishDetailViewModel dishDetail)
        {
            InitializeComponent();
            BindingContext = _viewModel = new NewDishViewModel(this, dishDetail);
        }

        private void DeleteIngredient(object sender, EventArgs eventArgs)
        {
            _viewModel.DeleteIngredientCommand.Execute(sender);
        }
    }
}