using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Threading;
using AlarmClock.Annotations;
using AlarmClock.Misc;

namespace AlarmClock.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        private readonly Regex _regex = new Regex("[^0-9.-]+");

        private string _textTime;
        private string _textHour = "00";
        private string _textMinute = "00";
        private ICommand _clickUpHour;
        private ICommand _clickDownHour;
        private ICommand _clickUpMinute;
        private ICommand _clickDownMinute;

        public string TextTime
        {
            get => _textTime;
            set
            {
                _textTime = value;
                OnPropertyChanged(nameof(TextTime));
            }
        }

        public string TextHour
        {
            get => _textHour;
            set
            {
                if (IsValidTime(value, 23))
                {
                    _textHour = value;
                    OnPropertyChanged(nameof(TextHour));
                }
            }
        }

        public string TextMinute
        {
            get => _textMinute;
            set
            {
                if (IsValidTime(value, 59))
                {
                    _textMinute = value;
                    OnPropertyChanged(nameof(TextMinute));
                }
            }
        }

        public ICommand ClickUpHour => _clickUpHour ?? (_clickUpHour = new RelayCommand(delegate { SpinChange(ref _textHour, nameof(TextHour), 1, 23); }));
        public ICommand ClickDownHour => _clickDownHour ?? (_clickDownHour = new RelayCommand(delegate { SpinChange(ref _textHour, nameof(TextHour), -1, 23); }));

        public ICommand ClickUpMinute => _clickUpMinute ?? (_clickUpMinute = new RelayCommand(delegate { SpinChange(ref _textMinute, nameof(TextMinute), 1, 59); }));
        public ICommand ClickDownMinute => _clickDownMinute ?? (_clickDownMinute = new RelayCommand(delegate { SpinChange(ref _textMinute, nameof(TextMinute), -1, 59); }));

        private void SpinChange(ref string v, string obj, int offset, int highBound)
        {
            var newValue = Int32.Parse(v) + offset;
            v = $"{(newValue == -1 ? highBound : newValue == highBound + 1 ? 0 : newValue):00}";
            OnPropertyChanged(obj);
        }

              
        private bool IsValidTime(string text, int param) => !_regex.IsMatch(text) && text.Length == 2 && 
            Int32.Parse(text) >= 0 && Int32.Parse(text) <= param;

        public MainViewModel()
        {
            SetTimer();
        }

        private void SetTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e) =>
            TextTime = DateTime.Now.ToString("H:mm:ss");

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
