using System.ComponentModel.DataAnnotations.Schema;
using Xamarin.Forms;

namespace Xamarin.Models.Models
{
    public class Profile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        [NotMapped]
        public ImageSource ImageSource { get; set; }
        public byte[] Image { get; set; }
    }
}
