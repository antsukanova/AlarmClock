using System.Windows;

using AlarmClock.ViewModels;

namespace AlarmClock.Views
{
    public partial class SignUpView
    {
        private string _password
        {
            get => ((SignUpViewModel)DataContext).Password;
            set => ((SignUpViewModel)DataContext).Password = value;
        }

        public SignUpView()
        {
            InitializeComponent();
            DataContext = new SignUpViewModel();
        }

        private void PasswordChanged(object sender, RoutedEventArgs e) =>_password = Password.Password;

    }
}
