using System;
using Microsoft.SPOT;

namespace MasterMind
{
    /// <summary>
    /// Set of possible colors for a pin
    /// </summary>
    public enum PinColor { None, Red, Blue, Green, Purple, Yellow, Magenta }
    
    /// <summary>
    /// Difference statuses of a game
    /// </summary>
    public enum GameStatus { SelectionMode, EnterSecret, Guess, Finished }
    
    /// <summary>
    /// Mode of the game: 1 player or 2 player
    /// </summary>
    public enum GameMode { P1, P2 };
    
    /// <summary>
    /// Important movement of joystick module
    /// </summary>
    public enum JoystickMovement { None, Top, Bottom, Left, Right };
}
