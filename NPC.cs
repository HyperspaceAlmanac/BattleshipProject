using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipProject
{
    class NPC : Player
    {
        protected readonly int RNGSEED = 100;
        protected readonly bool USE_RNG_SEED = false;
        protected Random rand;
        public NPC(int num) : base(num)
        {
            if (USE_RNG_SEED)
            {
                rand = new Random(RNGSEED);
            }
            else
            {
                rand = new Random();
            }
        }
        public override void TakeTurn(GameState state1, GameState state2)
        {
            EndTurn();
        }

        public override void PlaceShips(GameState playerState)
        {
            Console.WriteLine("The NPC has finished placing its ships");
            EndTurn();
        }

        private void EndTurn()
        {
            Console.WriteLine("The NPC has finished making its decision. Press any key to continue");
            Console.ReadKey();
        }
    }
}
