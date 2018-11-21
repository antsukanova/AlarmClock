using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using AlarmClock.Managers;
using AlarmClock.Misc;
using AlarmClock.DBModels;
using AlarmClock.Properties;
using AlarmClock.Tools;

namespace AlarmClock.ViewModels
{
    internal class SignInViewModel : NotifyPropertyChanged
    {
        private string _login;
        private string _password;

        private ICommand _signIn;
        private ICommand _toSignUp;

        #region properties
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand SignIn => _signIn ?? (_signIn = new RelayCommand<object>(SignInExecute, SignInCanExecute));

        public ICommand ToSignUp => _toSignUp ?? (_toSignUp = new RelayCommand<object>(ToSignUpExecute));
        #endregion

        private static void ToSignUpExecute(object obj) => NavigationManager.Navigate(Page.SignUp);

        private async void SignInExecute(object obj)
        {
            LoaderManager.Instance.ShowLoader();

            var result = await Task.Run(() =>
            {
                User user;

                Logger.Log($"User tried to sign in with login - {Login}.");

                try
                {
                    user = DbManager.GetUserByLogin(Login);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Resources.CantGetUserError);
                    Logger.Log(ex, Resources.CantGetUserError);
                    return false;
                }

                if (user == null)
                {
                    var msg = string.Format(Resources.UserDoesntExistError, Login);

                    MessageBox.Show(msg);
                    Logger.Log(msg);
                    return false;
                }

                if (!user.IsPasswordCorrect(Password))
                {
                    MessageBox.Show(Resources.WrongPasswordError);
                    Logger.Log(Resources.WrongPasswordError);
                    return false;
                }

                DbManager.UpdateUser(user.UpdateLastVisit());
                Logger.Log($"User {user.Login} last visit time was successfully updated.");

                StationManager.Authorize(user);

                return true;
            });

            LoaderManager.Instance.HideLoader();

            if (result)
                NavigationManager.Navigate(Page.Main);
        }

        private bool SignInCanExecute(object obj) =>
            !(string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password));
    }
}
