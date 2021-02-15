using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipProject
{
    // Changed my mind, just use Console.ReadKey() and update display on moves made
    class GameEngine
    {
        private bool playerOneTurn;
        private Player p1;
        private Player p2;

        public GameEngine()
        {
            playerOneTurn = true;
            
        }

        private void DisplayIntro()
        {
            Console.WriteLine("Welcome to Battleship!");
        }

        private void DisplayControls()
        {
            Console.WriteLine("Please use letters, numbers, and direction keys to select row and column");
            Console.WriteLine("Use Escape key to cancel, and space to confirm");
        }

        private void ChangePlaer()
        {
            Console.WriteLine($"Player{(playerOneTurn ? 1 : 2)}: Please press any key to take your turn");
            Console.ReadKey();
        }
    }
}
