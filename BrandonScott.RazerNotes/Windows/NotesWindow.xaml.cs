using System.Windows;
using BrandonScott.RazerNotes.Lib;
using BrandonScott.RazerNotes.ViewModels;
using Sharparam.SharpBlade.Native;

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
            SharpBladeHelper.Manager.DynamicKeyEvent += Manager_DynamicKeyEvent;
            SharpBladeHelper.Manager.EnableDynamicKey(RazerAPI.DynamicKeyType.DK1, @".\Resources\RazerNotesAdd.png");
            SharpBladeHelper.Manager.EnableDynamicKey(RazerAPI.DynamicKeyType.DK2, @".\Resources\RazerNotesEdit.png");
            SharpBladeHelper.Manager.EnableDynamicKey(RazerAPI.DynamicKeyType.DK3, @".\Resources\RazerNotesDelete.png");
#endif
        }
          
        public NotesWindow(Note note) : this()
        {
            var notesVm = (NotesViewModel) DataContext;
            if (!notesVm.Notes.Contains(note))
                notesVm.Notes.Add(note);
            else
                notesVm.Notes.Save();
        }

        private void NewClick(object sender, RoutedEventArgs e)
        {
            NewNote();
        }
        private void NewNote()
        {
#if RAZER
            SharpBladeHelper.Manager.Touchpad.ClearWindow();
#endif
            Application.Current.MainWindow = new NoteWindow();
            Close();
            Application.Current.MainWindow.Show();
            SharpBladeHelper.Manager.DynamicKeyEvent -= Manager_DynamicKeyEvent;
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            DeleteNote();
        }
        private void DeleteNote()
        {
            var selectedItem = NotesListBox.SelectedItem;

            if (selectedItem == null)
                return;

            ((NotesViewModel)DataContext).Notes.Remove((Note)selectedItem);
        }
        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            EditNote();
        }
        private void EditNote()
        {
            var selectedItem = NotesListBox.SelectedItem;

            if (selectedItem == null)
                return;

            Application.Current.MainWindow = new NoteWindow((Note)selectedItem);
            Close();
            Application.Current.MainWindow.Show();
            SharpBladeHelper.Manager.DynamicKeyEvent -= Manager_DynamicKeyEvent;
        }
        private void ParameterFufill(object sender, System.EventArgs e)
        {
            //To fufill parameter
        }

        private void Manager_DynamicKeyEvent(object sender, Sharparam.SharpBlade.Razer.Events.DynamicKeyEventArgs e)
        {
            switch (e.KeyType)
            {
                case Sharparam.SharpBlade.Native.RazerAPI.DynamicKeyType.DK1:
                    NewNote();
                    break;
                case Sharparam.SharpBlade.Native.RazerAPI.DynamicKeyType.DK2:
                    EditNote();
                    break;
                case Sharparam.SharpBlade.Native.RazerAPI.DynamicKeyType.DK3:
                    DeleteNote();
                    break;
            }
        }

    }
}
