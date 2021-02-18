using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BattleshipProject
{
    class FleetCommand : PlayerControl
    {
        enum PLACEMENT_STATUS
        {
            CAN_PLACE,
            PLACEMENT_ERROR,
            CONTINUE
        }
        public FleetCommand(GameState game, int playerNum) : base(game, playerNum)
        {
        }

        public override void PerformDuty()
        {
            game.DisplayOwnBoard();
            PlaceShips();
        }

        private void PlaceShips()
        {
            bool done;
            Tuple<int, int>[] shipCoordinates;
            PLACEMENT_STATUS status = PLACEMENT_STATUS.CONTINUE;
            foreach (Tuple<string, int> piece in Ship.PIECES)
            {
                // initialize ship coordinates
                shipCoordinates = new Tuple<int, int>[piece.Item2];
                // testing just a do something x time loop
                for (int i = 0; i < shipCoordinates.Length; i++)
                {
                    // Go right 1
                    shipCoordinates[i] = new Tuple<int, int>(i * Ship.DIRECTIONS[0].Item1, i * Ship.DIRECTIONS[0].Item2);
                }
                done = false;
                status = PLACEMENT_STATUS.CONTINUE;
                while (!done)
                {
                    Console.Clear();
                    Console.WriteLine($"Player{playerNum}'s turn to deploy their fleet");
                    DisplayShipPlacementControls();
                    // Display status related to controls
                    switch (status)
                    {
                        case PLACEMENT_STATUS.CONTINUE:
                            Console.WriteLine($"Please place the size {piece.Item2} {piece.Item1}");
                            break;
                        case PLACEMENT_STATUS.PLACEMENT_ERROR:
                            Console.ForegroundColor = ERROR_COLOR;
                            Console.WriteLine("A ship is already there! Please choose another location");
                            Console.ResetColor();
                            break;
                        default:
                            // in theory should never have PLACEMENT_STATUS.PLACED_OK
                            Console.WriteLine("Should not reach here");
                            break;
                    }
                    game.DisplayPlaceShip(shipCoordinates);
                    // if tried to 
                    status = PlaceShipCommands(shipCoordinates);
                    if (status == PLACEMENT_STATUS.CAN_PLACE)
                    {
                        if (game.ValidPlacement(shipCoordinates))
                        {
                            game.AddShip(new Ship(piece.Item1, shipCoordinates));
                            done = true;
                        }
                    }
                }
                Console.Clear();
                Console.WriteLine("All ships successfully placed");
                game.DisplayAll();
            }
        }
        private PLACEMENT_STATUS PlaceShipCommands(Tuple<int, int>[] coordinates)
        {
            ConsoleKey key = ReadUserInput();
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    TryToShift(coordinates, 0, -1);
                    break;
                case ConsoleKey.RightArrow:
                    TryToShift(coordinates, 0, 1);
                    break;
                case ConsoleKey.UpArrow:
                    TryToShift(coordinates, -1, 0);
                    break;
                case ConsoleKey.DownArrow:
                    TryToShift(coordinates, 1, 0);
                    break;
                case ConsoleKey.Spacebar:
                case ConsoleKey.Enter:
                    if (game.ValidPlacement(coordinates))
                    {
                        return PLACEMENT_STATUS.CAN_PLACE;
                    }
                    else
                    {
                        return PLACEMENT_STATUS.PLACEMENT_ERROR;
                    }
                case ConsoleKey.R:
                    RotateShip(coordinates);
                    break;
                default:
                    break;
            }

            return PLACEMENT_STATUS.CONTINUE;
        }

        // General function to move ship by 1 space if possible
        private void TryToShift(Tuple<int, int>[] coordinates, int xShift, int yShift)
        {
            bool canMove = true;
            int x, y;
            foreach (Tuple<int, int> location in coordinates)
            {
                x = location.Item1 + xShift;
                y = location.Item2 + yShift;
                if (x < 0 || x >= GameState.BOARDHEIGHT || y < 0 || y >= GameState.BOARDWIDTH)
                {
                    canMove = false;
                    break;
                }
            }

            if (canMove)
            {
                for (int i = 0; i < coordinates.Length; i++)
                {
                    coordinates[i] = new Tuple<int, int>(coordinates[i].Item1 + xShift, coordinates[i].Item2 + yShift);
                }
            }
        }

        // Rotate in place
        // To have the rotations... feel better, there is a difference between 
        // Ship facing up and down, left, and right. Pivot will always be first location
        private void RotateShip(Tuple<int, int>[] coordinates)
        {
            // Rotate first, move into boundary later
            int startX = coordinates[0].Item1;
            int startY = coordinates[0].Item2;
            // These are the initial offsets. They will be upadted to new offset values
            int diffX = coordinates[1].Item1 - coordinates[0].Item1;
            int diffY = coordinates[1].Item2 - coordinates[0].Item2;
            // Just do if else for the 4 directions
            // If left or right
            if (diffX == 0)
            {
                // Right -> DOWN
                if (diffY == 1)
                {
                    diffX = 1;
                    diffY = 0;
                    // Maybe need to move up a few rows
                    // board of size 10, ship size of 3 = occupying rows 7, 8, 9
                    startX = Math.Min(GameState.BOARDHEIGHT - coordinates.Length, startX);
                }
                else // Left -> UP
                {
                    diffX = -1;
                    diffY = 0;
                    // May need to move down a few rows
                    // Ship of size 3, would need to occupy 0, 1, 2
                    startX = Math.Max(coordinates.Length - 1, startX);
                }
            }
            else if (diffX == 1)
            {
                // Down relative to view of board -> Left
                diffX = 0;
                diffY = -1;
                // May need to move the column to the right
                startY = Math.Max(coordinates.Length - 1, startY);

            }
            else // has to be (-1, 0)
            {
                // Up relative to view of board -> Right
                diffX = 0;
                diffY = 1;
                // May need to move column to the left
                startY = Math.Min(GameState.BOARDWIDTH - coordinates.Length, startY);
            }
            for (int i = 0; i < coordinates.Length; i++)
            {
                coordinates[i] = new Tuple<int, int>(startX + diffX * i, startY + diffY * i);
            }
        }
        private void DisplayShipPlacementControls()
        {
            Console.WriteLine("Please use Arrow keys to move the ship around and \"R\" to clockwise by 90 degrees.");
            Console.WriteLine("Please press spacebar or Enter to confirm placement");
        }
    }
}
