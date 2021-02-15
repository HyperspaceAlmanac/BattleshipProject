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
        private GameState state1;
        private GameState state2;

        public GameEngine()
        {
            playerOneTurn = true;
            RunGame();
            
        }

        private void DisplayIntro()
        {
            Console.WriteLine("===============================");
            Console.WriteLine("Welcome to Battleship!");
            Console.WriteLine("===============================");
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
        private void RunGame()
        {
            bool gameOver = false;
            bool exitGame = false;
            while (!exitGame)
            {
                gameOver = false;
                DisplayIntro();
                SelectMode();
                p1.PlaceShips(state1);
                p2.PlaceShips(state2);
                while (!gameOver)
                {
                    if (playerOneTurn)
                    {
                        p1.TakeTurn(state1, state2);
                    }
                    else
                    {
                        p2.TakeTurn(state2, state1);
                    }
                    if (state1.AllShipsSunk()) {
                        DisplayWinner(true);
                        gameOver = true;
                    } else if (state2.AllShipsSunk())
                    {
                        DisplayWinner(false);
                        gameOver = true;
                    }
                    state1.TogglePlayer();
                    state2.TogglePlayer();
                }
                exitGame = RestartGame();
            }
        }

        private void SelectMode()
        {
        }

        private void DisplayWinner(bool playerOne)
        {
        }

        private bool RestartGame() {
            return false;
        }
    }
}
