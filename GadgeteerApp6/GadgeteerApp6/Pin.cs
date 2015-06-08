using System;
using Microsoft.SPOT;
using Gadgeteer.Modules.GHIElectronics;
using GT = Gadgeteer;

namespace GadgeteerApp6
{
    class Pin
    {
        private GT.Color _color;

        private int row, col;

        private bool selected;

        public Pin(GT.Color color, int col = 1, int row = 1, bool selected = false)
        {
            this._color = color;
            this.row = row;
            this.col = col;
            this.selected = selected;
        }
        
        public Pin(PinColor color, int col = 1, int row = 1, bool selected = false)
            : this(C2C(color), col, row, selected) { }

        public void Show(DisplayTE35 display, int radius, int xBase, int yBase)
        {
            display.SimpleGraphics.DisplayEllipse(selected ? GT.Color.Orange : _color,
                1, _color,
                xBase + col * radius * 2,
                yBase + row * radius * 2, radius - 1, radius - 1);
        }

        public void ShowPinOnLeft(DisplayTE35 display)
        {
            Show(display, 10, 20, 20);
        }

        public void ShowPinOnRight(DisplayTE35 display)
        {
            Show(display, 10, 220, 40);
        }

        public static GT.Color C2C(PinColor color)
        {
            switch (color)
            {
                case PinColor.Red:
                    return GT.Color.Red;
                case PinColor.Blue:
                    return GT.Color.Blue;
                case PinColor.Green:
                    return GT.Color.Green;
                case PinColor.Purple:
                    return GT.Color.Purple;
                case PinColor.Yellow:
                    return GT.Color.Yellow;
                case PinColor.Magenta:
                    return GT.Color.Magenta;
                default:
                    return GT.Color.White;
            }
        }
    }
}
