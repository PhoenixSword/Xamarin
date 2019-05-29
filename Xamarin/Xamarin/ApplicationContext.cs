using Microsoft.EntityFrameworkCore;
using Xamarin.Models.Models;

namespace Xamarin
{
    public sealed class ApplicationContext : DbContext
    {
        private string _databasePath;

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        public ApplicationContext(string databasePath)
        {
            _databasePath = databasePath;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_databasePath}");
        }
    }
}