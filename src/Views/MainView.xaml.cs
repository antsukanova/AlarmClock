using AlarmClock.ViewModels;

using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;

namespace AlarmClock.Views
{
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void KeyDownHandler(object sender, KeyEventArgs e)//to VM
        {
            var myCaretIndex = ((TextBox)sender).CaretIndex;
            var characters   = ((TextBox)sender).Text.ToCharArray();

            if (myCaretIndex >= characters.Length)
                return;

            characters[myCaretIndex] = (char)KeyInterop.VirtualKeyFromKey(e.Key);

            ((TextBox)sender).Text = string.Join("", characters);
            ((TextBox)sender).CaretIndex = myCaretIndex + 1;

            e.Handled = true;
        }

        private void StoryboardCompleted(object sender, EventArgs e)
        {
            var alarm = ((MainViewModel)DataContext).AlarmClocks.SingleOrDefault(item => item.IsActive);
            if (alarm != null)
                alarm.IsStopped = true;
        }
    }
}
