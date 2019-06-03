﻿using Microsoft.AspNetCore.Mvc;
using Server.Data.Repositories.Abstract;
using Xamarin.Models.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProfileController : Controller
    {
        private readonly IRepo _repo;

        public ProfileController(IRepo repo)
        {
            _repo = repo;
        }
        [HttpPost]
        public void UpdateProfile(Profile profile)
        {
            _repo.UpdateProfile(profile);
        }
        [HttpPost]
        public Profile Login(Profile profile)
        {
            return _repo.LoginProfile(profile);
        }
        [HttpPost]
        public Profile Register(Profile profile)
        {
            return _repo.RegisterProfile(profile);
        }
    }
}
