using System.Windows;
using System.Windows.Controls;

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

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                ((dynamic)DataContext).Password = ((PasswordBox)sender).Password;
        }
    }
}
