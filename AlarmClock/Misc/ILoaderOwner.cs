using System.ComponentModel;
using System.Windows;

namespace AlarmClock.Misc
{
    internal interface ILoaderOwner : INotifyPropertyChanged
    {
        Visibility LoaderVisibility { get; set; }
        bool IsEnabled { get; set; }
    }
}
