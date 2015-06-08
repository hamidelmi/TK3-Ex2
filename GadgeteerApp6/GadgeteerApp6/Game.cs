using System;
using Microsoft.SPOT;
using Gadgeteer.Modules.GHIElectronics;
using System.Text;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;


namespace MasterMind
{
    /// <summary>
    /// Game core
    /// </summary>
    public class Game
    {
        public const int MAX_GUESS = 10, MAX_PIN = 4, MAX_COLOR = 6;

        private IGameScreen screen;
        private IResultIndicator resultIndicator;
        private GameStatus _gameStatus;
        private GameMode _gameMode;

        public PinColor[] secret;
        public PinColor[][] guesses;
        public GT.Color[][] results;
        public int numberOfGuess;

        public GameMode gameMode
        {
            get
            {
                return _gameMode;
            }
            private set
            {
                _gameMode = value;
                screen.SelectionScreen(this);
                if (_gameMode == GameMode.P1)
                    secret = GenerateRandomSecret();
                else
                {
                    secret = new PinColor[MAX_PIN];
                    for (int i = 0; i < secret.Length; i++)
                        secret[i] = PinColor.None;
                }
            }
        }

        private GameStatus gameStatus
        {
            get
            {
                return _gameStatus;
            }
            set
            {
                _gameStatus = value;
                switch (_gameStatus)
                {
                    case GameStatus.SelectionMode:
                        guesses = new PinColor[10][];
                        results = new GT.Color[10][];
                        numberOfGuess = 0;
                        gameMode = GameMode.P1;
                        screen.SelectionScreen(this);
                        resultIndicator.Reset();
                        break;
                    case GameStatus.EnterSecret:
                        screen.SecretScreen(this, JoystickMovement.None);
                        break;
                    case GameStatus.Guess:
                        AddNewGuess();
                        screen.GuessScreen(this, JoystickMovement.None);
                        break;
                    default:
                        break;
                }
            }
        }

        public Game(IGameScreen screen, IResultIndicator resultIndicator)
        {
            this.resultIndicator = resultIndicator;
            this.screen = screen;
            gameStatus = GameStatus.SelectionMode;
        }

        /// <summary>
        /// Add a new empty row to list of guesses
        /// </summary>
        private void AddNewGuess()
        {
            if (numberOfGuess > 0)
            {
                results[numberOfGuess - 1] = new GT.Color[MAX_PIN];
                for (int j = 0; j < MAX_PIN; j++)
                    results[numberOfGuess - 1][j] = GT.Color.White;
            }
            guesses[numberOfGuess] = new PinColor[MAX_PIN];
            for (int j = 0; j < MAX_PIN; j++)
                guesses[numberOfGuess][j] = PinColor.None;
        }

        /// <summary>
        /// Generate a random array of color out of available colors
        /// </summary>
        /// <returns></returns>
        private PinColor[] GenerateRandomSecret()
        {
            Random r = new Random();
            PinColor[] result = new PinColor[MAX_PIN];
            for (int i = 0; i < MAX_PIN; i++)
            {
                int v = r.Next(MAX_COLOR - 1) + 1;
                result[i] = (PinColor)v;
            }
            return result;
        }

        /// <summary>
        /// In case button pressed, this method should be called
        /// It will check the game status:
        ///     selection mode: 
        ///         1 Player: show guess screen
        ///         2 Player: show sceret screen
        ///     Secret screen: Show guess screen
        ///     Guess screen: check the selection set and show the partial result
        ///                     in case it was the last selection, show the final result
        ///     Resultscreen: Show the menu screen
        /// </summary>
        public void ButtonPressed()
        {
            switch (gameStatus)
            {
                case GameStatus.SelectionMode:
                    if (gameMode == GameMode.P1)
                        gameStatus = GameStatus.Guess;
                    else
                        gameStatus = GameStatus.EnterSecret;
                    break;
                case GameStatus.EnterSecret:
                    gameStatus = GameStatus.Guess;
                    break;
                case GameStatus.Guess:
                    if (numberOfGuess < MAX_GUESS - 1)
                    {
                        numberOfGuess++;
                        AddNewGuess();
                        if (CalculateResult())
                        {
                            screen.ResultScreen(this);
                            gameStatus = GameStatus.Finished;
                            resultIndicator.Win();
                        }
                        else
                            screen.GuessScreen(this, JoystickMovement.None);
                    }
                    else
                    {
                        screen.ResultScreen(this);
                        gameStatus = GameStatus.Finished;
                        resultIndicator.Lost();
                    }
                    break;
                case GameStatus.Finished:
                    gameStatus = GameStatus.SelectionMode;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Check the result and return true if all the pins' color and position are correct
        /// It also save the partial result in the class
        /// </summary>
        /// <returns></returns>
        private bool CalculateResult()
        {
            int correct = 0;
            for (int i = 0; i < MAX_PIN; i++)
            {
                //Correct Color,Correct position=Green
                if (guesses[numberOfGuess - 1][i] == secret[i])
                {
                    results[numberOfGuess - 1][i] = GT.Color.Green;
                    correct++;
                }
                else
                {
                    //Totally wrong
                    results[numberOfGuess - 1][i] = GT.Color.Gray;

                    //Correct Color, wrong position=Blue
                    for (int j = 0; j < MAX_PIN; j++)
                        if (guesses[numberOfGuess - 1][i] == secret[j])
                        {
                            results[numberOfGuess - 1][i] = GT.Color.Blue;
                            break;
                        }
                }
            }

            return correct == MAX_PIN;
        }

        /// <summary>
        /// Response to joystick movement based on game status:
        ///     Selection Mode: 
        ///         Top: Active 1 player mode
        ///         Buttom : Active 2 player mode
        ///     Enter Secret:
        ///         Top,Buttom: Change the color of selcted pin of the secret
        ///     Guess screen:
        ///         Top,Buttom: Change the color of selcted pin of the current row of guesses
        /// </summary>
        /// <param name="movement"></param>
        public void Joystick(JoystickMovement movement)
        {
            switch (gameStatus)
            {
                case GameStatus.SelectionMode:
                    switch (movement)
                    {
                        case JoystickMovement.Top:
                            gameMode = GameMode.P1;
                            break;
                        case JoystickMovement.Bottom:
                            gameMode = GameMode.P2;
                            break;
                    }
                    break;
                case GameStatus.EnterSecret:
                    {
                        var selected = screen.Selected;
                        switch (movement)
                        {
                            case JoystickMovement.Top:
                                secret[selected] = (PinColor)(((int)secret[selected] + 1) % MAX_COLOR);
                                break;
                            case JoystickMovement.Bottom:
                                secret[selected] = ((int)secret[selected] - 1 == -1) ? PinColor.Magenta : secret[selected] - 1;
                                break;
                            default:
                                break;
                        }
                        screen.SecretScreen(this, movement);
                    }
                    break;
                case GameStatus.Guess:
                    {
                        var selected = screen.Selected;
                        switch (movement)
                        {
                            case JoystickMovement.Top:
                                guesses[numberOfGuess][selected] = (PinColor)(((int)guesses[numberOfGuess][selected] + 1) % MAX_COLOR);
                                break;
                            case JoystickMovement.Bottom:
                                guesses[numberOfGuess][selected] = ((int)guesses[numberOfGuess][selected] - 1 == -1) ? PinColor.Magenta : guesses[numberOfGuess][selected] - 1;
                                break;
                            default:
                                break;
                        }
                        screen.GuessScreen(this, movement);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
