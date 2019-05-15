using Microsoft.EntityFrameworkCore;
using Xamarin.Models;

namespace Xamarin
{
    public class ApplicationContext : DbContext
    {
        private string _databasePath;

        public DbSet<Item> Items { get; set; }

        public ApplicationContext(string databasePath)
        {
            _databasePath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_databasePath}");
        }
    }
}