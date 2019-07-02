using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Data.Repositories.Abstract;
using Xamarin.Models.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProfileController : Controller
    {
        private readonly IProfileRepo _profileRepo;

        public ProfileController(IProfileRepo profileRepo)
        {
            _profileRepo = profileRepo;
        }
        [HttpPost]
        public void UpdateProfile(Profile profile)
        {
            _profileRepo.UpdateProfile(profile);
        }
        [HttpPost]
        public Profile Login(Profile profile)
        {
            return _profileRepo.LoginProfile(profile);
        }
        [HttpPost]
        public Task<Profile> Register(Profile profile)
        {
            return _profileRepo.RegisterProfile(profile);
        }
    }
}
