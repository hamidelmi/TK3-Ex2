using System;
using Microsoft.SPOT;
using Gadgeteer.Modules.GHIElectronics;

namespace GadgeteerApp6
{
    public interface IResultIndicator
    {
        void Reset();
        void Win();
        void Lost();
    }
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
