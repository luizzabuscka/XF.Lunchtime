using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XF.LunchTime.ViewModel;
using Xamarin.Forms;

namespace XF.LunchTime
{
    public partial class App : Application
    {
        #region ViewModels
            public static FavoritoViewModel FavoritoVM { get; set; }
        #endregion
        public static string nome = "Teste"; 

        public App()
        {
            InitializeComponent();
            InitializeApplication();
            
                MainPage = new NavigationPage(new View.LoginView());
            
            //MainPage = new NavigationPage(new MainPage());
        }
        private void InitializeApplication()
        {
            if (FavoritoVM == null) {
                FavoritoVM = new FavoritoViewModel();
            }
        }
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
