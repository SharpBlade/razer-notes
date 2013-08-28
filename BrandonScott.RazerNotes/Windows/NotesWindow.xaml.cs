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
#if RAZER
            SharpBladeHelper.Manager.Touchpad.ClearWindow();
#endif
            Application.Current.MainWindow = new MainWindow();
            Close();
            Application.Current.MainWindow.Show();
        }
    }
}
