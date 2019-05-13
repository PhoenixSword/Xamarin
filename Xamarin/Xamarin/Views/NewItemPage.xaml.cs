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
        public Item Item { get; set; }
        public string Name { get; set; }

        public NewItemPage()
        {
            Name = "Add Item";
            InitializeComponent();

            Item = new Item
            {
                Text = "Item name",
                Description = "This is an item description."
            };

            BindingContext = this;
        }

        [Obsolete]
        public NewItemPage(ItemDetailViewModel itemDetail)
        {
            var deleteButton = new ToolbarItem {Text = "Delete", Icon = "DeleteIcon.png"};
            Name = "Edit Item";
            deleteButton.Clicked += Delete_Clicked;
            InitializeComponent();
            Item = new Item
            {
                Id = itemDetail.Item.Id,
                Text = itemDetail.Item.Text,
                Description = itemDetail.Item.Description
            };

            BindingContext = this;

            ToolbarItems.Add(deleteButton);
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, string.IsNullOrEmpty(Item.Id) ? "AddItem" : "UpdateItem", Item);
            await Navigation.PopAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void Delete_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "DeleteItem", Item);
            await Navigation.PopAsync();
        }
    }
}