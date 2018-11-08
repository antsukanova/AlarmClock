﻿using System.ComponentModel.DataAnnotations;
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
            var userRepo = new UserRepository();
            var user = new User(Name, Surname, Login, Email, Password);

            Logger.Log("User tried to sign up with credentials:" +
                       $" Name - {Name}, Surname - {Surname}, Login - {Login}, Email - {Email}");

            if (!new EmailAddressAttribute().IsValid(Email))
            {
                var msg = string.Format(Resources.InvalidEmailError, Email);

                MessageBox.Show(msg);
                Logger.Log(msg);

                return;
            }

            if (userRepo.Exists(user))
            {
                var msg = string.Format(Resources.UserAlreadyExistsError, Email, Login);

                MessageBox.Show(msg);
                Logger.Log(msg);

                return;
            }

            userRepo.Add(user);
            Logger.Log($"User {user.Login} was successfully added to the db.");

            SerializationManager.SerializeUsers(userRepo);

            StationManager.Authorize(user);
            Logger.Log($"User {user.Login} was successfully signed up");

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
