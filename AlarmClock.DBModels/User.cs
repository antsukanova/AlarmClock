using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using AlarmClock.Tools;

namespace AlarmClock.DBModels
{
    [Serializable]
    public class User
    {
        #region properites
        public Guid Id { get; private set; }
        public string Password { get; private set; }
        public string Salt { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Login { get; private set; }
        public string Email { get; private set; }
        public DateTime LastVisited { get; private set; }
        public List<Clock> Clocks { get; private set; }

        #endregion

        #region constructors
        public User(
            string name, string surname, string login, string email, string password,
            DateTime lastVisited)// : this()
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

            Clocks = new List<Clock>();
        }

        public User(
            string name, string surname, string login, string email, string password
        ) : this(name, surname, login, email, password, DateTime.Now)
        {}

        private User()
        {
        }

        #endregion

        public bool IsPasswordCorrect(string password) => Encrypter.Hash(Salt + password).Equals(Password);

        public bool IsPasswordCorrect(User userCandidate)
        {
            try
            {
                return IsPasswordCorrect(userCandidate.Password);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public User UpdateLastVisit()
        {
            LastVisited = DateTime.Now;
            return this;
        }

        #region EntityConfiguration

        public class UserEntityConfiguration : EntityTypeConfiguration<User>
        {
            public UserEntityConfiguration()
            {
                ToTable("Users");
                HasKey(s => s.Id);
                
                Property(p => p.Id)
                    .HasColumnName("Id")
                    .IsRequired();
                Property(p => p.Password)
                    .HasColumnName("Password")
                    .IsRequired();
                Property(p => p.Salt)
                    .HasColumnName("Salt")
                    .IsRequired();
                Property(p => p.Name)
                    .HasColumnName("Name")
                    .IsOptional();
                Property(p => p.Surname)
                    .HasColumnName("Surname")
                    .IsRequired();
                Property(p => p.Login)
                    .HasColumnName("Login")
                    .IsRequired();
                Property(p => p.Email)
                    .HasColumnName("Email")
                    .IsRequired();
                Property(p => p.LastVisited)
                    .HasColumnName("LastVisited")
                    .IsRequired();

                HasMany(s => s.Clocks)
                    .WithRequired(c => c.Owner)
                    .HasForeignKey(c => c.UserId)
                    .WillCascadeOnDelete(true);
            }
        }
        #endregion
    }
}
