using System;
using Microsoft.SPOT;
using Gadgeteer.Modules.GHIElectronics;

namespace MasterMind
{
    /// <summary>
    /// Result indicator should be able to reset its current status, 
    /// and indicate win or lost of the game to the user
    /// </summary>
    public interface IResultIndicator
    {
        void Reset();
        void Win();
        void Lost();
    }
    /// <summary>
    /// LEDStrip implementation of IResultIndicator
    /// It shows two red lights in case of lost
    /// and two green lights in case of win
    /// and turn off all of the lights when it asked to be reset
    /// </summary>
    public class LEDStripResultIndicator : IResultIndicator
    {
        LEDStrip ledstrip;
        public LEDStripResultIndicator(LEDStrip ledstrip)
        {
            this.ledstrip = ledstrip;
        }
        public void Reset() { ledstrip.TurnAllLedsOff(); }
        public void Win()
        {
            ledstrip.TurnLedOn(0);
            ledstrip.TurnLedOn(1);
        }
        public void Lost()
        {
            ledstrip.TurnLedOn(5);
            ledstrip.TurnLedOn(6);
        }
    }
}
