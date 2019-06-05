using Xamarin.ViewModels;

namespace Xamarin.Views
{
    public partial class RegisterPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            BindingContext = new RegisterViewModel();
        }
    }
}