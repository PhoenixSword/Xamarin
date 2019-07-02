using System.Collections.Generic;
using Xamarin.Models.Models;

namespace Server.Data.Repositories.Abstract
{
    public interface IRepo
    {
        IEnumerable<Dish> GetDishes(string id);
        void DeleteDishes(string id);
        void UpdateDishes(Dish dish);
        void UpdateProfile(Profile profile);
        Profile LoginProfile(Profile profile);
        Profile RegisterProfile(Profile profile);

    }
    
}
