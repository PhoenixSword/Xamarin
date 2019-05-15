using System;
using System.Collections.Generic;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Xamarin.Models;
using Xamarin.ViewModels;
using static Xamarin.Forms.Device;

namespace Xamarin.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class NewItemPage : ContentPage
    {
        string dbPath;
        public Item Item { get; set; }
        public string Name { get; set; }

        public NewItemPage()
        {
            dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            Name = "Add Item";
            InitializeComponent();

            Item = new Item
            {
                Text = "Item name",
                Description = "This is an item description.",
                Number = "0"
            };

            BindingContext = this;
        }

        [Obsolete]
        public NewItemPage(ItemDetailViewModel itemDetail)
        {
            dbPath = DependencyService.Get<IPath>().GetDatabasePath(App.DBFILENAME);
            var deleteButton = new ToolbarItem {Text = "Delete", Icon = "DeleteIcon.png"};
            Name = "Edit Item";
            deleteButton.Clicked += Delete_Clicked;
            InitializeComponent();
            Item = new Item
            {
                Id = itemDetail.Item.Id,
                Text = itemDetail.Item.Text,
                Description = itemDetail.Item.Description,
                Number = itemDetail.Item.Number
            };

            BindingContext = this;

            ToolbarItems.Add(deleteButton);
        }

        //async void Save_Clicked(object sender, EventArgs e)
        //{
        //    MessagingCenter.Send(this, string.IsNullOrEmpty(Item.Id) ? "AddItem" : "UpdateItem", Item);
        //    await Navigation.PopAsync();
        //}

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        //async void Delete_Clicked(object sender, EventArgs e)
        //{
        //    MessagingCenter.Send(this, "DeleteItem", Item);
        //    await Navigation.PopAsync();
        //}

        private void Save_Clicked(object sender, EventArgs e)
        {
            var item = Item;
            if (!String.IsNullOrEmpty(item.Text))
            {
                using (ApplicationContext db = new ApplicationContext(dbPath))
                {
                    if (string.IsNullOrEmpty(item.Id))
                        db.Items.Add(item);
                    else
                    {
                        db.Items.Update(item);
                    }
                    db.SaveChanges();
                }
            }
            this.Navigation.PopAsync();
        }
        private void Delete_Clicked(object sender, EventArgs e)
        {
            var item = Item;
            using (ApplicationContext db = new ApplicationContext(dbPath))
            {
                db.Items.Remove(item);
                db.SaveChanges();
            }
            this.Navigation.PopAsync();
        }
    }
}