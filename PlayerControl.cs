using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BattleshipProject
{
    abstract class PlayerControl
    {
        protected GameState game;
        protected int playerNum;
        protected static readonly ConsoleColor ERROR_COLOR = ConsoleColor.Red;
        protected static readonly ConsoleColor OK_COLOR = ConsoleColor.Green;
        public PlayerControl(GameState game, int playerNum)
        {
            this.game = game;
            this.playerNum = playerNum;
        }

        // Call this
        public abstract void PerformDuty();

        // Utility function to delay reading input a bit when button is held down
        public static void PressKeyToContinue()
        {
            Console.ReadKey();
            Thread.Sleep(100);
        }

        public void GuessShipLocation()
        {
            game.DisplayOpponentBoard();
        }

        protected ConsoleKey ReadUserInput()
        {
            // Could not quite get LastTimeInfo struct to work
            // Found this workaround online to only read input when button is not held down
            while (Console.KeyAvailable)
            {
                // don't intercept
                Console.ReadKey(false);
            }
            return Console.ReadKey().Key;
        }
    }
}
