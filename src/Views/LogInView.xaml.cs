using System.Windows;
using System.Windows.Controls;

using AlarmClock.ViewModels;

namespace AlarmClock.Views
{
    public partial class LogInView : UserControl
    {
        public LogInView()
        {
            InitializeComponent();
            DataContext = new LogInViewModel();
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
                ((dynamic)DataContext).Password = ((PasswordBox)sender).SecurePassword;
        }
    }
}
