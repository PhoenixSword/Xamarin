using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Models;
using Xamarin.Views;

namespace Xamarin
{
    public partial class App : Application
    {
        public const string DBFILENAME = "app.db";
        public App()
        {
            InitializeComponent();

            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(DBFILENAME);
            using (var db = new ApplicationContext(dbPath))
            {
                db.Database.EnsureCreated();
                if (!db.Dishes.Any())
                {
                    db.Dishes.Add(new Dish { Name = "Dish1", Description = "Dishdescr1", Ingredients = new List<Ingredient> { new Ingredient{ Name = "name1", Count = "1", Price = "0"}}});
                    db.Dishes.Add(new Dish { Name = "Dish2", Description = "Dishdescr2", Ingredients = new List<Ingredient> { new Ingredient { Name = "name2", Count = "2", Price = "0" } }});
                    db.SaveChanges();
                }
            }

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
