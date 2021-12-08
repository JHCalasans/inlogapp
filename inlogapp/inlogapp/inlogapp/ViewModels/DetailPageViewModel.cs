using inlogapp.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;

namespace inlogapp.ViewModels
{
    public class DetailPageViewModel : ViewModelBase
    {

        private MovieDetail _filme;
        public MovieDetail Filme
        {
            get => _filme;
            set => _ = SetProperty(ref _filme, value);
        }
        public DetailPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {

        }

        public DelegateCommand CompartilharFilmeCommand => new DelegateCommand(CompartilharFilme);
        

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("Filme"))
            {
               Filme = (MovieDetail)parameters["Filme"];

            }
        }

        private async void CompartilharFilme()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = Filme.overview,
                Title = Filme.title
            });
        }
    }
}
