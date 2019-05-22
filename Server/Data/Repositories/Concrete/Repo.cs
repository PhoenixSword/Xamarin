using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Data;
using Server.Data.Repositories.Abstract;
using Server.Models;
using Server.Models.Mappings;

namespace Server.Data.Repositories.Concrete
{
    public class Repo : IRepo
    {
        private readonly ApplicationDbContext _ctx;
        private IEnumerable<Dish> Dishes => _ctx.Dishes.Include(d => d.Ingredients).ToList();

        public Repo(ApplicationDbContext applicationDbContext)
        {
            _ctx = applicationDbContext;
        }

        public IEnumerable<Dish> GetDishes()
        {
            var list = Dishes.Select(d => d.Map()).ToList();
            return list;
        }

        public void DeleteDishes(string id)
        {
            var dish = Dishes.FirstOrDefault(d=>d.Id == id);
            _ctx.Dishes.Remove(dish);
            _ctx.Ingredients.RemoveRange(dish.Ingredients);
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
