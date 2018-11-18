using System.ComponentModel;
using System.Windows;

namespace AlarmClock.Tools
{
    public interface ILoaderOwner : INotifyPropertyChanged
    {
        Visibility LoaderVisibility { get; set; }
        bool IsEnabled { get; set; }
    }
}
