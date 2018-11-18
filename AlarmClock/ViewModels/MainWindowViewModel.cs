using System.Windows;

using AlarmClock.Managers;
using AlarmClock.Misc;
using AlarmClock.Tools;

namespace AlarmClock.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChanged, ILoaderOwner
    {
        private Visibility _visibility = Visibility.Hidden;
        private bool _isEnabled = true;

        public Visibility LoaderVisibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                OnPropertyChanged(nameof(LoaderVisibility));
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        public MainWindowViewModel() => LoaderManager.Instance.Initialize(this);
    }
}
