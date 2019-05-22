﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Android.Database;
using Newtonsoft.Json;
using Xamarin.Forms;

using Xamarin.Models;
using Xamarin.Services;
using Xamarin.Views;

namespace Xamarin.ViewModels
{
    public class DishesViewModel : BaseViewModel
    {
        string dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
        DishesService dishesService = new DishesService();
        public ObservableCollection<Dish> Dishes { get; set; }
        public Command LoadDishesCommand { get; set; }
        public DishesViewModel()
        {
            Title = "My Dishes";
            Dishes = new ObservableCollection<Dish>();
            LoadDishesCommand = new Command(async () => await ExecuteLoadDishesCommand());
        }


        public async Task ExecuteLoadDishesCommand()
        {
             if (IsBusy)
                return;

             IsBusy = true;
             await GetDishes();
             using (ApplicationContext db = new ApplicationContext(dbPath))
             {
                 var list = db.Dishes.Select(i => new Dish
                 {
                     Id = i.Id,
                     Name = i.Name,
                     Description = i.Description,
                     Image = i.Image,
                     Sum = i.Sum,
                     ImageSource = i.Image != null ? ImageSource.FromStream(() => new MemoryStream(i.Image)) : ImageSource.FromFile("EmptyImage.jpg"),
                     Ingredients = i.Ingredients
                 }).ToList();
                 Dishes.Clear();
                 foreach (var dish in list)
                     Dishes.Add(dish);
             }
             IsBusy = false;

        }

        public async Task GetDishes()
        {

            IEnumerable<Dish> dishes = await dishesService.GetDishes();

            using (var db = new ApplicationContext(dbPath))
            {
                db.Database.EnsureCreated();
                var list1 = dishes.ToList();
                var list2 = db.Dishes.ToList();
                var listId = list1.Select(d => d.Id).Except(list2.Select(d => d.Id)).ToList();
                foreach (var id in listId)
                {
                    db.Dishes.Add(dishes.FirstOrDefault(d => d.Id == id));
                }

                db.SaveChanges();
            }
            LoadDishesCommand.Execute(null);
        }

    }
}