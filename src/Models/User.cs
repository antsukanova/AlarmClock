using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using AlarmClock.Annotations;

namespace AlarmClock.Models
{
    public class User : INotifyPropertyChanged
    {
        private string _id;
        private string _name;
        private string _surname;
        private string _login;
        private string _email;
        private string _password;
        private DateTime _lastVisited;

        #region properites
        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

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

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
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

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public DateTime LastVisited
        {
            get => _lastVisited;
            set
            {
                _lastVisited = value;
                OnPropertyChanged(nameof(LastVisited));
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
