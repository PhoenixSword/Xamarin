using System.Collections.Generic;
using Xamarin.Models.Models;

namespace Server.Data.Repositories.Abstract
{
    public interface IRepo
    {
        IEnumerable<Dish> GetDishes();
        void DeleteDishes(string id);
        void UpdateDishes(Dish dish);
        Profile GetProfile();
        void UpdateProfile(Profile profile);
        Profile LoginProfile(Profile profile);
        bool RegisterProfile(Profile profile);

    }
    
}
