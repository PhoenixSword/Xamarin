using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.Data.Repositories.Abstract;
using Xamarin.Models.Models;

namespace Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DishController : Controller
    {
        private readonly IRepo _repo;

        public DishController(IRepo repo)
        {
            _repo = repo;
        }
        [HttpPost]
        public IEnumerable<Dish> GetDishes(string id)
        {
            return _repo.GetDishes(id);
        }
        [HttpPost]
        public void UpdateDishes(Dish dish)
        {
            _repo.UpdateDishes(dish);
        }
        [HttpDelete]
        public void DeleteDishes(string id)
        {
            _repo.DeleteDishes(id);
        }
    }
}
