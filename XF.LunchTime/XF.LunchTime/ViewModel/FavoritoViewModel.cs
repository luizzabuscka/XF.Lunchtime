using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XF.LunchTime.Model;
using Xamarin.Forms;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace XF.LunchTime.ViewModel
{
    public class FavoritoViewModel : INotifyPropertyChanged
    {
        #region Propriedades

        public Favoritos FavoritoModel { get; set; }
        
        private Favoritos selecionado;
        public Favoritos Selecionado
        {
            get { return selecionado; }
            set
            {
                selecionado = value as Favoritos;
                EventPropertyChanged();
            }
        }

        
        public List<Favoritos> CopiaListaFavoritos;
        public ObservableCollection<Favoritos> Favoritos { get; set; } = new ObservableCollection<Favoritos>();

        // UI Events
        public OnAdicionarFavoritoCMD OnAdicionarFavoritoCMD { get; }
        public OnEditarFavoritoCMD OnEditarFavoritoCMD { get; }
        public OnDeleteFavoritoCMD OnDeleteFavoritoCMD { get; }
        public ICommand OnSairCMD { get; private set; }
        public ICommand OnNovoCMD { get; private set; }

        #endregion

        public FavoritoViewModel()
        {
            FavoritoRepository repository = FavoritoRepository.Instance;

            OnAdicionarFavoritoCMD = new OnAdicionarFavoritoCMD(this);
            OnEditarFavoritoCMD = new OnEditarFavoritoCMD(this);
            OnDeleteFavoritoCMD = new OnDeleteFavoritoCMD(this);
            OnSairCMD = new Command(OnSair);
            OnNovoCMD = new Command(OnNovo);

            CopiaListaFavoritos = new List<Favoritos>();
            Carregar();
        }

        public void Carregar()
        {
            CopiaListaFavoritos = FavoritoRepository.GetFavoritos().ToList();
            AplicarFiltro();
        }
        private string pesquisaPorNome;
        public string PesquisaPorNome
        {
            get { return pesquisaPorNome; }
            set
            {
                if (value == pesquisaPorNome) return;

                pesquisaPorNome = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PesquisaPorNome)));
                AplicarFiltro();
            }
        }
        private void AplicarFiltro()
        {
            if (pesquisaPorNome == null)
                pesquisaPorNome = "";

            var resultado = CopiaListaFavoritos.Where(n => n.Nome.ToLowerInvariant()
                                .Contains(PesquisaPorNome.ToLowerInvariant().Trim())).ToList();

            var removerDaLista = Favoritos.Except(resultado).ToList();
            foreach (var item in removerDaLista)
            {
                Favoritos.Remove(item);
            }

            for (int index = 0; index < resultado.Count; index++)
            {
                var item = resultado[index];
                if (index + 1 > Favoritos.Count || !Favoritos[index].Equals(item))
                    Favoritos.Insert(index, item);
            }
        }

        public void Adicionar(Favoritos paramFavorito)
        {
            if ((paramFavorito == null) || (string.IsNullOrWhiteSpace(paramFavorito.Nome)) || (string.IsNullOrWhiteSpace(paramFavorito.Endereco)) || (string.IsNullOrWhiteSpace(paramFavorito.telefone)))
                App.Current.MainPage.DisplayAlert("Atenção", "Os campos nome, endereço e telefone são obrigatórios", "OK");
            else if (FavoritoRepository.SalvarFavorito(paramFavorito) > 0)
                App.Current.MainPage.Navigation.PopAsync();
            else
                App.Current.MainPage.DisplayAlert("Falhou", "Desculpe, ocorreu um erro inesperado =(", "OK");
        }

        public async void Editar()
        {
            await App.Current.MainPage.Navigation.PushAsync(
                new View.AddView() { BindingContext = App.FavoritoVM });
        }

        public async void Remover()
        {
            if (await App.Current.MainPage.DisplayAlert("Atenção?",
                string.Format("Tem certeza que deseja remover o {0}?", Selecionado.Nome), "Sim", "Não"))
            {
                if (FavoritoRepository.RemoverFavorito(Selecionado.Id) > 0)
                {
                    CopiaListaFavoritos.Remove(Selecionado);
                    Carregar();
                }
                else
                    await App.Current.MainPage.DisplayAlert(
                            "Falhou", "Desculpe, ocorreu um erro inesperado =(", "OK");
            }
        }

        private async void OnSair()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }

        private void OnNovo()
        {
            App.FavoritoVM.Selecionado = new Model.Favoritos();
            App.Current.MainPage.Navigation.PushAsync(
                new View.AddView() { BindingContext = App.FavoritoVM });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void EventPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class OnAdicionarFavoritoCMD : ICommand
    {
        private FavoritoViewModel favoritoVM;
        public OnAdicionarFavoritoCMD(FavoritoViewModel paramVM)
        {
            favoritoVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void AdicionarCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter)
        {
            favoritoVM.Adicionar(parameter as Favoritos);
        }
    }

    public class OnEditarFavoritoCMD : ICommand
    {
        private FavoritoViewModel favoritoVM;
        public OnEditarFavoritoCMD(FavoritoViewModel paramVM)
        {
            favoritoVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void EditarCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) => (parameter != null);
        public void Execute(object parameter)
        {
            App.FavoritoVM.Selecionado = parameter as Favoritos;
            favoritoVM.Editar();
        }
    }

    public class OnDeleteFavoritoCMD : ICommand
    {
        private FavoritoViewModel favoritoVM;
        public OnDeleteFavoritoCMD(FavoritoViewModel paramVM)
        {
            favoritoVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void DeleteCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) => (parameter != null);
        public void Execute(object parameter)
        {
            App.FavoritoVM.Selecionado = parameter as Favoritos;
            favoritoVM.Remover();
        }
    }
}
