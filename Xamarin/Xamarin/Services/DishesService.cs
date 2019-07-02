using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Xamarin.Forms;
using Xamarin.Models.Models;

namespace Xamarin.Services
{
    public class DishesService   
    {
        string path = "http://192.168.0.141:8080/api/";

        private HttpClient _client = new HttpClient();
        public async Task<IEnumerable<Dish>> GetDishes()
        {
            try
            {
                _client.Timeout = TimeSpan.FromMilliseconds(1000);
                var httpResponseMessage = _client.PostAsync(path + "dish/getdishes?id=" + Application.Current.Properties["id"], null).Result;
                var resp = await httpResponseMessage.Content.ReadAsStringAsync();
                return httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK ? JsonConvert.DeserializeObject<List<Dish>>(resp) : null;
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
                await _client.DeleteAsync(path + "dish/deletedishes?id=" + id);
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
                dish.ImageSource = null;
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                settings.Converters.Add(new StringEnumConverter());
                var json = JsonConvert.SerializeObject(dish, settings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await _client.PostAsync(path + "dish/updatedishes", content);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public async void UpdateProfile(Profile profile)
        {
            try
            {
                _client.Timeout = TimeSpan.FromMilliseconds(3000);
                profile.ImageSource = null;
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                settings.Converters.Add(new StringEnumConverter());
                var json = JsonConvert.SerializeObject(profile, settings);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await _client.PostAsync(path + "profile/updateprofile", content);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public Profile Login(string email, string password)
        {
            try
            {
                _client.Timeout = TimeSpan.FromMilliseconds(5000);
                var json = JsonConvert.SerializeObject(new { email, password });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var httpResponseMessage = _client.PostAsync(path + "profile/login", content).Result;
                var resp = httpResponseMessage.Content.ReadAsStringAsync().Result;
                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    return JsonConvert.DeserializeObject<Profile>(resp);
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Profile Register(string email, string password)
        {
            try
            {
                _client.Timeout = TimeSpan.FromMilliseconds(5000);
                var json = JsonConvert.SerializeObject(new { email, password });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var httpResponseMessage = _client.PostAsync(path + "profile/register", content).Result;
                var resp = httpResponseMessage.Content.ReadAsStringAsync().Result;
                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    return JsonConvert.DeserializeObject<Profile>(resp);
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
