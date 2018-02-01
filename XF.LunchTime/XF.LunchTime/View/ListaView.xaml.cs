using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.LunchTime.Model;
using XF.LunchTime.ViewModel;

namespace XF.LunchTime.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaView : ContentPage
    {
        

        public ListaView()
        {
            InitializeComponent();
            //carregar();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.FavoritoVM.Carregar();
        }



    }
}