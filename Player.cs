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
    // Made the decision to have IO handling inside of player class
    // Moving most of these to HumanPlayer class
    abstract class Player
    {
        protected int playerNum;
        protected GameState ownBoard;
        protected GameState opponentBoard;
        public Player(int num, GameState ownBoard, GameState opponentBoard)
        {
            this.ownBoard = ownBoard;
            this.opponentBoard = opponentBoard;
            playerNum = num;
        }
        public abstract void TakeTurn();

        public abstract void PlaceShips();

        protected bool ValidPlacement(Tuple<string, int> pair, int startX, int startY, Tuple<int, int> modifier)
        {
            int modifiedX, modifiedY;
            for (int i = 0; i < pair.Item2; i++)
            {
                modifiedX = startX + modifier.Item1 * i;
                modifiedY = startY + modifier.Item2 * i;
                if (modifiedX < 0 || modifiedX >= GameState.BOARDWIDTH ||
                    modifiedY < 0 || modifiedY >= GameState.BOARDHEIGHT ||
                    ownBoard.LocationOccupied(modifiedX, modifiedY))
                {
                    return false;
                }
            }
            return true;
        }

        // Responsibility of method calling this to check correctness
        protected void AddShip(int x, int y, Tuple<string, int> pair, Tuple<int, int> modifier)
        {
            Tuple<int, int>[] coord = new Tuple<int, int>[pair.Item2];
            for (int i = 0; i < pair.Item2; i++)
            {
                coord[i] = new Tuple<int, int>(x + i * modifier.Item1, y + i * modifier.Item2);
            }
            ownBoard.AddShip(new Ship(pair.Item1, coord));
        }

        public bool AllShipsSunk()
        {
            return ownBoard.AllShipsSunk();
        }
    }
}
