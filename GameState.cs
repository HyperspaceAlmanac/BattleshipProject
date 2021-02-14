using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipProject
{
    // Used to keep track of current state of the game.
    // Passed around to be updated by other classes
    class GameState
    {
        // String representation of hits, misses, and ships
        string boardState;
        List<Ship> ships;
        int numShots;

        // check if game can continue
        public bool MovesAvailable()
        {
            return numShots < 100;
        }

        // check if this player has won (sunk all of opponent's ships)
        public bool AllShipsSunk()
        {
            bool result = true;
            foreach (Ship s in ships)
            {
                if (s.IsAlive())
                {
                    return false;
                }
            }
            return true;
        }

        public bool MakeMove()
        {
            return true;
        }
    }
}
