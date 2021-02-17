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

        // Display screen when switching between players
        public abstract void SwitchPlayer();

        // Responsibility of method calling this to check correctness

        public bool AllShipsSunk()
        {
            return ownBoard.AllShipsSunk();
        }
    }
}
