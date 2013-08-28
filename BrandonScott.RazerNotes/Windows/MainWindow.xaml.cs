using System.Windows;
using BrandonScott.RazerNotes.Lib;

namespace BrandonScott.RazerNotes.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

#if RAZER
            SharpBladeHelper.Manager.Touchpad.SetWindow(this);
#endif
        }

        private void AddNoteButtonClick(object sender, RoutedEventArgs e)
        {
            NoteManager.GetManager().Add(new Note(NoteTitleBox.Text, NoteContentBox.Text));
        }

        // go to notes list
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = new NotesWindow();
#if RAZER
            SharpBladeHelper.Manager.Touchpad.ClearWindow();
#endif
            Close();
            Application.Current.MainWindow.Show();
        }

        // safe exit button
        private void Button_Click(object sender, RoutedEventArgs e)
        {
#if RAZER
            SharpBladeHelper.Manager.Stop();
#endif
            Close();
        }
    }
}
