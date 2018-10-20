using System.Windows;
using System.Windows.Controls;

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

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                ((dynamic)DataContext).Password = ((PasswordBox)sender).Password;
        }
    }
}
