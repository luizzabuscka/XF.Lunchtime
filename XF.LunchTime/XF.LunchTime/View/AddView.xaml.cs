using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.LunchTime.Model;

namespace XF.LunchTime.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddView : ContentPage 
    {
        private int favoritoId = 0;

        public AddView()
        {
            InitializeComponent();
        }
        public AddView(int Id)
        {
            InitializeComponent();
            var favorito = FavoritoRepository.GetFavorito(Id);
            txtNome.Text = favorito.Nome;
            txtEndereco.Text = favorito.Endereco;
            txtTelefone.Text = favorito.telefone;
            favorito.Id = Id;
            favoritoId = Id;
        }
        public void OnSalvar(object sender, EventArgs args)
        {
            Favoritos favoritos = new Favoritos()
            {
                Id = favoritoId,
                Nome = txtNome.Text,
                Endereco = txtEndereco.Text,
                telefone = txtTelefone.Text
                
            };
            FavoritoRepository.SalvarFavorito(favoritos);
            limpar();
            Navigation.PopAsync();


        }
        public void limpar()
        {
            txtNome.Text = txtEndereco.Text = txtTelefone.Text = string.Empty;

        }
    }
}