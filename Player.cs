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

        protected bool ValidPlacement(Tuple<string, int> pair, int startX, int startY, Tuple<int, int> modifier, GameState playerState)
        {
            int modifiedX, modifiedY;
            for (int i = 0; i < pair.Item2; i++)
            {
                modifiedX = startX + modifier.Item1 * i;
                modifiedY = startY + modifier.Item2 * i;
                if (modifiedX < 0 || modifiedX >= GameState.BOARDWIDTH ||
                    modifiedY < 0 || modifiedY >= GameState.BOARDHEIGHT ||
                    playerState.LocationOccupied(modifiedX, modifiedY))
                {
                    return false;
                }
            }
            return true;
        }

        // Responsibility of method calling this to check correctness
        protected void AddShip(int x, int y, Tuple<string, int> pair, Tuple<int, int> modifier, GameState playerState)
        {
            Tuple<int, int>[] coord = new Tuple<int, int>[pair.Item2];
            for (int i = 0; i < pair.Item2; i++)
            {
                coord[i] = new Tuple<int, int>(x + i * modifier.Item1, y + i * modifier.Item2);
            }
            playerState.AddShip(new Ship(pair.Item1, coord));
        }
    }
}
