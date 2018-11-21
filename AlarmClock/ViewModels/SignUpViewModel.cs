using System.ComponentModel.DataAnnotations;
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
    internal class SignUpViewModel : NotifyPropertyChanged
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

        public ICommand SignUp => _signUp ?? (_signUp = new RelayCommand<object>(SignUpExecute, SignUpCanExecute));

        public ICommand ToSignIn => _toSignIn ?? (_toSignIn = new RelayCommand<object>(ToSignInExecute));
        #endregion

        private static void ToSignInExecute(object obj) => NavigationManager.Navigate(Page.SignIn);

        private async void SignUpExecute(object obj)
        {
            LoaderManager.Instance.ShowLoader();
            var result = await Task.Run(() =>
            {
                var user = new User(Name, Surname, Login, Email, Password);

                Logger.Log("User tried to sign up with credentials: " +
                           $"Name - {Name}, Surname - {Surname}, Login - {Login}, Email - {Email}");

                if (!new EmailAddressAttribute().IsValid(Email))
                {
                    var msg = string.Format(Resources.InvalidEmailError, Email);

                    MessageBox.Show(msg);
                    Logger.Log(msg);

                    return false;
                }

                if (DbManager.UserExists(user.Login))
                {
                    var msg = string.Format(Resources.UserAlreadyExistsError, Email, Login);

                    MessageBox.Show(msg);
                    Logger.Log(msg);

                    return false;
                }

                DbManager.AddUser(user);
                Logger.Log($"User {user.Login} was successfully added to the db.");

                StationManager.Authorize(user);

                return true;
            });

            LoaderManager.Instance.HideLoader();
            if (result)
                NavigationManager.Navigate(Page.Main);
        }

        private bool SignUpCanExecute(object obj) => 
           !(string.IsNullOrWhiteSpace(Name)    ||
             string.IsNullOrWhiteSpace(Surname) ||
             string.IsNullOrWhiteSpace(Email)   ||
             string.IsNullOrWhiteSpace(Login)   ||
             string.IsNullOrWhiteSpace(Password));
    }
}
