using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Models.Models;
using Xamarin.Models.Models.Mappings;

using Xamarin.Forms;
namespace Xamarin.Services
{
    public class DishesService   
    {
        string path = "http://192.168.0.141:8080/api/dish/";

        private HttpClient _client = new HttpClient{DefaultRequestHeaders = {{"token", Application.Current.Properties["token"].ToString()}}};
        
        public async Task<IEnumerable<Dish>> GetDishes()
        {
            try
            {
                _client.Timeout = TimeSpan.FromMilliseconds(4000);
                var result = await _client.GetStringAsync(path + "getdishes");
                return JsonConvert.DeserializeObject<List<Dish>>(result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async void DeleteDishes(string id)
        {
            try
            {
                _client.Timeout = TimeSpan.FromMilliseconds(3000);
                await _client.DeleteAsync(path + "deletedishes?id=" + id);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public async void UpdateDishes(Dish dish)
        {
            try
            {
                _client.Timeout = TimeSpan.FromMilliseconds(3000);
                dish = dish.Map();
                var json = JsonConvert.SerializeObject(dish);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await _client.PostAsync(path + "updatedishes", content);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public async Task<Profile> GetProfile()
        {
            try
            {
                _client.Timeout = TimeSpan.FromMilliseconds(4000);
                var result = await _client.GetStringAsync(path + "getprofile");
                return JsonConvert.DeserializeObject<Profile>(result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async void UpdateProfile(Profile profile)
        {
            try
            {
                _client.Timeout = TimeSpan.FromMilliseconds(3000);
                profile = profile.Map();
                var json = JsonConvert.SerializeObject(profile);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await _client.PostAsync(path + "updateprofile", content);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public bool Login(string email, string password)
        {
            try
            {
                _client.Timeout = TimeSpan.FromMilliseconds(5000);
                var json = JsonConvert.SerializeObject(new { email, password });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return Convert.ToBoolean(_client.PostAsync(path + "login", content).Result.Content.ToString());
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Register(string email, string password)
        {
            try
            {
                _client.Timeout = TimeSpan.FromMilliseconds(5000);
                var json = JsonConvert.SerializeObject(new { email, password });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return Convert.ToBoolean(_client.PostAsync(path + "login", content).Result.Content.ToString());
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
