using System.Windows;

using AlarmClock.ViewModels;

namespace AlarmClock.Views
{
    public partial class SignUpView
    {
        public SignUpView()
        {
            InitializeComponent();
            DataContext = new SignUpViewModel();
        }

        private void PasswordChanged(object sender, RoutedEventArgs e) => 
            ((SignUpViewModel)DataContext).Password = Password.Password;
    }
}
