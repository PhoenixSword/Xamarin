using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Android.Content.Res;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;
using Xamarin.Models;
using Xamarin.Services;

namespace Xamarin.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        readonly DishesService dishesService = new DishesService();
        public Profile Profile { get; set; }
        public string Name { get; set; }

        public SettingsViewModel()
        {
            Title = "Settings";

            if (db.Profiles.FirstOrDefault() == null)
            {
                var profile = dishesService.GetProfile();
                if (profile != null)
                {
                    db.Profiles.Add(profile);
                    db.SaveChanges();
                }
            }
            
            Profile = db.Profiles.Select(p => new Profile
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
                db.Profiles.Add(Profile);
            else
            {
                db.Update(Profile);
            }
            db.SaveChanges();

            MessagingCenter.Send<object, Profile>(this, "Settings", Profile);

            dishesService.UpdateProfile(Profile);
        }

    }
}