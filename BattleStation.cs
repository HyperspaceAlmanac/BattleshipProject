using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipProject
{
    // Class for handling player inputs for calling out locations
    enum TargetingStatus
    {
        Continue,
        InvalidTarget,
        LegalTarget
    }
    class BattleStation : PlayerControl
    {
        private int columnNumber;
        private int rowNumber;
        private Dictionary<ConsoleKey, int> numberSet;
        private Dictionary<ConsoleKey, int> letterSet;
        private FleetCommand otherDivision;
        public BattleStation(GameState game, FleetCommand otherDivision, int playerNum) : base(game, playerNum)
        {
            numberSet = new Dictionary<ConsoleKey, int>();
            letterSet = new Dictionary<ConsoleKey, int>();
            PopulateDictionaries();
            this.otherDivision = otherDivision;
        }

        public override void PerformAction()
        {
            game.DisplayOpponentBoardMode();
            PickSpot();
        }

        private void PickSpot()
        {
            columnNumber = rowNumber = -1;
            bool done = false;
            // For now result not actually used
            bool result = false;
            TargetingStatus status = TargetingStatus.Continue;
            while (!done)
            {
                Console.Clear();
                DisplayTargetingControls();
                DisplayCurrentStatus();
                if (status == TargetingStatus.Continue)
                {
                    Console.WriteLine("Please call out a location");
                }
                else if (status == TargetingStatus.InvalidTarget)
                {
                    Console.ForegroundColor = ERROR_COLOR;
                    Console.WriteLine("Invalid location, please call out another locatoin");
                    Console.ResetColor();
                }
                else
                {
                    // Should not reach here
                    Console.WriteLine("Time to debug why the game did not break out of loop");
                }

                if (rowNumber > -1 && columnNumber > -1)
                {
                    game.DisplayHighlight(rowNumber, columnNumber);
                }
                else
                {
                    game.DisplayAll();
                }
                otherDivision.DisplayStatus();
                status = HandleInput(ReadUserInput());
                if (status == TargetingStatus.LegalTarget)
                {
                    result = game.MakeMove(rowNumber, columnNumber);
                    done = true;
                }
            }
            Console.Clear();
            game.DisplayAction(rowNumber, columnNumber, "Player" + playerNum);
            game.DisplayAll();
            otherDivision.DisplayStatus();

        }
        private TargetingStatus HandleInput(ConsoleKey key)
        {
            if (key == ConsoleKey.Escape)
            {
                rowNumber = -1;
                columnNumber = -1;
                return TargetingStatus.Continue;
            }
            else if (key == ConsoleKey.Enter || key == ConsoleKey.Spacebar)
            {
                if (LegalTarget())
                {
                    return TargetingStatus.LegalTarget;
                }
                else
                {
                    return TargetingStatus.InvalidTarget;
                }
            }
            else if (HandleDirectionKeys(key))
            {
                return TargetingStatus.Continue;
            }
            else
            {
                NumberOrLetter(key);
            }
            return TargetingStatus.Continue;
        }

        private bool HandleDirectionKeys(ConsoleKey key) {
            if (key == ConsoleKey.LeftArrow)
            {
                if (columnNumber == -1)
                {
                    columnNumber = 0;
                }
                else
                {
                    columnNumber = Math.Max(0, columnNumber - 1);
                }
                return true;
            }
            if (key == ConsoleKey.RightArrow)
            {
                if (columnNumber == -1)
                {
                    columnNumber = 0;
                }
                else
                {
                    columnNumber = Math.Min(GameState.BOARDWIDTH - 1, columnNumber + 1);
                }
                return true;
            }
            if (key == ConsoleKey.UpArrow)
            {
                if (rowNumber == -1)
                {
                    rowNumber = 0;
                }
                else
                {
                    rowNumber = Math.Max(0, rowNumber - 1);
                }
                return true;
            }
            if (key == ConsoleKey.DownArrow)
            {
                if (rowNumber == -1)
                {
                    rowNumber = 0;
                }
                else
                {
                    rowNumber = Math.Min(GameState.BOARDHEIGHT - 1, rowNumber + 1);
                }
                return true;
            }
            return false;
        }

        // Don't acutally need to check if it is a number of letter
        private void NumberOrLetter(ConsoleKey key)
        {
            // Internally 0-19, from user's perspective it is 1-20
            if (numberSet.ContainsKey(key))
            {
                int val = numberSet[key];
                // If not set
                if (rowNumber == -1)
                {
                    // Set to 1 less
                    rowNumber = val - 1;
                }
                // if user entered 1 before
                else if (rowNumber == 0)
                {
                    // Now it is double digit number
                    rowNumber = 10 + val - 1;
                }
                // If user entered 2 before
                else if (rowNumber == 1)
                {
                    if (val == 0)
                    {
                        rowNumber = 20;
                    }
                    else // No other valid number that is 2x
                    {
                        rowNumber = val - 1;
                    }
                }
                else
                {
                    // For everything else, pretty much override
                    rowNumber = val - 1;
                }
                
            }
            else if (letterSet.ContainsKey(key))
            {
                columnNumber = letterSet[key];
            }
        }
        private bool LegalTarget()
        {
            if (rowNumber < 0 || rowNumber >= GameState.BOARDHEIGHT || columnNumber < 0 || columnNumber >= GameState.BOARDWIDTH)
            {
                return false;
            }
            return game.AvailableMove(rowNumber, columnNumber);
        }
        private void PopulateDictionaries()
        {
            numberSet.Add(ConsoleKey.NumPad0, 0);
            numberSet.Add(ConsoleKey.D0, 0);
            numberSet.Add(ConsoleKey.NumPad1, 1);
            numberSet.Add(ConsoleKey.D1, 1);
            numberSet.Add(ConsoleKey.NumPad2, 2);
            numberSet.Add(ConsoleKey.D2, 2);
            numberSet.Add(ConsoleKey.NumPad3, 3);
            numberSet.Add(ConsoleKey.D3, 3);
            numberSet.Add(ConsoleKey.NumPad4, 4);
            numberSet.Add(ConsoleKey.D4, 4);
            numberSet.Add(ConsoleKey.NumPad5, 5);
            numberSet.Add(ConsoleKey.D5, 5);
            numberSet.Add(ConsoleKey.NumPad6, 6);
            numberSet.Add(ConsoleKey.D6, 6);
            numberSet.Add(ConsoleKey.NumPad7, 7);
            numberSet.Add(ConsoleKey.D7, 7);
            numberSet.Add(ConsoleKey.NumPad8, 8);
            numberSet.Add(ConsoleKey.D8, 8);
            numberSet.Add(ConsoleKey.NumPad9, 9);
            numberSet.Add(ConsoleKey.D9, 9);

            //Not sure if it would be bad practice to iterate through ENUM as numbers of 65-90 for letters
            letterSet.Add(ConsoleKey.A, 0);
            letterSet.Add(ConsoleKey.B, 1);
            letterSet.Add(ConsoleKey.C, 2);
            letterSet.Add(ConsoleKey.D, 3);
            letterSet.Add(ConsoleKey.E, 4);
            letterSet.Add(ConsoleKey.F, 5);
            letterSet.Add(ConsoleKey.G, 6);
            letterSet.Add(ConsoleKey.H, 7);
            letterSet.Add(ConsoleKey.I, 8);
            letterSet.Add(ConsoleKey.J, 9);
            letterSet.Add(ConsoleKey.K, 10);
            letterSet.Add(ConsoleKey.L, 11);
            letterSet.Add(ConsoleKey.M, 12);
            letterSet.Add(ConsoleKey.N, 13);
            letterSet.Add(ConsoleKey.O, 14);
            letterSet.Add(ConsoleKey.P, 15);
            letterSet.Add(ConsoleKey.Q, 16);
            letterSet.Add(ConsoleKey.R, 17);
            letterSet.Add(ConsoleKey.S, 18);
            letterSet.Add(ConsoleKey.T, 19);
        }
        private void DisplayTargetingControls()
        {
            Console.WriteLine("Please use Letters, Numbers, and Arrow keys to select a location.");
            Console.WriteLine("Please press spacebar or Enter to confirm placement, and Escape to clear input");
        }

        private void DisplayCurrentStatus()
        {
            if (columnNumber == -1) {
                Console.Write("No column selected ");
            } else {
                if (columnNumber > -1 && columnNumber < 25)
                {
                    Console.Write("Column: " + GameState.NUMTOALPHABET[columnNumber] + " ");
                }
                else
                {
                    Console.WriteLine("Something went wrong with column ");
                }
            }
            if (rowNumber == -1)
            {
                Console.Write("No row selected ");
            }
            else
            {
                Console.Write("Row: " + (rowNumber + 1) + " " );
            }
            Console.WriteLine();
        }
    }
}
