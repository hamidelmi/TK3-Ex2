using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;
using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;
using System.Text;
using MasterMind;


namespace GadgeteerApp6
{
    public partial class Program
    {
        private Joystick.Position position;
        private Game game;

        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            /*******************************************************************************************
            Modules added in the Program.gadgeteer designer view are used by typing 
            their name followed by a period, e.g.  button.  or  camera.
            
            Many modules generate useful events. Type +=<tab><tab> to add a handler to an event, e.g.:
                button.ButtonPressed +=<tab><tab>
            
            If you want to do something periodically, use a GT.Timer and handle its Tick event, e.g.:
                GT.Timer timer = new GT.Timer(1000); // every second (1000ms)
                timer.Tick +=<tab><tab>
                timer.Start();
            *******************************************************************************************/


            // Use Debug.Print to show messages in Visual Studio's "Output" window during debugging.
            Debug.Print("Program Started");

            ledStrip.TurnAllLedsOff();

            //Configure the game object with our available gadgeeter module
            game = new Game(new GameDisplayTE35(displayTE35), new LEDStripResultIndicator(ledStrip));

            joystick.Calibrate();
            position = joystick.GetPosition();

            var joystickThread = new Thread(JoystickReadThread);
            joystickThread.Start();

            //In case user clicked on the button, let the game object know
            button.ButtonPressed += (s, ea) =>
            {
                game.ButtonPressed();
            };
        }


        /// <summary>
        /// Check the joystick movement and reports interesting movement to the game object
        /// It should be run on a different thread 
        /// </summary>
        public void JoystickReadThread()
        {
            while (true)
            {
                var newPos = joystick.GetPosition();
                if (newPos.Y > .7)
                    game.Joystick(JoystickMovement.Top);
                else if (newPos.Y < -.7)
                    game.Joystick(JoystickMovement.Bottom);
                else if (newPos.X > .7)
                    game.Joystick(JoystickMovement.Right);
                else if (newPos.X < -.7)
                    game.Joystick(JoystickMovement.Left);

                Thread.Sleep(100);
            }
        }
    }
}
