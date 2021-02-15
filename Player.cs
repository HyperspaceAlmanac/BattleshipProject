using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipProject
{
    class Player
    {
        public Player()
        {
        }
        public virtual bool TakeTurn(GameState playerState, GameState opponentState)
        {

            return false;
        }

        public virtual bool PlaceShips(GameState playerState)
        {
            return true;
        }
    }
}
