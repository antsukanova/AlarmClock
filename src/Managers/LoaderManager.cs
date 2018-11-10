using System.Windows;
using AlarmClock.Misc;

namespace AlarmClock.Managers
{
    internal class LoaderManager
    {
        private static readonly object Lock = new object();

        private static LoaderManager _instance;

        private ILoaderOwner _loaderOwner;

        internal static LoaderManager Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                // Lock in case several threads will try
                // to initialize Instance at the same time
                lock (Lock)
                    return _instance = new LoaderManager();
            }
        }

        internal void Initialize(ILoaderOwner loaderOwner) => _loaderOwner = loaderOwner;

        internal void ShowLoader()
        {
            _loaderOwner.LoaderVisibility = Visibility.Visible;
            _loaderOwner.IsEnabled = false;

        }

        internal void HideLoader()
        {
            _loaderOwner.LoaderVisibility = Visibility.Hidden;
            _loaderOwner.IsEnabled = true;
        }
    }
}
