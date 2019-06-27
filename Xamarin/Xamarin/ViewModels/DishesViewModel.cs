using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Models.Models;
using Xamarin.Services;
using Xamarin.ViewModels.Base;
using Xamarin.Views;

namespace Xamarin.ViewModels
{
    public class DishesViewModel : BaseViewModel
    {
        private Page _page;
        private readonly DishesService _dishesService = new DishesService();

        private bool _loaderIsVisible;
        public bool LoaderIsVisible
        {
            get => _loaderIsVisible;
            set
            {
                if (_loaderIsVisible == value)
                    return;
                _loaderIsVisible = value;
                OnPropertyChanged();
            }
        }

        private string _connectionLabel;
        public string ConnectionLabel
        {
            get => _connectionLabel;
            set
            {
                if (_connectionLabel == value || value == null)
                    return;
                _connectionLabel = value;
                OnPropertyChanged();
            }
        }

        private int _connectionLabelHeight;
        public int ConnectionLabelHeight
        {
            get => _connectionLabelHeight;
            set
            {
                if (_connectionLabelHeight == value)
                    return;
                _connectionLabelHeight = value;
                OnPropertyChanged();
            }
        }

        private bool _labelIsVisible = true;
        public bool LabelIsVisible
        {
            get => _labelIsVisible;
            set
            {
                if (_labelIsVisible == value)
                    return;
                _labelIsVisible = value;
                OnPropertyChanged();
            }
        }

        private Color _labelColor;
        public Color LabelColor
        {
            get => _labelColor;
            set
            {
                if (_labelColor == value)
                    return;
                _labelColor = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Dish> _dishes;
        public ObservableCollection<Dish> Dishes
        {
            get => _dishes;
            set
            {
                if (_dishes == value)
                    return;
                _dishes = value;
                OnPropertyChanged();
            }
        }
        public Command LoadDishesCommand { get; }
        public Command AddItemCommand { get; }
        public Command RefreshCommand { get; }
        public DishesViewModel(Page page)
        {
            Title = "My Dishes";
            Dishes = new ObservableCollection<Dish>();
            LoaderIsVisible = false;
            LoadDishesCommand = new Command(ExecuteLoadDishesCommand);
            AddItemCommand = new Command(async () => await AddItemAsync());
            RefreshCommand = new Command(RefreshAsync);
            _page = page;
            var busy = false;
            MessagingCenter.Subscribe<object, bool>(this, MessageKeys.Connection, (s, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    LabelColor = e ? Color.Green : Color.Gray;
                    ConnectionLabel = e ? "Back online" : "No connection";

                    if (LabelIsVisible && e)
                    {
                        //await ConnectionLabel.FadeTo(0, 2000);
                        busy = true;
                        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                        {
                            LabelIsVisible = false;
                            ConnectionLabelHeight = 0;
                            busy = false;
                            return false;
                        });
                    }
                    else if (!e && !busy)
                    {
                        LabelIsVisible = true;
                        ConnectionLabelHeight = 20;
                        //await ConnectionLabel.FadeTo(1, 500);
                    }

                });
            });

            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                CrossConnectivity.Current.IsRemoteReachable("192.168.0.141", 8080).ContinueWith(task =>
                {
                    MessagingCenter.Send<object, bool>(this, MessageKeys.Connection, task.Result);
                });
                return true;
            });
        }

        private Dish _itemSelected;
        public Dish ItemSelected
        {
            get => _itemSelected;
            set
            {
                if (_itemSelected == value || value == null)
                    return;
                _itemSelected = value;
                OnPropertyChanged();
                _page.Navigation.PushAsync(new NewDishPage(new DishDetailViewModel(value)));
                _itemSelected = null;
                OnPropertyChanged();
            }
        }



        private async Task AddItemAsync()
        {
            await _page.Navigation.PushAsync(new NewDishPage());
        }

        private void RefreshAsync()
        {
            Task.Run(() => GetDishes(true));
        }

        private void ExecuteLoadDishesCommand(object parameter)
        {
             if (IsBusy)
                return;

             if (parameter == null)
                 IsBusy = true;
             var list = Db.Dishes.Select(i => new Dish
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
             if (parameter == null)
                 IsBusy = false;

        }

        public async Task<HttpResponseMessage> GetDishes(bool firstStart = false)
        {
            if (firstStart)
            {
                Dishes.Clear();
                LoaderIsVisible = true;
                Thread.Sleep(1000);
            }
            var dishes = await _dishesService.GetDishes();

            if (dishes == null)
            {
                LoadDishesCommand.Execute(firstStart);
                return null;
            }
            Db.Database.EnsureCreated();
            var enumerable = dishes as Dish[] ?? dishes.ToArray();
            var list1 = enumerable.ToList();
            var list2 = Db.Dishes.ToList();
            var listId = list1.Select(d => d.Id).Except(list2.Select(d => d.Id)).ToList();
            foreach (var id in listId)
            {
                var item = enumerable.FirstOrDefault(d => d.Id == id);
                if (item != null) Db.Dishes.Add(item);
            }

            Db.SaveChanges();
            LoadDishesCommand.Execute(firstStart);
            if(firstStart)
                LoaderIsVisible = false;
            return new HttpResponseMessage(new HttpStatusCode());
        }
    }
}