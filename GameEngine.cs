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
    // Changed my mind, just use Console.ReadKey() and update display on moves made
    class GameEngine
    {
        private bool playerOneTurn;
        private Player p1;
        private Player p2;
        private GameState state1;
        private GameState state2;
        public static readonly Tuple<string, int>[] PIECES = new Tuple<string, int>[] {
            new Tuple<string, int>("destroyer", 2),
            new Tuple<string, int>("destroyer", 2),
            new Tuple<string, int>("submarine", 3),
            new Tuple<string, int>("battleship", 4),
            new Tuple<string, int>("aircraft carrier", 5)};

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

        public void RunGame()
        {
            bool gameOver = false;
            bool exitGame = false;
            while (!exitGame)
            {
                gameOver = false;
                DisplayIntro();
                SelectMode();
                state1 = new GameState(false);
                state2 = new GameState(true);
                p1.PlaceShips(state1);
                state2.TogglePlayer();
                p2.PlaceShips(state2);
                state2.TogglePlayer();
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
            bool done = false;
            while (!done)
            {
                Console.WriteLine("Please enter 1 for single player, or 2 for multiplayer");
                string val = Console.ReadLine();
                switch (val)
                {
                    case "1":
                        p1 = new HumanPlayer(1);
                        p2 = new NPC(2);
                        done = true;
                        break;
                    case "2":
                        p1 = new HumanPlayer(1);
                        p2 = new HumanPlayer(2);
                        done = true;
                        break;
                    // Debug only
                    case "3":
                        p1 = new NPC(1);
                        p2 = new NPC(2);
                        done = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private void DisplayWinner(bool playerOne)
        {
        }

        private bool RestartGame() {
            return false;
        }
    }
}
