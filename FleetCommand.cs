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
            PLACED_OK,
            PLACEMENT_ERROR,
            CONTINUE
        }
        public FleetCommand(GameState game, int playerNum) : base(game, playerNum)
        {
        }

        public override void PerformDuty()
        {
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
                    Console.WriteLine($"Player{playerNum}: Please place the size {piece.Item2} {piece.Item1}");
                    DisplayShipPlacementControls();
                    // Display status related to controls
                    switch (status)
                    {
                        case PLACEMENT_STATUS.CONTINUE:
                            Console.WriteLine("Waiting to place ship");
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
                    if (status == PLACEMENT_STATUS.PLACED_OK)
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
                    break;
                case ConsoleKey.RightArrow:
                    break;
                case ConsoleKey.UpArrow:
                    break;
                case ConsoleKey.DownArrow:
                    break;
                case ConsoleKey.Spacebar:
                    break;
                case ConsoleKey.R:
                    break;
                default:
                    break;
            }

            Thread.Sleep(100);
            return PLACEMENT_STATUS.CONTINUE;
        }
    }
}
