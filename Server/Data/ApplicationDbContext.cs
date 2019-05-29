using Microsoft.EntityFrameworkCore;
using Xamarin.Models.Models;

namespace Server.Data
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Profile> Profiles { get; set; }

    }
}