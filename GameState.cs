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
    // Used to keep track of current state of the game.
    // Passed around to be updated by other classes
    enum Location {
        Empty,
        Hit,
        Miss
    }
    class GameState
    {
        // String representation of hits, misses, and ships
        public static readonly int BOARDWIDTH = 10;
        public static readonly int BOARDHEIGHT = 10;
        private static readonly string TOPBORDER = "  A B C D E F G H I J";
        Location[,] boardState;
        List<Ship> ships;
        List<Tuple<int, int>> allShipLocations;
        HashSet<Tuple<int, int>> moveHistory;
        int numShots;

        // Options for displaying current selected row / column
        private int rowSelected;
        private int columnSelected;
        private bool highlightRow;
        private bool highlightColumn;
        // Temporarily make public
        public bool opponentBoard;

        public GameState(bool opponentBoard)
        {
            highlightRow = false;
            highlightColumn = false;
            boardState = new Location[BOARDWIDTH, BOARDHEIGHT];
            for (int i = 0; i < BOARDHEIGHT; i++)
            {
                for (int j = 0; j < BOARDWIDTH; j++)
                {
                    boardState[i, j] = Location.Empty;
                }
            }
            numShots = 0;
            ships = new List<Ship>();
            allShipLocations = new List<Tuple<int, int>>();
            moveHistory = new HashSet<Tuple<int, int>>();
            this.opponentBoard = opponentBoard;
        }

        // check if game can continue
        public bool MovesAvailable()
        {
            return numShots < BOARDWIDTH * BOARDHEIGHT;
        }

        // check if this player has won (sunk all of opponent's ships)
        public bool AllShipsSunk()
        {
            foreach (Ship s in ships)
            {
                if (s.IsAlive())
                {
                    return false;
                }
            }
            return true;
        }

        public bool MakeMove(int x, int y)
        {
            // Check that coordinates are correct
            if (x > -1 && x < BOARDHEIGHT && y > -1 && y < BOARDWIDTH)
            {
                if (boardState[x, y] == Location.Empty)
                {
                    Location current = Location.Miss;
                    foreach (Ship s in ships)
                    {
                        if (s.hitShip(x, y))
                        {
                            current = Location.Hit;
                            break;
                        }
                    }
                    boardState[x, y] = current;
                    numShots += 1;
                    return current == Location.Hit;
                }
            }
            Console.WriteLine("Should not reach here! Need to debug");
            return false;
        }

        // use this to experiment with displayign color to console
        public static void ColorDisplayTest()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Cyan");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Green");
        }

        private void DisplayLocation(Location location)
        {
            if (location == Location.Empty)
            {
                Console.Write("  ");
            }
            else if (location == Location.Hit)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" o");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(" x");
                Console.ResetColor();
            }
                
        }
        public void DisplayAll()
        {
            DisplayShipState();
            DisplayBoard();
        }
        private void DisplayShipState()
        {
            foreach (Ship s in ships)
            {
                s.DisplayState();
            }
            for (int i = 0; i < 4 + BOARDWIDTH * 2; i++)
            {
                Console.Write("=");
            }
            Console.WriteLine();
        }

        private void DisplayBoard()
        {
            // Display letters at top
            Console.WriteLine(" " + TOPBORDER);
            for (int i = 0; i < boardState.GetLength(0); i++)
            {
                Console.Write((i < 9 ? " " : "") + (i + 1));
                if (opponentBoard && highlightRow && rowSelected == i)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                }
                for (int j = 0; j < boardState.GetLength(1); j++)
                {
                    ConsoleColor prev = Console.BackgroundColor;
                    if (opponentBoard && highlightColumn && columnSelected == j)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    // Display your own board, check if this is a ship location
                    else if (!opponentBoard)
                    {
                        if (allShipLocations.Contains(new Tuple<int, int>(i, j))) {
                            Console.BackgroundColor = ConsoleColor.White;
                        }
                    }
                    DisplayLocation(boardState[i, j]);
                    Console.BackgroundColor = prev;
                }
                Console.ResetColor();
                Console.WriteLine();
            }
        }

        // For manually testing adding in ships
        public bool TestAddShip()
        {
            // Destroyer 1
            Tuple<int, int>[] shipCoords = new Tuple<int, int>[2];
            shipCoords[0] = new Tuple<int, int> (0, 0);
            shipCoords[1] = new Tuple<int, int> (0, 1);

            ships.Add(new Ship("Destroyer", shipCoords));

            // Destroyer 2
            shipCoords = new Tuple<int, int>[2];
            shipCoords[0] = new Tuple<int, int>(3, 2);
            shipCoords[1] = new Tuple<int, int>(4, 2);
            ships.Add(new Ship("Destroyer", shipCoords));

            shipCoords = new Tuple<int, int>[5];
            shipCoords[0] = new Tuple<int, int>(6, 2);
            shipCoords[1] = new Tuple<int, int>(6, 3);
            shipCoords[2] = new Tuple<int, int>(6, 4);
            shipCoords[3] = new Tuple<int, int>(6, 5);
            shipCoords[4] = new Tuple<int, int>(6, 6);
            ships.Add(new Ship("Aircraft Carrier", shipCoords));

            return true;
        }
        // Change to private later

        public void TestDisplayHighlight()
        {
            highlightRow = true;
            rowSelected = 0;
        }

        public void RemoveHighlight()
        {
            highlightRow = false;
            highlightColumn = false;
        }

        public void TogglePlayer()
        {
            opponentBoard = !opponentBoard;
        }

        public bool LocationOccupied(int x, int y)
        {
            return allShipLocations.Contains(new Tuple<int, int>(x, y));
        }

        public void AddShip(Ship ship) {
            ships.Add(ship);
            ship.FillInCoordinates(allShipLocations);
        }

        public bool AvailableMove(int x, int y)
        {
            return moveHistory.Contains(new Tuple<int, int>(x, y));
        }
        public bool LocationHit(int x, int y)
        {
            return boardState[x, y] == Location.Hit;
        }

        public int SunkShipTotal()
        {
            int total = 0;
            foreach (Ship s in ships)
            {
                if (!s.IsAlive())
                {
                    total += s.GetPoints();
                }
            }
            return total;
        }
    }
}
