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
            sunkList = new List<int>();
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
                Nextmove(opponentState);
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
        // Hmmm, there are some really difficult game states for NPC to check
        // In the interest of time, I will not make the AI completely optimal
        // Some ship placement can be really tricky and will require AI to keep track of order of moves
        // For now I do not want to do that
        protected void Nextmove(GameState state)
        {
            List<Tuple<int, int>> soFar = new List<Tuple<int, int>>();
            // NPC will randomly select a hit, and use DFS to prioritize adjacent hits
            // Continue Adjacent path if able, otherwise, choose any legal move
            // Randomly choose to go thorugh direction list forward or backward

            // Check if AI will continue to search for ships or go completely random next turn
            FoundAllNearbyShips(state);
        }


        // NPC will 
        protected void DFS(GameState state, List<int[]> soFar, bool clockwise)
        {
        }
 
        // Try best moves to sink this ship

        protected void FoundAllNearbyShips(GameState state)
        {
            if (prevTotal + currentSearch.Count == state.SunkShipTotal())
            {
                searchNewShip = true;
                prevTotal += currentSearch.Count;
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
