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
        public IEnumerable<Dish> GetDishes()
        {
            return _repo.GetDishes();
        }
        [HttpDelete]
        public void DeleteDishes(string id)
        {
            _repo.DeleteDishes(id);
        }
        [HttpPost]
        public void UpdateDishes(Dish dish)
        {
            _repo.UpdateDishes(dish);
        }
        public Profile GetProfile()
        {
            return _repo.GetProfile();
        }
        [HttpPost]
        public void UpdateProfile(Profile profile)
        {
            _repo.UpdateProfile(profile);
        }
    }
}
