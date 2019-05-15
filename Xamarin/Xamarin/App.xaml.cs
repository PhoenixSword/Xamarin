using System;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Models;
using Xamarin.Views;

namespace Xamarin
{
    public partial class App : Application
    {
        public const string DBFILENAME = "itemsapp.db";
        public App()
        {
            InitializeComponent();

            string dbPath = DependencyService.Get<IPath>().GetDatabasePath(DBFILENAME);
            using (var db = new ApplicationContext(dbPath))
            {
                db.Database.EnsureCreated();
                if (!db.Items.Any())
                {
                    db.Items.Add(new Item { Text = "item1", Description = "descr1", Number = "1"});
                    db.Items.Add(new Item { Text = "item2", Description = "descr2", Number = "2" });
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
