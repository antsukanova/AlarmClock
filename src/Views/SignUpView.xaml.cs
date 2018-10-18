using System.Windows;
using System.Windows.Controls;

namespace AlarmClock.Views
{
    public partial class SignUpView
    {
        public SignUpView()
        {
            InitializeComponent();
            //DataContext = new LogInViewModel();
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                ((dynamic)DataContext).Password = ((PasswordBox)sender).SecurePassword;
        }
    }
}
