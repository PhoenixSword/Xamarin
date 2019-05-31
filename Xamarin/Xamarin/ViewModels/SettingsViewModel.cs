using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Models.Models;
using Xamarin.Services;

namespace Xamarin.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        readonly DishesService _dishesService = new DishesService();
        public Profile Profile { get; }
        public SettingsViewModel()
        {
            Title = "Settings";
            var profile = (Profile)Application.Current.Properties["profile"];
            if (profile != null)
            {
                Profile = new Profile
                {
                    Id = profile.Id,
                    Name = profile.Name,
                    Image = profile.Image,
                    ImageSource = profile.Image != null
                        ? ImageSource.FromStream(() => new MemoryStream(profile.Image))
                        : ImageSource.FromFile("Profile.jpg")
                };
            }
            else
            {
                Profile = profile = new Profile { Name = "Unknown", ImageSource = ImageSource.FromFile("Profile.jpg") };
            }
            MessagingCenter.Send<object, Profile>(this, "Settings", Profile);
        }


        public void SelectImage(Stream stream)
        {
            Profile.Image = GetImageStreamAsBytes(stream);
        }

        private byte[] GetImageStreamAsBytes(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(Profile.Name)) return;
            //if (string.IsNullOrEmpty(Profile.Id))
            //{
            //    Db.Profiles.Add(Profile);
            //}
            //else
            //{
            //    var profile = Db.Profiles.Single();
            //    profile.Name = Profile.Name;
            //    profile.Image = Profile.Image;
            //}
            //Db.SaveChanges();

            MessagingCenter.Send<object, Profile>(this, "Settings", Profile);

            _dishesService.UpdateProfile(Profile);
        }

    }
}