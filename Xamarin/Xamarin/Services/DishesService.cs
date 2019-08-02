using System;
using System.Collections.Generic;
using System.Net.Http;
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

        public DishesService()
        {
            _client.BaseAddress = new Uri(path);
        }
        public async Task<IEnumerable<Dish>> GetDishes()
        {
            try
            {
                _client.Timeout = TimeSpan.FromMilliseconds(1000);
                var httpResponseMessage = _client.PostAsync(path + "dish/GetDishes?id=" + Application.Current.Properties["id"], null).Result;
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
                await _client.DeleteAsync(path + "dish/DeleteDishes?id=" + id);
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
                var request = new HttpRequestMessage(HttpMethod.Post, "dish/updatedishes")
                {
                    Content = new StringContent(json,
                        Encoding.UTF8,
                        "application/json")
                };
                await _client.SendAsync(request);
            }
            catch (Exception)
            {
                // ignore
            }
        }

    }
}
