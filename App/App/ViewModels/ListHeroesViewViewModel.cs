using App.Interfaces;
using App.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace App.ViewModels
{
    public class ListHeroesViewViewModel : ViewModelBase
    {
        private readonly INavigationService _navigation;
        private readonly IHeroes _heroes;


        private ObservableCollection<Result> _herois;
        public ObservableCollection<Result> Herois
        {
            get { return _herois; }
            set 
            { 
                _herois = value;
                RaisePropertyChanged();
            }
        }

        public ListHeroesViewViewModel(INavigationService navigationService, IHeroes heroes) : base(navigationService)
        {
            _navigation = navigationService;
            _heroes = heroes;
            Title = "Heroes";
        }


        //OnAppearing
        public async override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            var limit = parameters.GetValue<string>(ParansKeys.Limite);

            try
            {
                IsBusy = true;
                var heroes = await _heroes.GetHeroes(limit);
                Herois = new ObservableCollection<Result>(heroes.data.results);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Atenção", $"Error:{ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
            
        }
    }
}
