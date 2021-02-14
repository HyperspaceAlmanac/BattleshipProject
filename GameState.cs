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
    }
}
