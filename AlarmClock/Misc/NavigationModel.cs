using System;
using AlarmClock.Properties;
using AlarmClock.Views;

namespace AlarmClock.Misc
{
    public enum Page
    {
        SignIn,
        SignUp,
        Main
    }

    public class NavigationModel
    {
        private readonly MainWindow _contentWindow;

        public NavigationModel(MainWindow contentWindow) => _contentWindow = contentWindow;

        public void Navigate(Page page)
        {
            switch (page)
            {
                case Page.SignIn:
                    _contentWindow.ContentControl.Content = new SignInView();
                    break;
                case Page.SignUp:
                    _contentWindow.ContentControl.Content = new SignUpView();
                    break;
                case Page.Main:
                    _contentWindow.ContentControl.Content = new MainView();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(page), page, Resources.UnknownPageError);
            }
        }

    }
}