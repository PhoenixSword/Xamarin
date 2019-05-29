using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Models;
using Xamarin.Models.Mappings;

namespace Xamarin.Services
{
    class DishesService
    {
        string path = "http://192.168.0.141:8080/api/dish/";

        readonly HttpClient client = new HttpClient();

        public async Task<IEnumerable<Dish>> GetDishes()
        {
            try
            {
                client.Timeout = TimeSpan.FromMilliseconds(4000);
                var result = await client.GetStringAsync(path + "getdishes");
                return JsonConvert.DeserializeObject<List<Dish>>(result);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async void DeleteDishes(string id)
        {
            try
            {
                client.Timeout = TimeSpan.FromMilliseconds(3000);
                var result = await client.DeleteAsync(path + "deletedishes?id=" + id);
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        public async void UpdateDishes(Dish dish)
        {
            try
            {
                client.Timeout = TimeSpan.FromMilliseconds(3000);
                dish = dish.Map();
                var json = JsonConvert.SerializeObject(dish);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await client.PostAsync(path + "updatedishes", content);
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        public Profile GetProfile()
        {
            try
            {
                client.Timeout = TimeSpan.FromMilliseconds(3000);
                var result = client.GetStringAsync(path + "getprofile").Result;
                return JsonConvert.DeserializeObject<Profile>(result);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async void UpdateProfile(Profile profile)
        {
            try
            {
                client.Timeout = TimeSpan.FromMilliseconds(3000);
                profile = profile.Map();
                var json = JsonConvert.SerializeObject(profile);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await client.PostAsync(path + "updateprofile", content);
            }
            catch (Exception e)
            {
                // ignored
            }
        }
    }
}
