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
        HttpClient client = new HttpClient();
        public async Task<IEnumerable<Dish>> GetDishes()
        {
            string result = await client.GetStringAsync(path + "getdishes");
            return JsonConvert.DeserializeObject<List<Dish>>(result);
        }

        public async void DeleteDishes(string id)
        {
            await client.DeleteAsync(path + "deletedishes?id=" + id);
        }

        public void UpdateDishes(Dish dish)
        {
            dish = dish.Map();
            var json = JsonConvert.SerializeObject(dish);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = client.PostAsync(path+ "updatedishes", content).Result;
            var _ = result;
        }

    }
}
