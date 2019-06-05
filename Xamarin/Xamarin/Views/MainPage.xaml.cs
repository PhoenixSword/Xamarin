using Xamarin.ViewModels;

namespace Xamarin.Views
{
    public partial class MainPage
    {
        public readonly MainViewModel ViewModel;
        public MainPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = new MainViewModel(this);
        }
    }
}