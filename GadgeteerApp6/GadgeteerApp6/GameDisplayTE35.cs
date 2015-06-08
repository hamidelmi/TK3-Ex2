using System;
using Microsoft.SPOT;
using Gadgeteer.Modules.GHIElectronics;
using GT = Gadgeteer;

namespace GadgeteerApp6
{
    public interface IGameScreen
    {
        int Selected { get; }
        void SelectionScreen(Game game);
        void GuessScreen(Game game, JoystickMovement movement);

        void ResultScreen(Game game);

        void SecretScreen(Game game, JoystickMovement movement);
    }

    class GameDisplayTE35 : IGameScreen
    {
        private DisplayTE35 displayTE35;

        private Font font;

        private int selected = 0;
        public int Selected
        {
            get { return selected; }
        }

        public GameDisplayTE35(DisplayTE35 display)
        {
            this.displayTE35 = display;
            font = Resources.GetFont(Resources.FontResources.NinaB);

        }

        public void SelectionScreen(Game game)
        {
            displayTE35.SimpleGraphics.Clear();
            displayTE35.SimpleGraphics.DisplayText("Select Mode", font, GT.Color.White, 10, 10);

            displayTE35.SimpleGraphics.DisplayRectangle(
                game.gameMode == GameMode.P1 ? GT.Color.White : GT.Color.Red,
                1,
                GT.Color.Red,
                10, 50, 200, font.Height + 5
                );
            displayTE35.SimpleGraphics.DisplayText("1 Player", font, GT.Color.White, 12, 52);

            displayTE35.SimpleGraphics.DisplayRectangle(
                game.gameMode == GameMode.P2 ? GT.Color.White : GT.Color.Blue,
                1,
                GT.Color.Blue,
                10, 80, 200, font.Height + 5
                );
            displayTE35.SimpleGraphics.DisplayText("2 Player", font, GT.Color.White, 12, 82);
        }

        public void GuessScreen(Game game, JoystickMovement movement)
        {
            Pin[][] pins;

            pins = new Pin[game.guesses.Length][];
            int lastRow = game.numberOfGuess;

            switch (movement)
            {
                case JoystickMovement.Left:
                    selected = (selected - 1 == -1) ? Game.MAX_PIN - 1 : selected - 1;
                    break;
                case JoystickMovement.Right:
                    selected = (selected + 1) % Game.MAX_PIN;
                    break;
                case JoystickMovement.None:
                    selected = 0;
                    break;
                default:
                    break;
            }

            for (int i = 0; i <= lastRow; i++)
            {
                pins[i] = new Pin[Game.MAX_PIN];
                for (int j = 0; j < Game.MAX_PIN; j++)
                    pins[i][j] = new Pin(game.guesses[i][j], j + 1, i + 1, lastRow == i && selected == j);
            }
            displayTE35.SimpleGraphics.Clear();
            displayTE35.SimpleGraphics.DisplayText("Time to crack the code", font, GT.Color.White, 10, 10);
            for (int i = 0; i < pins.Length; i++)
            {
                if (pins[i] != null)
                    for (int j = 0; j < pins[i].Length; j++)
                    {
                        pins[i][j].ShowPinOnLeft(displayTE35);
                    }
            }

            for (int i = 0; i < game.results.Length; i++)
            {
                if (game.results[i] != null)
                    for (int j = 0; j < game.results[i].Length; j++)
                    {
                        new Pin(game.results[i][j], j, i).ShowPinOnRight(displayTE35);
                    }
            }
        }

        public void ResultScreen(Game game)
        {
            displayTE35.SimpleGraphics.DisplayText("The code was:", font, GT.Color.White, 115, 100);

            for (int j = 0; j < game.secret.Length; j++)
                new Pin(game.secret[j], j).Show(displayTE35, 10, 130, 120);
        }

        public void SecretScreen(Game game, JoystickMovement movement)
        {
            Pin[] pins;

            switch (movement)
            {
                case JoystickMovement.Left:
                    selected = (selected - 1 == -1) ? Game.MAX_PIN - 1 : selected - 1;
                    break;
                case JoystickMovement.Right:
                    selected = (selected + 1) % Game.MAX_PIN;
                    break;
                case JoystickMovement.None:
                    selected = 0;
                    break;
                default:
                    break;
            }

            pins = new Pin[Game.MAX_PIN];
            for (int j = 0; j < Game.MAX_PIN; j++)
                pins[j] = new Pin(game.secret[j], j + 1, 1, selected == j);

            displayTE35.SimpleGraphics.Clear();
            displayTE35.SimpleGraphics.DisplayText("Set secret", font, GT.Color.White, 10, 10);

            for (int j = 0; j < pins.Length; j++)
            {
                pins[j].ShowPinOnLeft(displayTE35);
            }
        }
    }
}
