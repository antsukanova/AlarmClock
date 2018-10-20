using AlarmClock.ViewModels;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace AlarmClock.Views
{
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void KeyDownHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int myCaretIndex = ((TextBox)sender).CaretIndex;
            char[] characters = ((TextBox)sender).Text.ToCharArray();

            if (myCaretIndex < characters.Length)
            {
                characters[myCaretIndex] = (Char)KeyInterop.VirtualKeyFromKey(e.Key);
                ((TextBox)sender).Text = string.Join("", characters);
                ((TextBox)sender).CaretIndex = myCaretIndex + 1;
                e.Handled = true;
            }
        }
    }
}
