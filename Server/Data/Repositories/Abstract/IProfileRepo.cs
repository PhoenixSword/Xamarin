using System.Threading.Tasks;
using Xamarin.Models.Models;

namespace Server.Data.Repositories.Abstract
{
    public interface IProfileRepo
    {
        void UpdateProfile(Profile profile);
        Profile LoginProfile(Profile profile);
        Task<Profile> RegisterProfile(Profile profile);
    }
}
