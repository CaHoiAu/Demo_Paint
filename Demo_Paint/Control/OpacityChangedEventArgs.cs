using System;
using System.Drawing;

namespace ColorPicker
{
    public class OpacityChangedEventArgs : EventArgs
    {
        private int selectedAlpha;

        public OpacityChangedEventArgs(int selectedAlpha)
        {
            this.selectedAlpha = selectedAlpha;
        }

        public int SelectedAlpha { get => selectedAlpha; set => selectedAlpha = value; }
    }
}
