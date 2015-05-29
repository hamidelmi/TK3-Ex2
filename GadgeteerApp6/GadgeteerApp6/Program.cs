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
    public partial class Program
    {
        private enum GameStatus { SelectionMode, EnterSecret, Guess }
        private GameStatus gameStatus;
        private const int MAX_GUESS = 10;
        private int numberOfGuess;
        private enum GameMode { P1, P2 };
        private PinColor[] secret;
        private enum PinColor { Red, Blue, Green, Purple, Yellow, Magenta }
        private Font f;
        private GameMode gameMode;

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

            gameStatus = GameStatus.SelectionMode;
            // Show mode selection screen
            SelectionScreen();

            button.ButtonPressed += (s, ea) =>
            {
                switch (GameStatus)
                {
                    case GameStatus.SelectionMode:
                        if (gameMode == GameMode.P1)
                            GameStatus = GameStatus.Guess;
                        else
                            GameStatus = GameStatus.EnterSecret;
                        break;
                    case GameStatus.EnterSecret:
                        GameStatus = GameStatus.Guess;
                        break;
                    case GameStatus.Guess:
                        if (numberOfGuess < MAX_GUESS)
                            numberOfGuess++;
                        break;
                    default:
                        break;
                }
            };
            f = Resources.GetFont(Resources.FontResources.NinaB);
        }

        private void SelectionScreen()
        {
            Window window = displayTE35.WPFWindow;
            window.Background = new SolidColorBrush(Color.Black);
            Panel p = new Panel();
            window.Child = p;

            StackPanel controls = new StackPanel(Orientation.Vertical);
            controls.SetMargin(10);
            p.Children.Add(controls);

            String _1_player = "1 Player";
            String _2_player = "2 Player";

            Border _1_playerBorder = GetBorder(GT.Color.Red);
            Border _2_playerBorder = GetBorder(GT.Color.Blue);

            _1_playerBorder.TouchDown += (s, e) => { gameMode = GameMode.P1; secret = GenerateRandomSecret(); };
            _2_playerBorder.TouchDown += (s, e) => { gameMode = GameMode.P2; };

            controls.Children.Add(GetStackPanel(_1_player, _1_playerBorder));
            controls.Children.Add(GetStackPanel(_2_player, _2_playerBorder));
        }

        private StackPanel GetStackPanel(String text, Border border)
        {
            StackPanel sp = new StackPanel(Orientation.Horizontal);
            sp.SetMargin(10);
            sp.HorizontalAlignment = HorizontalAlignment.Stretch;
            sp.VerticalAlignment = VerticalAlignment.Top;
            sp.Children.Add(new Text(f, text) { ForeColor = Color.White, Width = 60, Height = border.Height, VerticalAlignment = VerticalAlignment.Center });
            sp.Children.Add(border);
            return sp;
        }

        private Border GetBorder(Color color, Color foreground = Color.White)
        {
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(color);
            border.Height = f.Height + 5;
            border.Width = 300;
            return border;
        }

        private PinColor[] GenerateRandomSecret()
        {
            StringBuilder sb = new StringBuilder();
            Random r = new Random();
            PinColor[] result = new PinColor[4];
            for (int i = 0; i < 4; i++)
            {
                int v = r.Next(6);
                result[i] = (PinColor)v;
            }
            return result;
        }
    }
}
