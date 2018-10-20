using System;
using System.Windows;
using System.Windows.Input;

using AlarmClock.Managers;
using AlarmClock.Misc;
using AlarmClock.Models;
using AlarmClock.Properties;
using AlarmClock.Repositories;

namespace AlarmClock.ViewModels
{
    class SignUpViewModel : NotifyPropertyChanged
    {
        private string _name;
        private string _surname;
        private string _email;
        private string _login;
        private string _password;

        private ICommand _signUp;
        private ICommand _toSignIn;

        #region properties
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

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

        public ICommand SignUp => _signUp ?? (_signUp = new RelayCommand(SignUpExecute, SignUpCanExecute));

        public ICommand ToSignIn => _toSignIn ?? (_toSignIn = new RelayCommand(ToSignInExecute));
        #endregion

        private static void ToSignInExecute(object obj) => NavigationManager.Navigate(Page.SignIn);

        private void SignUpExecute(object obj)
        {
            User currentUser = null;

            /*try
            {
                currentUser = new UserRepository().Find(EmailOrLogin);
            }
            catch (Exception)
            {
                MessageBox.Show(string.Format(Resources.CantGetUserError));
                return;
            }

            if (currentUser == null)
            {
                MessageBox.Show(string.Format(Resources.UserDoesntExistError, EmailOrLogin));
                return;
            }

            if (!currentUser.IsPasswordCorrect(Password))
            {
                MessageBox.Show(Resources.WrongPasswordError);
                return;
            }*/

            StationManager.CurrentUser = currentUser;

            NavigationManager.Navigate(Page.Main);
        }

        private bool SignUpCanExecute(object obj)
            => !(string.IsNullOrWhiteSpace(Name)    ||
                 string.IsNullOrWhiteSpace(Surname) ||
                 string.IsNullOrWhiteSpace(Email)   ||
                 string.IsNullOrWhiteSpace(Login)   ||
                 string.IsNullOrWhiteSpace(Password));
    }
}
