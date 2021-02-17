using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BattleshipProject
{
    class PlayerControl
    {
        private GameState game;
        private int playerNum;
        public PlayerControl(GameState game, int playerNum)
        {
            this.game = game;
            this.playerNum = playerNum;
        }

        // Utility function to delay reading input a bit when button is held down
        public static void PressKeyToContinue()
        {
            Console.ReadKey();
            Thread.Sleep(100);
        }

        public void PlaceShips()
        {
            bool done;
            game.DisplayOwnBoard();
            foreach (Tuple<string, int> piece in Ship.PIECES) {
                Console.Clear();
                Console.WriteLine($"Player{playerNum}: Please place the size {piece.Item2} {piece.Item1}");
                DisplayShipPlacementControls();
                done = false;
                while (!done)
                {

                    done = true;
                }
            }
        }

        private void DisplayShipPlacementControls()
        {
            Console.WriteLine("Please use Arrow keys to move the ship around.\n\"r\" key to rotate clockwise by 90 degrees, and spacebar to confirm placement");
        }

    }
}
