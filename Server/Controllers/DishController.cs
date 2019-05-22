using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server.Data.Repositories.Abstract;
using Server.Models;

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
        public Dish UpdateDishes(Dish dish)
        {
            _repo.UpdateDishes(dish);
            return dish;
        }
    }
}
