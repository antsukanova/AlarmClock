using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security;

using AlarmClock.Annotations;
using AlarmClock.Models;

namespace AlarmClock.ViewModels
{
    class SignInViewModel : INotifyPropertyChanged
    {
        private List<User> CorrectUsers { get; }

        private string _emailOrLogin;
        private SecureString _password;

        public string EmailOrLogin
        {
            get => _emailOrLogin;
            set
            {
                _emailOrLogin = value;
                OnPropertyChanged(nameof(EmailOrLogin));
            }
        }

        public SecureString Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public SignInViewModel()
        {
            CorrectUsers = new List<User>
            {
                new User {Email = "e@ma.il"  , Login = "MyLogin", Password = "12345"},
                new User {Email = "my@ema.il", Login = "Login"  , Password = "54321"},
                new User {Email = "mail@a.b" , Login = "Log In" , Password = "11111"}
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
