using System.Windows;
using BrandonScott.RazerNotes.Lib;

namespace BrandonScott.RazerNotes.Windows
{
    /// <summary>
    /// Interaction logic for winNotes.xaml
    /// </summary>
    public partial class NotesWindow
    {
        public NotesWindow()
        {
            InitializeComponent();

#if RAZER
            SharpBladeHelper.Manager.Touchpad.SetWindow(this);
#endif
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = new MainWindow();
#if RAZER
            SharpBladeHelper.Manager.Touchpad.ClearWindow();
#endif
            Close();
            Application.Current.MainWindow.Show();
        }
    }
}
