using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using AlarmClock.Managers;
using AlarmClock.Misc;
using AlarmClock.DBModels;
using AlarmClock.Properties;
using AlarmClock.Repositories;
using AlarmClock.Tools;

namespace AlarmClock.ViewModels
{
    internal class SignInViewModel : NotifyPropertyChanged
    {
        private string _emailOrLogin;
        private string _password;

        private ICommand _signIn;
        private ICommand _toSignUp;

        #region properties
        public string EmailOrLogin
        {
            get => _emailOrLogin;
            set
            {
                _emailOrLogin = value;
                OnPropertyChanged(nameof(EmailOrLogin));
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
                var userRepo = new UserRepository();

                Logger.Log($"User tried to sign in with email or login - {EmailOrLogin}.");

                try
                {
                    user = userRepo.Find(EmailOrLogin);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Resources.CantGetUserError);
                    Logger.Log(ex, Resources.CantGetUserError);
                    return false;
                }

                if (user == null)
                {
                    var msg = string.Format(Resources.UserDoesntExistError, EmailOrLogin);

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

                userRepo.Update(user.UpdateLastVisit());
                Logger.Log($"User {user.Login} last visit time was successfully updated.");

                StationManager<UserRepository>.Authorize(user);

                return true;
            });

            LoaderManager.Instance.HideLoader();
            if (result)
                NavigationManager.Navigate(Page.Main);
        }

        private bool SignInCanExecute(object obj) =>
            !(string.IsNullOrWhiteSpace(EmailOrLogin) || string.IsNullOrWhiteSpace(Password));
    }
}
