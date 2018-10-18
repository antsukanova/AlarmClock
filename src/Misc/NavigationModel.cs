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
        private SignInView _signInView;
        private SignUpView _signUpView;
        private MainView   _mainView;

        public NavigationModel(MainWindow contentWindow) => _contentWindow = contentWindow;

        public void Navigate(Page page)
        {
            switch (page)
            {
                case Page.SignIn:
                    _contentWindow.Content = _signInView ?? (_signInView = new SignInView());
                    break;
                case Page.SignUp:
                    _contentWindow.Content = _signUpView ?? (_signUpView = new SignUpView());
                    break;
                case Page.Main:
                    _contentWindow.Content = _mainView ?? (_mainView = new MainView());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(page), page, Resources.UnknownPageError);
            }
        }

    }
}