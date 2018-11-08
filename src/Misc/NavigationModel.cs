using System;

using AlarmClock.Properties;
using AlarmClock.ViewModels;
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
        private MainView   _mainView;

        public NavigationModel(MainWindow contentWindow) => _contentWindow = contentWindow;

        public void Navigate(Page page)
        {
            switch (page)
            {
                case Page.SignIn:
                    var siView = new SignInView();
                    _contentWindow.Content = siView;
                    siView.CheckCurrentUser();
                    break;
                case Page.SignUp:
                    _contentWindow.Content = new SignUpView();
                    break;
                case Page.Main:
                    ((MainViewModel)(_mainView = _mainView ?? new MainView()).DataContext).Changed();
                    _contentWindow.Content = _mainView;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(page), page, Resources.UnknownPageError);
            }
        }

    }
}