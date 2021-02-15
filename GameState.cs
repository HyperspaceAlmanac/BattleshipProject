using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipProject
{
    // Used to keep track of current state of the game.
    // Passed around to be updated by other classes
    enum Location {
        Empty,
        Hit,
        Miss
    }
    class GameState
    {
        // String representation of hits, misses, and ships
        static readonly int BOARDWIDTH = 10;
        static readonly int BOARDHEIGHT = 10;
        private static readonly string TOPBORDER = "  A B C D E F G H I J";
        Location[,] boardState;
        List<Ship> ships;
        int numShots;

        public GameState()
        {
            boardState = new Location[BOARDWIDTH, BOARDHEIGHT];
            for (int i = 0; i < BOARDHEIGHT; i++)
            {
                for (int j = 0; j < BOARDWIDTH; j++)
                {
                    boardState[i, j] = Location.Empty;
                }
            }
            numShots = 0;
            ships = new List<Ship>();
        }

        // check if game can continue
        public bool MovesAvailable()
        {
            return numShots < 100;
        }

        // check if this player has won (sunk all of opponent's ships)
        public bool AllShipsSunk()
        {
            foreach (Ship s in ships)
            {
                if (s.IsAlive())
                {
                    return false;
                }
            }
            return true;
        }

        public bool MakeMove(int x, int y)
        {
            // Check that coordinates are correct
            if (x > -1 && x < BOARDHEIGHT && y > -1 && y < BOARDWIDTH)
            {
                if (boardState[x, y] == Location.Empty)
                {
                    Location current = Location.Miss;
                    foreach (Ship s in ships)
                    {
                        if (s.hitShip(x, y))
                        {
                            current = Location.Hit;
                            break;
                        }
                    }
                    boardState[x, y] = current;
                    numShots += 1;
                    return true;
                }
            }
            return false;
        }

        // use this to experiment with displayign color to console
        public static void ColorDisplayTest()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Cyan");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Green");
        }

        private void DisplayLocation(Location location)
        {
            if (location == Location.Empty)
            {
                Console.Write("  ");
            }
            else if (location == Location.Hit)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" x");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(" o");
            }
            Console.ResetColor();
                
        }

        public void DisplayBoard()
        {
            // Display letters at top
            Console.WriteLine(" " + TOPBORDER);
            for (int i = 0; i < boardState.GetLength(0); i++)
            {

                Console.Write((i < 9 ? " " : "") + (i + 1));
                  
                for (int j = 0; j < boardState.GetLength(1); j++)
                {
                    DisplayLocation(boardState[i, j]);
                }
                Console.WriteLine();
            }
            
        }
    }
}
