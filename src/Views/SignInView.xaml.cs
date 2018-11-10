using System.Windows;

using AlarmClock.ViewModels;

namespace AlarmClock.Views
{
    public partial class SignInView
    {
        public SignInView()
        {
            InitializeComponent();

            DataContext = new SignInViewModel();
        }

        private void PasswordChanged(object sender, RoutedEventArgs e) =>
            ((SignInViewModel)DataContext).Password = Password.Password;
    }
}
