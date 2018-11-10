using AlarmClock.Managers;
using AlarmClock.Misc;
using AlarmClock.ViewModels;

using Page = AlarmClock.Misc.Page;

namespace AlarmClock
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();

            NavigationManager.Initialize(new NavigationModel(this))
                             .Navigate(StationManager.CurrentUser != null ? Page.Main : Page.SignIn);
        }
    }
}
