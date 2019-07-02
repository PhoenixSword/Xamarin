using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Xamarin.Forms;

namespace Xamarin.Models.Models
{
    public class Profile : IdentityUser
    {
        public string Name { get; set; }
        [NotMapped]
        public string Password { get; set; }
        [NotMapped]
        public ImageSource ImageSource { get; set; }
        public byte[] Image { get; set; }
    }
}
