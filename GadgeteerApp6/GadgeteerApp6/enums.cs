using System;
using Microsoft.SPOT;

namespace GadgeteerApp6
{
    public enum PinColor { None, Red, Blue, Green, Purple, Yellow, Magenta }
    public enum GameStatus { SelectionMode, EnterSecret, Guess, Finished }
    public enum GameMode { P1, P2 };
    public enum JoystickMovement { None, Top, Bottom, Left, Right };
}
