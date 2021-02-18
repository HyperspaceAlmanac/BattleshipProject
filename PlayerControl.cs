using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BattleshipProject
{
    enum IO_Status {
        OK,
        ERROR,
        CONTINUE,
        CANCEL
    }
    class PlayerControl
    {
        private GameState game;
        private int playerNum;
        private bool opponentBoard;
        public PlayerControl(GameState game, int playerNum, bool opponentBoard)
        {
            this.game = game;
            this.playerNum = playerNum;
            this.opponentBoard = opponentBoard;
        }

        // Utility function to delay reading input a bit when button is held down
        public static void PressKeyToContinue()
        {
            Console.ReadKey();
            Thread.Sleep(100);
        }

        public void GuessShipLocation()
        {
            game.DisplayOpponentBoard();
        }

        public void PlaceShips()
        {
            bool done;
            Tuple<int, int>[] shipCoordinates;
            game.DisplayOwnBoard();
            foreach (Tuple<string, int> piece in Ship.PIECES) {
                Console.Clear();
                Console.WriteLine($"Player{playerNum}: Please place the size {piece.Item2} {piece.Item1}");
                DisplayShipPlacementControls();
                // initialize ship coordinates
                shipCoordinates = new Tuple<int, int>[piece.Item2];
                // testing just a do something x time loop
                for (int i = 0; i < shipCoordinates.Length; i++)
                {
                    // Go right 1
                    shipCoordinates[i] = new Tuple<int, int>(i * Ship.DIRECTIONS[0].Item1, i * Ship.DIRECTIONS[0].Item2);
                }
                done = false;
                while (!done)
                {
                    game.DisplayPlaceShip(shipCoordinates);
                    // if tried to 
                    if (PlaceShipCommands(shipCoordinates) == IO_Status.OK) {
                        if (game.ValidPlacement(shipCoordinates))
                        {
                            game.AddShip(new Ship(piece.Item1, shipCoordinates));
                        }
                    }
                    done = true;
                }
            }
        }
        private IO_Status PlaceShipCommands(Tuple<int, int>[] coordinates)
        {
            char val = Console.ReadKey().KeyChar;
            Console.WriteLine(val);
            Console.ReadLine();
            Thread.Sleep(100);
            return IO_Status.CONTINUE;
        }

        private void DisplayShipPlacementControls()
        {
            Console.WriteLine("Please use Arrow keys to move the ship around.\n\"r\" key to rotate clockwise by 90 degrees, and spacebar to confirm placement");
        }

    }
}
