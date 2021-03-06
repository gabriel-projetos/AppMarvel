﻿using App.Views;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;

namespace App.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigation;
        public ICommand GoListPageHeroesCommand { get; set; }
        public string Limit { get; set; }

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";
            _navigation = navigationService;

            GoListPageHeroesCommand = new Command(() =>
            {
                var param = new NavigationParameters
                {
                    { ParansKeys.Limite, Limit }
                };
                _navigation.NavigateAsync(nameof(ListHeroesView), param);
            });
        }
    }
}
