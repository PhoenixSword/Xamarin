using Xamarin.Forms;
using Xamarin.ViewModels;

namespace Xamarin.Views
{
    public partial class MenuPage
    {
        public MenuPage()
        {
            InitializeComponent();
            BindingContext = new MenuPageViewModel(this);
            ItemsList.ItemTemplate = new DataTemplate(typeof(CustomCell));
        }
    }
}