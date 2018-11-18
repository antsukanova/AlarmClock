using AlarmClock.ViewModels;

using System;

namespace AlarmClock.Views
{
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void StoryboardCompleted(object sender, EventArgs e) =>
            (DataContext as MainViewModel)?.AlarmCompleted.Execute(e);
    }
}
