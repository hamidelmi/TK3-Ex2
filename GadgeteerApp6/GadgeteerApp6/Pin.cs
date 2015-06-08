using System;
using Microsoft.SPOT;
using Gadgeteer.Modules.GHIElectronics;
using GT = Gadgeteer;

namespace MasterMind
{
    /// <summary>
    /// Represent a pin on a DisplayTE35
    /// </summary>
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

        /// <summary>
        /// Show the pin on a arbitary position on the screen.
        /// </summary>
        /// <param name="display"></param>
        /// <param name="radius"></param>
        /// <param name="xBase"></param>
        /// <param name="yBase"></param>
        public void Show(DisplayTE35 display, int radius, int xBase, int yBase)
        {
            display.SimpleGraphics.DisplayEllipse(selected ? GT.Color.Orange : _color,
                1, _color,
                xBase + col * radius * 2,
                yBase + row * radius * 2, radius - 1, radius - 1);
        }

        /// <summary>
        /// Show the pin on a fixed position on the left side of screen 
        /// </summary>
        /// <param name="display"></param>
        public void ShowPinOnLeft(DisplayTE35 display)
        {
            Show(display, 10, 20, 20);
        }

        /// <summary>
        /// Show the pin on a fixed position on the right side of screen 
        /// </summary>
        /// <param name="display"></param>
        public void ShowPinOnRight(DisplayTE35 display)
        {
            Show(display, 10, 220, 40);
        }

        /// <summary>
        /// Convert PinColor enum to actual color on the screen
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
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
