using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Server.Data.Repositories.Abstract;
using Xamarin.Models.Models;
using Xamarin.Models.Models.Mappings;

namespace Server.Data.Repositories.Concrete
{
    public class Repo : IRepo
    {
        private readonly ApplicationDbContext _ctx;
        private IEnumerable<Dish> Dishes => _ctx.Dishes.Include(d => d.Ingredients).ToList();
        //private IEnumerable<Profile> Profiles => _ctx.Profiles.ToList();
        public Repo(ApplicationDbContext applicationDbContext)
        {
            _ctx = applicationDbContext;
        }

        public IEnumerable<Dish> GetDishes()
        {
            var list = Dishes.Select(d => d.Map()).ToList();
            return list;
        }

        public void UpdateProfile(Profile profile)
        {
            var oldProfile = _ctx.Profiles.FirstOrDefault(p => p.Id == profile.Id);
            if (oldProfile == null) return;
            oldProfile.Name = profile.Name;
            oldProfile.Image = profile.Image;
            _ctx.SaveChanges();
        }

        public Profile LoginProfile(Profile profile)
        {
            return _ctx.Profiles.Any(p => p.Email == profile.Email && p.Password == HashPassword(profile.Password)) ? _ctx.Profiles.FirstOrDefault(p => p.Email == profile.Email) : null;
        }

        public Profile RegisterProfile(Profile profile)
        {
            if (_ctx.Profiles.Any(p => p.Email == profile.Email)) return null;

            profile.Id = Guid.NewGuid().ToString();

            profile.Password = HashPassword(profile.Password);

            _ctx.Add(profile);
            _ctx.SaveChanges();

            return profile;
        }

        public void DeleteDishes(string id)
        {
            var dish = Dishes.FirstOrDefault(d=>d.Id == id);
            if (dish != null)
            {
                _ctx.Dishes.Remove(dish);
                _ctx.Ingredients.RemoveRange(dish.Ingredients);
            }

            _ctx.SaveChanges();
        }

        public void UpdateDishes(Dish dish)
         {
            _ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var oldDish = _ctx.Dishes.FirstOrDefault(d => d.Id == dish.Id);
            if (oldDish != null)
            {
                _ctx.Remove(oldDish);
                _ctx.SaveChanges();
            }
            _ctx.Add(dish);
            _ctx.SaveChanges();

        }

        private static string HashPassword(string password)
        {
            var data = Encoding.ASCII.GetBytes(password);
            var md5 = new MD5CryptoServiceProvider();
            var md5data = md5.ComputeHash(data);
            return BitConverter.ToString(md5data).Replace("-", "");
        }
    }
}
