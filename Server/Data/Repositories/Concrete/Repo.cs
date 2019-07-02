using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Data.Repositories.Abstract;
using Xamarin.Models.Models;

namespace Server.Data.Repositories.Concrete
{
    public class Repo : IRepo
    {
        private readonly ApplicationDbContext _ctx;
        public readonly HttpClient Client;
        const string key = "AAAAT2Hpqc0:APA91bEsDTjQEHH8FhzBDEnsBRvTAt3Di6Shc1vjg7W3DCOgu6u61xn_IUq9UltNfdQfan3-HYTcvOc9bHYPKoKfuJTIgEENcZwL6GajL0Fgjq6Mug248ewInqpwcjQDb4rH_WnMpV_g";

        private IEnumerable<Dish> Dishes => _ctx.Dishes.Include(d => d.Ingredients).ToList();
        //private IEnumerable<Profile> Profiles => _ctx.Profiles.ToList();

        public Repo(ApplicationDbContext applicationDbContext)
        {
            _ctx = applicationDbContext;
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"key={key}");
        }

        public IEnumerable<Dish> GetDishes(string id)
        {
            var list = Dishes.Where(d=>d.ProfileId == id).ToList();
            return list;
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

            if (oldDish != null) return;
            JObject obj;
            obj = JObject.Parse("{ \"to\": \"/topics/notification\",\"notification\" : { \"body\" : \"New dish: " + dish.Name + "\"} }");

            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var _ = Client.PostAsync("https://fcm.googleapis.com/fcm/send", content).Result;
         }

    }
}
