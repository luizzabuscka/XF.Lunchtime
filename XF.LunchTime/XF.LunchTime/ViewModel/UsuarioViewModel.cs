using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XF.LunchTime.Model;

namespace XF.LunchTime.ViewModel
{
    public class UsuarioViewModel
    {
        #region Propriedade
        public User UsuarioModel { get; set; }
        public string Nome { get; set; }
        public string Stream { get; set; }

        // UI Events
        public IsAutenticarCMD IsAutenticarCMD { get; }
        #endregion

        public UsuarioViewModel()
        {
            UsuarioModel = new User();
            IsAutenticarCMD = new IsAutenticarCMD(this);
        }

        public void IsAutenticar(User paramUser)
        {
            this.Nome = paramUser.Nome;
            if (UserRepository.IsAutorizado(paramUser))
                App.Current.MainPage.Navigation.PushAsync(
                    new View.ListaView() { BindingContext = App.FavoritoVM });
            else
                App.Current.MainPage.DisplayAlert("Atenção", "Usuário não autorizado", "Ok");
        }

    }

    public class IsAutenticarCMD : ICommand
    {
        private UsuarioViewModel usuarioVM;
        public IsAutenticarCMD(UsuarioViewModel paramVM)
        {
            usuarioVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void DeleteCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter)
        {
            if (parameter != null) return true;

            return false;
        }
        public void Execute(object parameter)
        {
            usuarioVM.IsAutenticar(parameter as User);
        }
    }
}
