using System.Windows.Controls;
using AlarmClock.Managers;
using AlarmClock.Misc;
using AlarmClock.ViewModels;
using Page = AlarmClock.Misc.Page;

namespace AlarmClock
{
    public partial class MainWindow : IContentWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new MainWindowViewModel();
            DataContext = viewModel;

            NavigationManager.Initialize(new NavigationModel(this))
                             .Navigate(StationManager.CurrentUser != null ? Page.Main : Page.SignIn);
        }

        public ContentControl ContentControl => PageContent;
    }
}
