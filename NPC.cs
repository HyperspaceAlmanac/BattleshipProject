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
            Tuple<int, int>[] directions = new Tuple<int, int>[] {new Tuple<int, int>(0, 1), new Tuple<int, int>(0, -1),
                new Tuple<int, int>(1, 0), new Tuple<int, int>(-1, 0)};
            int x;
            int y;
            bool placed;
            Tuple<int, int> modifier;

            foreach (Tuple<string, int> pair in GameEngine.PIECES)
            {
                placed = false;
                while (!placed)
                {
                    placed = false;
                    x = rand.Next(GameState.BOARDHEIGHT);
                    y = rand.Next(GameState.BOARDWIDTH);
                    modifier = directions[rand.Next(directions.Length)];
                    
                    if (ValidPlacement(pair, x, y, modifier, playerState)) {
                        AddShip(x, y, pair, modifier, playerState);
                        placed = true;
                    }
                }
            }
            Console.WriteLine("The NPC has finished placing its ships");
            // DEBUG1
            playerState.DisplayAll();
            EndTurn();
        }

        private void EndTurn()
        {
            Console.WriteLine("The NPC has finished making its decision. Press any key to continue");
            Console.ReadKey();
        }
    }
}
