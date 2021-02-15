using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipProject
{
    // Made the decision to have IO handling inside of player class
    // Moving most of these to HumanPlayer class
    abstract class Player
    {
        protected int playerNum;
        public Player(int num)
        {
            playerNum = num;
        }
        public abstract void TakeTurn(GameState playerState, GameState opponentState);

        public abstract void PlaceShips(GameState playerState);
    }
}
