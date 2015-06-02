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

namespace GadgeteerApp6
{

    class Pin : Control
    {

    }

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

            game = new Game(new GameDisplayTE35(displayTE35));

            joystick.Calibrate();
            position = joystick.GetPosition();

            joystick.JoystickPressed += joystick_JoystickPressed;

            var joystickThread = new Thread(JoystickReadThread);
            joystickThread.Start();

            button.ButtonPressed += (s, ea) =>
            {
                game.ButtonPressed();
            };
        }

        void joystick_JoystickPressed(Joystick sender, Joystick.ButtonState state)
        {
            //throw new NotImplementedException();
        }

        public void JoystickReadThread()
        {
            //while (true)
            //{
            //    var newPos = joystick.GetPosition();
            //    if (System.Math.Abs(newPos.Y - position.Y) > .5)
            //    {
            //        gameMode = gameMode == GameMode.P1 ? GameMode.P2 : GameMode.P1;
            //        SelectionScreen();
            //    }
            //    else
            //        position = newPos;

            //    Thread.Sleep(100);
            //}
        }


    }
}
