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
        public static readonly Tuple<int, int>[] DIRECTIONS = new Tuple<int, int>[] {
            new Tuple<int, int>(0, 1), new Tuple<int, int>(0, -1),
            new Tuple<int, int>(1, 0), new Tuple<int, int>(-1, 0)};
        protected readonly int RNGSEED = 100;
        protected readonly bool USE_RNG_SEED = false;
        protected Random rand;
        protected HashSet<Tuple<int, int>> currentSearch;
        protected int turnNumber;
        protected bool searchNewShip;
        protected int prevTotal;
        public NPC(int num, GameState ownBoard, GameState opponentBoard) : base(num, ownBoard, opponentBoard)
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
        public override void TakeTurn()
        {
            Console.Clear();
            //opponentBoard.DisplayOpponentBoard();
            turnNumber += 1;
            // Need differnt logic for first two turns before NPC has seen 2 ships
            if (searchNewShip)
            {
                CompletelyRandom();
            } else {
                // Based on if prev is hit or miss, choose new action
                // If prev missed, then try another legal adjacent move
                // If prev hit, and there is hit before it, continue this trajectory
                Nextmove();
            }
            // For now, thinking of keeping track of previous 2 hits

            EndTurn();
        }

        protected void CompletelyRandom()
        {
            bool done = false;
            int x, y;
            bool result;
            while (!done)
            {
                x = rand.Next(GameState.BOARDHEIGHT);
                y = rand.Next(GameState.BOARDWIDTH);
                if (opponentBoard.AvailableMove(x, y))
                {
                    result = opponentBoard.MakeMove(x, y);
                    opponentBoard.DisplayAction(x, y, $"NPC player{playerNum}");
                    opponentBoard.DisplayAll();
                    if (result)
                    {
                        currentSearch = new HashSet<Tuple<int, int>>();
                        currentSearch.Add(new Tuple<int, int>(x, y));
                        searchNewShip = false;
                    }
                    done = true;
                }
            }
        }
        // Hmmm, there are some really difficult game states for NPC to check
        // In the interest of time, I will not make the AI completely optimal
        // Some ship placement can be really tricky and will require AI to keep track of order of moves
        // For now I do not want to do that
        protected void Nextmove()
        {
            List<int[]> soFar = new List<int[]>();
            // NPC will randomly select a hit, and use DFS to prioritize adjacent hits
            // Continue Adjacent path if able, otherwise, choose any legal move
            // Randomly choose to go thorugh direction list forward or backward
            int index = rand.Next(currentSearch.Count);

            //gotta assign it to something
            Tuple<int, int> temp;
            HashSet<Tuple<int, int>> highPriority = new HashSet<Tuple<int, int>>();
            HashSet<Tuple<int, int>> lowPriority = new HashSet<Tuple<int, int>>();
            List<int[]> highPriorityList = new List<int[]>();
            List<int[]> lowPriorityList = new List<int[]>();
            // Randomly choosing a number from HashSet
            // O(N) but best to have this here since it's done once per move
            foreach (Tuple<int, int> t in currentSearch) {
                foreach (Tuple<int, int> direction in DIRECTIONS) {
                    temp = new Tuple<int, int>(t.Item1 + direction.Item1, t.Item2 + direction.Item2);
                    // check if the move is good
                    if (ValidMove(temp.Item1, temp.Item2)) {
                        if (TwoAdjacent(temp.Item1, temp.Item2)) {
                            if (!highPriority.Contains(temp))
                            {
                                highPriority.Add(temp);
                                highPriorityList.Add(new int[] { temp.Item1, temp.Item2 });
                            }
                        }
                        else if (!lowPriority.Contains(temp))
                        {
                            lowPriority.Add(temp);
                            lowPriorityList.Add(new int[] { temp.Item1, temp.Item2 });
                        }
                    }
                }
            }
            int[] result;
            if (highPriority.Count > 0)
            {
                result = highPriorityList[rand.Next(highPriorityList.Count)];
            }
            else if (lowPriority.Count > 0)
            {
                result = lowPriorityList[rand.Next(highPriorityList.Count)];
            }
            else
            {
                result = new int[] { 0, 0 };
                Console.WriteLine("Something went seriously wrong at choose next NPC move");
            }
            opponentBoard.MakeMove(result[0], result[1]);
            if (opponentBoard.IsHit(result[0], result[1]))
            {
                currentSearch.Add(new Tuple<int, int>(result[0], result[1]));
            }
            opponentBoard.DisplayAction(result[0], result[1], $"NPC player{playerNum}");
            opponentBoard.DisplayAll();


            // Check if AI will continue to search for ships or go completely random next turn
            FoundAllNearbyShips();
        }


        // Maybe DFS is not the best for this since it's not a can you find a solution type of thing
        // Going to instead do Find all of legal surrounding moves, and separate them in to high and low priority
        // High priority are ones with 2 moves adjacent in a line, low is everything else.
        // Will not bother checking for duplicates. Just have locations taht overlap have higher chance of being picked

        protected bool ValidMove(int x, int y)
        {
            if (x < 0 || x >= GameState.BOARDHEIGHT || y < 0 || y >= GameState.BOARDWIDTH)
            {
                return false;
            }
            return opponentBoard.IsEmpty(x, y);
        }
        protected bool TwoAdjacent(int x, int y)
        {
            foreach (Tuple<int, int> tuple in DIRECTIONS)
            {
                if (currentSearch.Contains(new Tuple<int, int>(x + tuple.Item1, y + tuple.Item2)))
                {
                    if (currentSearch.Contains(new Tuple<int, int>(x + tuple.Item1 * 2, y + tuple.Item2 * 2)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
 
        // Try best moves to sink this ship

        protected void FoundAllNearbyShips()
        {
            if (prevTotal + currentSearch.Count == opponentBoard.SunkShipTotal())
            {
                searchNewShip = true;
                prevTotal += currentSearch.Count;
            }
        }

        public override void PlaceShips()
        {
            int x;
            int y;
            bool placed;
            Tuple<int, int> modifier;
            
            Console.Clear();
            ownBoard.DisplayOwnBoard();
            ownBoard.DisplayAll();
            foreach (Tuple<string, int> pair in GameEngine.PIECES)
            {
                placed = false;
                while (!placed)
                {
                    placed = false;
                    x = rand.Next(GameState.BOARDHEIGHT);
                    y = rand.Next(GameState.BOARDWIDTH);
                    modifier = DIRECTIONS[rand.Next(DIRECTIONS.Length)];
                    
                    if (ValidPlacement(pair, x, y, modifier)) {
                        AddShip(x, y, pair, modifier);
                        placed = true;
                    }
                }
            }
            Console.WriteLine("The NPC has finished placing its ships");
            // DEBUG1
            EndTurn();
        }

        private void EndTurn()
        {
            Console.WriteLine("The NPC has finished making its decision. Press any key to continue");
            Console.ReadKey();
        }
    }
}
