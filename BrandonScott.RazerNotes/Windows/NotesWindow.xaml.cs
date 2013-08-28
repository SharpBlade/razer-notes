using System.Windows;

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
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = new MainWindow();
            Close();
            Application.Current.MainWindow.Show();
        }
    }
}
