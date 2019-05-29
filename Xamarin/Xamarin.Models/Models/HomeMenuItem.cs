
namespace Xamarin.Models.Models
{
    public enum MenuItemType
    {
        MyDishes,
        Settings,
        Exit
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
