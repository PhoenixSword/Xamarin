
namespace Xamarin.Models.Models
{
    public enum MenuItemType
    {
        MyDishes,
        Settings,
        LogOut
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
