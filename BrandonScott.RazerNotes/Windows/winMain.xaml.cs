using System.Windows;
using BrandonScott.RazerNotes.Lib;

namespace BrandonScott.RazerNotes.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddNoteButtonClick(object sender, RoutedEventArgs e)
        {
            NoteManager.GetManager().Add(new Note(NoteTitleBox.Text, NoteContentBox.Text));
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = new NotesWindow();
            Close();
            Application.Current.MainWindow.Show();
        }
    }
}
