using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * Disclaimer: I do not own the rights to Battleship.
 * This is purely a project for educational purposes in learning and practicing programming
 **/
namespace BattleshipProject
{
    class HumanPlayer : Player
    {
        public HumanPlayer(int num) : base(num)
        {

        }
        public override void TakeTurn(GameState playerState, GameState opponentState)
        {
            ChangePlayer();
            bool done = false;
            while (!done)
            {
                done = true;
            }
            Console.WriteLine("end of Player's turn");
            Console.ReadKey();
        }

        public override void PlaceShips(GameState playerState)
        {
            Console.WriteLine("Player" + playerNum + "'s turn to place ships");
            // For now just automatically place one shp
        }

        protected void ChangePlayer()
        {
            Console.WriteLine($"Player{(playerNum == 1 ? 1 : 2)}: Please press any key to take your turn");
            Console.ReadKey();
            Console.Clear();
        }

        private void DisplayControls()
        {
            Console.WriteLine("Please use letters, numbers, and direction keys to select row and column");
            Console.WriteLine("Use Escape key to cancel, and space to confirm");
        }
    }
}
