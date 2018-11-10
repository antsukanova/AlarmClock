using AlarmClock.Managers;
using AlarmClock.Misc;
using AlarmClock.ViewModels;

namespace AlarmClock
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new MainWindowViewModel();
            DataContext = viewModel;

            NavigationManager.Initialize(new NavigationModel(this))
                             .Navigate(StationManager.CurrentUser != null ? Page.Main : Page.SignIn);
        }
    }
}
