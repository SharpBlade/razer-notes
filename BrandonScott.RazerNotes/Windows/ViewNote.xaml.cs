using System.Windows;
using System.Windows.Controls;
using BrandonScott.RazerNotes.Lib;
using Sharparam.SharpBlade.Native;
using Sharparam.SharpBlade.Razer.Events;

namespace BrandonScott.RazerNotes.Windows
{
    /// <summary>
    /// Interaction logic for ViewNote.xaml
    /// </summary>
    public partial class ViewNote
    {
        private readonly Note _note;

        public ViewNote(Note note)
        {
            InitializeComponent();

            _note = note;

            NoteTitleBox.Text = _note.Title;
            NoteContentBox.Text = _note.Content;
       
#if RAZER
            SharpBladeHelper.Manager.Touchpad.SetWindow(this);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK1);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK2);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK3);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK4);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK5);
            SharpBladeHelper.Manager.DisableDynamicKey(RazerAPI.DynamicKeyType.DK10);
            SharpBladeHelper.Manager.DynamicKeyEvent += OnDynamicKeyEvent;
            SharpBladeHelper.Manager.EnableDynamicKey(RazerAPI.DynamicKeyType.DK1, @".\Resources\RazerNotesBack.png");
#endif    
        }

        private void Back()
        {
#if RAZER
            SharpBladeHelper.Manager.Touchpad.ClearWindow();
            SharpBladeHelper.Manager.DynamicKeyEvent -= OnDynamicKeyEvent;
            SharpBladeHelper.Manager.SetKeyboardCapture(false);
#endif
            Application.Current.MainWindow = new NotesWindow();
            Close();
            Application.Current.MainWindow.Show();
        }

        private void OnDynamicKeyEvent(object sender, DynamicKeyEventArgs e)
        {
            if (e.State == RazerAPI.DynamicKeyState.Down)
            {
                switch (e.KeyType)
                {
                    case RazerAPI.DynamicKeyType.DK1:
                        Back();
                        break;
                }
            }
        }   
    }
}
