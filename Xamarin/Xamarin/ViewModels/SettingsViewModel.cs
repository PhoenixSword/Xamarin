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

            if (Db.Profiles.FirstOrDefault() == null)
            {
                Profile profile = null;
                Task.Run(async() => await _dishesService.GetProfile()).ContinueWith(task => profile = task.Result);
                if (profile != null)
                {
                    Db.Profiles.Add(profile);
                    Db.SaveChanges();
                }
            }
            
            Profile = Db.Profiles.Select(p => new Profile
            {
                Id = p.Id,
                Name = p.Name,
                Image = p.Image,
                ImageSource = p.Image != null ? ImageSource.FromStream(() => new MemoryStream(p.Image)) : ImageSource.FromFile("Profile.jpg")
            }).FirstOrDefault() ?? new Profile {Name = "Unknown", ImageSource = ImageSource.FromFile("Profile.jpg") };

            MessagingCenter.Send<object, Profile>(this, "Settings", Profile);
        }

        public void SelectImage(Stream stream)
        {
            Profile.Image = GetImageStreamAsBytes(stream);
        }

        public byte[] GetImageStreamAsBytes(Stream input)
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
            if (string.IsNullOrEmpty(Profile.Id))
            {
                Db.Profiles.Add(Profile);
            }
            else
            {
                var profile = Db.Profiles.Single();
                profile.Name = Profile.Name;
                profile.Image = Profile.Image;
            }
            Db.SaveChanges();

            MessagingCenter.Send<object, Profile>(this, "Settings", Profile);

            _dishesService.UpdateProfile(Profile);
        }

    }
}