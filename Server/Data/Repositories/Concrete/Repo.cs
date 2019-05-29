﻿using System.Collections.Generic;
using System.Linq;
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
        private Profile Profile => _ctx.Profiles.FirstOrDefault();

        public Repo(ApplicationDbContext applicationDbContext)
        {
            _ctx = applicationDbContext;
        }

        public IEnumerable<Dish> GetDishes()
        {
            var list = Dishes.Select(d => d.Map()).ToList();
            return list;
        }

        public Profile GetProfile()
        {
            return Profile;
        }
        public void UpdateProfile(Profile profile)
        {
            _ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            if (Profile != null)
            {
                _ctx.Remove(Profile);
                _ctx.SaveChanges();
            }
            _ctx.Add(profile);
            _ctx.SaveChanges();
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

    }
}
