﻿using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Services;

namespace Xamarin.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class LoginPage
    {
        private DishesService dishesService = new DishesService();
        public LoginPage()
        {
            InitializeComponent();
           
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            var email = Email.Text;
            var password = Password.Text;
            if (email == null || password == null)
            {
                DependencyService.Get<IMessage>().LongAlert("Enter login and password");
                return;
            }

            if (dishesService.Login(email, password))
            {
                Application.Current.Properties["token"] = "test";
                Application.Current.MainPage = new MainPage();
                return;
            }
            DependencyService.Get<IMessage>().LongAlert("Wrong credentials found!");

        }

        private void Register_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new RegisterPage();;
        }
    }
}