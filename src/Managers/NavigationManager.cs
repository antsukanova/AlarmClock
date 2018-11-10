using AlarmClock.Misc;

namespace AlarmClock.Managers
{
    /// <summary>
    /// Singleton manager used to help with navigation between pages
    /// </summary>
    class NavigationManager
    {
        private static readonly object Lock = new object();

        private static NavigationManager _instance;

        private NavigationModel Model { get; set; }

        private static NavigationManager Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                // Lock in case several threads will try
                // to initialize Instance at the same time
                lock (Lock)
                    return _instance = new NavigationManager();
            }
        }

        public static NavigationModel Initialize(NavigationModel navigationModel) => 
            Instance.Model = navigationModel;

        public static void Navigate(Page page) => Instance.Model?.Navigate(page);
    }
}
