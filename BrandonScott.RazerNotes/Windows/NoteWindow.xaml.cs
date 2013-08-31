using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BrandonScott.RazerNotes.Lib;
using Sharparam.SharpBlade.Native;

namespace BrandonScott.RazerNotes.Windows
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class NoteWindow : Window
    {
        private readonly Note _note;

        public NoteWindow() : this(new Note("Tap to add note title", "Tap to add note content"))
        {

        }

        public NoteWindow(Note note)
        {
            InitializeComponent();

            _note = note;

            NoteTitleBox.Text = _note.Title;
            NoteContentBox.Text = _note.Content;

            SharpBladeHelper.Manager.Touchpad.SetWindow(this);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK1);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK2);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK3);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK5);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK10);
            SharpBladeHelper.Manager.DynamicKeyEvent += Manager_DynamicKeyEvent;
            SharpBladeHelper.Manager.EnableDynamicKey(RazerAPI.DynamicKeyType.DK1, @".\Resources\RazerNotesSave.png");
            SharpBladeHelper.Manager.EnableDynamicKey(RazerAPI.DynamicKeyType.DK2, @".\Resources\RazerNotesBack.png");
        }

        private void SaveNote()
        {
            _note.Title = NoteTitleBox.Text;
            _note.Content = NoteContentBox.Text;
#if RAZER
            SharpBladeHelper.Manager.Touchpad.ClearWindow();
#endif
            Application.Current.MainWindow = new NotesWindow(_note);
            Close();
            Application.Current.MainWindow.Show();
            SharpBladeHelper.Manager.DynamicKeyEvent -= Manager_DynamicKeyEvent;
        }

        private void Back()
        {
#if RAZER
            SharpBladeHelper.Manager.Touchpad.ClearWindow();
#endif
            Application.Current.MainWindow = new NotesWindow();
            Close();
            Application.Current.MainWindow.Show();
            SharpBladeHelper.Manager.DynamicKeyEvent -= Manager_DynamicKeyEvent;
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            SaveNote();
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            Back();
        }

        private void Manager_DynamicKeyEvent(object sender, Sharparam.SharpBlade.Razer.Events.DynamicKeyEventArgs e)
        {
            if (e.State == RazerAPI.DynamicKeyState.Down)
            {
                switch (e.KeyType)
                {
                    case RazerAPI.DynamicKeyType.DK1:
                        SaveNote();
                        break;
                    case RazerAPI.DynamicKeyType.DK2:
                        Back();
                        break;
                }
            }
        }
    }
}
