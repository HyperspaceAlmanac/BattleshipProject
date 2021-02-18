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
    class HumanPlayer : Player
    {
        private FleetCommand fleet;
        private BattleStation artilleryStation;
        public HumanPlayer(int num, GameState ownBoard, GameState opponentBoard) : base(num, ownBoard, opponentBoard)
        {
            fleet = new FleetCommand(ownBoard, num);
            artilleryStation = new BattleStation(opponentBoard, fleet, num);
        }
        public override void TakeTurn()
        {
            ownBoard.DisplayOwnBoard();
            opponentBoard.DisplayOpponentBoard();
            artilleryStation.PerformDuty();
            Console.WriteLine("end of Player's turn");
            Console.ReadKey();
        }

        public override void PlaceShips()
        {
            Console.WriteLine("Player" + playerNum + "'s turn to place ships");
            fleet.PerformDuty();
            // For now just automatically place one shp
        }

        protected void ChangePlayer()
        {
            Console.WriteLine($"Player{(playerNum == 1 ? 1 : 2)}: Please press any key to take your turn");
            Console.ReadKey();
            Console.Clear();
        }

        private void DisplayControls()
        {
            Console.WriteLine("Please use letters, numbers, and direction keys to select row and column");
            Console.WriteLine("Use Escape key to cancel, and space to confirm");
        }

        public override void SwitchPlayer()
        {
            int otherPlayer = playerNum == 1 ? 1 : 2;
            Console.Clear();
            Console.WriteLine("Player" + playerNum + "'s turn has finished.");
            Console.WriteLine("Player" + otherPlayer + ": please press any key to continue");
            PlayerControl.PressKeyToContinue();
        }
    }
}
