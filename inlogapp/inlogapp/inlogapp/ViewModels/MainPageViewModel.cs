using Acr.UserDialogs;
using inlogapp.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace inlogapp.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {

        public DelegateCommand<Movie> SelecionarFilmeCommand => new DelegateCommand<Movie>(SelecionarFilme);

        public DelegateCommand<String> TextChangedCommand => new DelegateCommand<String>(TextoAlterado);
        public DelegateCommand LoadFilmesCommand => new DelegateCommand(ConsultarFIlmes);

        private int _limiteFilmes;
        public int LimiteFilmes
        {
            get => _limiteFilmes;
            set => _ = SetProperty(ref _limiteFilmes, value);
        }


        private List<Movie> FilmesFixos { get; set; }

        private ObservableCollection<Movie> _queryResults;
        public ObservableCollection<Movie> QueryResults
        {
            get => _queryResults;
            set => _ = SetProperty(ref _queryResults, value);
        }

        private int _currentPage = 1;

        private Boolean _fromFiltring = false;

        public MainPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService, dialogService)
        {
            Title = "Upcoming Movies";

            LimiteFilmes = 5;

            ConsultarFIlmes();
        }

        private void TextoAlterado(String novoTexto)
        {
            try
            {
                _fromFiltring = true;
                ObservableCollection<Movie> lista;
                if (novoTexto != null)
                {

                    lista = new ObservableCollection<Movie>(
                        FilmesFixos.FindAll(filme => filme.title.ToLower().
                        Contains(novoTexto.ToLower())));

                    //Mandados.Clear();
                    QueryResults = lista;
                }
                //

            }
            catch (Exception ex)
            {

            }
        }

        private async void SelecionarFilme(Movie movie)
        {
            
            try
            {
                UserDialogs.Instance.ShowLoading("Buscando...", MaskType.Gradient);

                using (HttpResponseMessage response = await IniciarCliente().GetAsync("movie/"+movie.id+"?api_key=" + api_key + "&language=pt-BR"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string respStr = await response.Content.ReadAsStringAsync();
                        MovieDetail movieDetail = JsonConvert.DeserializeObject<MovieDetail>(respStr);

                        var navParam = new NavigationParameters();
                        navParam.Add("Filme", movieDetail);
                        await NavigationService.NavigateAsync("DetailPage", navParam);

                    }
                    else
                    {
                        await DialogService.DisplayAlertAsync("Aviso", "Falha ao buscar detalhes.", "OK");
                    }
                }

            }
            catch (Exception ex)
            {
                await DialogService.DisplayAlertAsync("Aviso", "Falha ao buscar detalhes.", "OK");

            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }



        }


        private async void ConsultarFIlmes()
        {

            if (_fromFiltring)
            {
                _fromFiltring = false;
                return;
            }
            try
            {
                UserDialogs.Instance.ShowLoading("Buscando...", MaskType.Gradient);
                if (IsBusy)
                    return;

                IsBusy = true;

                using (HttpResponseMessage response = await IniciarCliente().GetAsync("movie/upcoming?api_key=" + api_key + "&language=pt-BR&page=" + _currentPage))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string respStr = await response.Content.ReadAsStringAsync();
                        QueryResult result = JsonConvert.DeserializeObject<QueryResult>(respStr);
                        if (QueryResults == null)
                        {
                            QueryResults = new ObservableCollection<Movie>(result.results.ToList());
                            FilmesFixos = new List<Movie>(result.results.ToList());
                        }
                        else
                        {
                            foreach (Movie mov in result.results.ToList())
                            {
                                QueryResults.Add(mov);
                                FilmesFixos.Add(mov);
                            }
                        }
                        _currentPage++;
                        if (LimiteFilmes > 0)
                            LimiteFilmes--;

                    }
                    else
                    {
                        await DialogService.DisplayAlertAsync("Aviso", "Falha na consulta de filmes.", "OK");
                    }
                }

            }
            catch (Exception ex)
            {
                await DialogService.DisplayAlertAsync("Aviso", "Falha na consulta de filmes.", "OK");
            }
            finally
            {
                await Task.Delay(2000);
                IsBusy = false;
                UserDialogs.Instance.HideLoading();
            }


        }
    }
}
