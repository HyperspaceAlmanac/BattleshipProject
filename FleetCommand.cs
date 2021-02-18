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
            ConsoleKey key = Console.ReadKey().Key;
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

        private void RotateShip(Tuple<int, int>[] coordinates)
        {
        }
    }
}
