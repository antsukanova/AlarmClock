using AlarmClock.Managers;
using AlarmClock.Misc;

namespace AlarmClock
{
    public partial class MainWindow
    {
        public new object Content
        {
            get => PageContent.Content;
            set => PageContent.Content = value;
        }

        public MainWindow()
        {
            InitializeComponent();

            NavigationManager.Initialize(new NavigationModel(this))
                             .Navigate(Page.Main);//SignIn);
        }
    }
}
