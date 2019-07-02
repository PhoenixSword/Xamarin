using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Server.Data.Repositories.Abstract;
using Xamarin.Models.Models;

namespace Server.Data.Repositories.Concrete
{
    public class ProfileRepo : IProfileRepo
    {
        private readonly ApplicationDbContext _ctx;
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;

        public ProfileRepo(ApplicationDbContext applicationDbContext, UserManager<Profile> userManager, SignInManager<Profile> signInManager)
        {
            _ctx = applicationDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public void UpdateProfile(Profile profile)
        {
            var oldProfile = _ctx.Profiles.FirstOrDefault(p => p.Id == profile.Id);
            if (oldProfile == null) return;
            oldProfile.Name = profile.Name;
            oldProfile.Image = profile.Image;
            _ctx.SaveChanges();
        }

        public Profile LoginProfile(Profile profile)
        {
            var user = _ctx.Profiles.FirstOrDefault(p => p.Email == profile.Email);
            if (user == null) return null;
            {
                if (_signInManager.CheckPasswordSignInAsync(user, profile.Password, false).Result.Succeeded)
                {
                    return user;
                }
            }
            return null;
        }

        public async Task<Profile> RegisterProfile(Profile profile)
        {
            if (_ctx.Profiles.Any(p => p.Email == profile.Email)) return null;
            profile.Name = profile.Email;
            profile.UserName = profile.Email;
            var result = await _userManager.CreateAsync(profile, profile.Password);
            return profile;
        }
    }
}
