using System;
using AlarmClock.Misc;

namespace AlarmClock.Models
{
    [Serializable]
    public class User
    {
        #region properites
        public Guid Id { get; }
        public string Password { get; }
        public string Salt { get; }
        public string Name { get; }
        public string Surname { get; }
        public string Login { get; }
        public string Email { get; }
        public DateTime LastVisited { get; private set; }

        #endregion

        #region constructors
        internal User(
            string name, string surname, string login, string email, string password,
            DateTime lastVisited
        )
        {
            Id = Guid.NewGuid();

            Name = name;
            Surname = surname;
            Login = login;
            Email = email;

            var hashObj = Encrypter.Encode(password);

            Password = hashObj.Hash;
            Salt = hashObj.Salt;

            LastVisited = lastVisited;
        }

        public User(
            string name, string surname, string login, string email, string password
        ) : this(name, surname, login, email, password, DateTime.Now)
        {}
        #endregion

        public bool IsPasswordCorrect(string password) => Encrypter.Hash(Salt + password).Equals(Password);

        public User UpdateLastVisit()
        {
            LastVisited = DateTime.Now;
            return this;
        }
    }
}
