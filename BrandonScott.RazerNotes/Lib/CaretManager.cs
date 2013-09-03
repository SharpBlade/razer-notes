using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BrandonScott.RazerNotes.Lib
{
    public class CaretManager : IDisposable
    {
        private const char CaretChar = '|';
        private const char BlankChar = ' ';

        private static readonly TimeSpan BlinkDelay = new TimeSpan(0, 0, 0, 0, 800);

        private TextBox _textBox;
        private TextBlock _textBlock;
        private DispatcherTimer _timer;

        private bool _caretActive;

        private int _lastCaretIndex = -1;

        public int CaretIndex;

        public CaretManager(TextBox box, int index = 0)
        {
            _textBox = box;
            _textBlock = null;

            CaretIndex = index;

            _timer = new DispatcherTimer(BlinkDelay, DispatcherPriority.Normal, BoxCaretTick, _textBox.Dispatcher);
            _timer.Start();
        }

        public CaretManager(TextBlock block, int index = 0)
        {
            _textBox = null;
            _textBlock = block;

            CaretIndex = index;

            _timer = new DispatcherTimer(BlinkDelay, DispatcherPriority.Normal, BlockCaretTick, _textBlock.Dispatcher);
            _timer.Start();
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer = null;
        }

        private void BoxCaretTick(object sender, EventArgs eventArgs)
        {
            var currentChar = _caretActive ? CaretChar : BlankChar;

            if (CaretIndex < 0)
                CaretIndex = 0;
            else if (CaretIndex > _textBox.Text.Length)
                CaretIndex = _textBox.Text.Length;

            if (CaretIndex != _lastCaretIndex)
                _caretActive = false;

            var nextChar = _caretActive ? BlankChar : CaretChar;

            if (_lastCaretIndex != -1 && _textBox.Text[_lastCaretIndex] == currentChar)
                _textBox.Text = _textBox.Text.Remove(CaretIndex, 1);

            _textBox.Text = _textBox.Text.Insert(CaretIndex, nextChar.ToString(CultureInfo.InvariantCulture));

            _caretActive = !_caretActive;
            _lastCaretIndex = CaretIndex;
        }

        private void BlockCaretTick(object sender, EventArgs eventArgs)
        {
            var currentChar = _caretActive ? CaretChar : BlankChar;

            if (CaretIndex < 0)
                CaretIndex = 0;
            else if (CaretIndex > _textBlock.Text.Length)
                CaretIndex = _textBlock.Text.Length;

            if (CaretIndex != _lastCaretIndex)
                _caretActive = false;

            var nextChar = _caretActive ? BlankChar : CaretChar;

            if (_lastCaretIndex != -1 && _textBlock.Text[_lastCaretIndex] == currentChar)
                _textBlock.Text = _textBlock.Text.Remove(CaretIndex, 1);

            _textBlock.Text = _textBlock.Text.Insert(CaretIndex, nextChar.ToString(CultureInfo.InvariantCulture));

            _caretActive = !_caretActive;
            _lastCaretIndex = CaretIndex;
        }
    }
}
