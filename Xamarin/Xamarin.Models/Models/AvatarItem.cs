
namespace Xamarin.Models.Models
{
    public enum AvatarType
    {
        Gallery,
        Camera
    }
    public class AvatarItem
    {
        public AvatarType Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
    }
}
