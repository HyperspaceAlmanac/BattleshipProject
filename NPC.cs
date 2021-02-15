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
        protected HashSet<Tuple<int, int>> currentSearch;
        protected int turnNumber;
        protected bool searchNewShip;
        protected int prevTotal;
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
            currentSearch = new HashSet<Tuple<int, int>>();
            turnNumber = 0;
            searchNewShip = true;
        }
        public override void TakeTurn(GameState ownState, GameState opponentState)
        {
            turnNumber += 1;
            // Need differnt logic for first two turns before NPC has seen 2 ships
            if (searchNewShip)
            {
                CompletelyRandom(opponentState);
            } else {
                // Based on if prev is hit or miss, choose new action
                // If prev missed, then try another legal adjacent move
                // If prev hit, and there is hit before it, continue this trajectory

            }
            // For now, thinking of keeping track of previous 2 hits
            EndTurn();
        }

        protected void CompletelyRandom(GameState opponentState)
        {
            bool done = false;
            int x, y;
            bool result;
            while (!done)
            {
                x = rand.Next(GameState.BOARDHEIGHT);
                y = rand.Next(GameState.BOARDWIDTH);
                if (opponentState.AvailableMove(x, y))
                {
                    result = opponentState.MakeMove(x, y);
                    if (result)
                    {
                        currentSearch = new HashSet<Tuple<int, int>>();
                        searchNewShip = true;
                    }
                    done = true;
                }
            }
        }
        protected void PrevHit(GameState state)
        {

        }
        // 
        protected void PrevMiss(GameState state)
        {
        }
        // Try best moves to sink this ship
        protected void TwoHits(GameState state)
        {
        }
        protected int currentNumHits(GameState state) {
            int total = 0;
            foreach (Tuple<int, int> tup in currentSearch)
            {
                if (state.LocationHit(tup.Item1, tup.Item2))
                {
                    total += 1;
                }
            }
            return total;
        }
        protected void FoundAllNearbyShips(GameState state)
        {
            int current = currentNumHits(state);
            if (prevTotal + current == state.SunkShipTotal())
            {
                searchNewShip = true;
                prevTotal += current;
            }
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
            //playerState.DisplayAll();
            EndTurn();
        }

        private void EndTurn()
        {
            Console.WriteLine("The NPC has finished making its decision. Press any key to continue");
            Console.ReadKey();
        }
    }
}
