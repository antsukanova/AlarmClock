using System;
using AlarmClock.Misc;

namespace AlarmClock.Models
{
    public class User : NotifyPropertyChanged
    {
        public  Guid Id { get; }
        private string _name;
        private string _surname;
        private string _login;
        private string _email;
        public  string Password { get; }
        public  string Salt { get; }
        private DateTime _lastVisited;

        #region constructor
        public User(
            string name, string surname, string login, string email, string password,
            DateTime lastVisited
        )
        {
            Id = Guid.NewGuid();

            Name = name;
            Surname = surname;
            Login = login;
            Email = email;

            (Password, Salt) = Encrypter.Encode(password);

            LastVisited = lastVisited;
        }
        #endregion

        #region properites
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

        public bool IsPasswordCorrect(string password) => Encrypter.Hash(password + Salt).Equals(Password);
    }
}
