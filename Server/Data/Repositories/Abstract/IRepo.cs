using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Models;

namespace Server.Data.Repositories.Abstract
{
    public interface IRepo
    {
        IEnumerable<Dish> GetDishes();
        void DeleteDishes(string id);
        void UpdateDishes(Dish dish);

    }
    
}
