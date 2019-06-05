using Xamarin.ViewModels;

namespace Xamarin.Views
{
    public partial class MenuPage
    {
        public MenuPage()
        {
            InitializeComponent();
            BindingContext = new MenuPageViewModel(this);
        }
    }
}