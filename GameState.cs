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
        public static readonly ConsoleColor ALIVE_COLOR = ConsoleColor.Magenta;
        public static readonly ConsoleColor SUNK_COLOR = ConsoleColor.Red;
        public static readonly ConsoleColor SHIP_COLOR = ConsoleColor.White;
        public static readonly ConsoleColor HIGHLIGHT = ConsoleColor.White;
        public static readonly ConsoleColor REPORT_COLOR = ConsoleColor.DarkBlue;
        public static readonly ConsoleColor REPORT_BACKGROUND = ConsoleColor.Gray;
        public static readonly ConsoleColor PLACE_SHIP_COLOR = ConsoleColor.Green;
        public static readonly ConsoleColor SHIP_OVERLAP = ConsoleColor.DarkGray;
        public static readonly int BOARDWIDTH = 20;
        public static readonly int BOARDHEIGHT = 20;
        public static readonly string NUMTOALPHABET = "ABCDEFGHIJKLNOPQRSTUVWXYZ";
        Location[,] boardState;
        List<Ship> ships;
        List<Tuple<int, int>> allShipLocations;
        HashSet<Tuple<int, int>> moveHistory;
        int numShots;

        // Options for displaying current selected row / column
        // These are only set through function that checks that these are in range and only update then
        private int rowSelected;
        private int columnSelected;

        private bool highlightRow;
        private bool highlightColumn;
        // Temporarily make public
        private bool opponentBoard;

        public GameState()
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
            opponentBoard = true;
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
        public void DisplayAction(int x, int y, string player)
        {
            Console.BackgroundColor = REPORT_BACKGROUND;
            Console.ForegroundColor = REPORT_COLOR;
            Console.Write(player + " chose " + NUMTOALPHABET[y].ToString() + (x + 1) + ".");
            if (boardState[x, y] == Location.Hit)
            {
                Console.ForegroundColor = SUNK_COLOR;
            }
            else
            {
                Console.ForegroundColor = ALIVE_COLOR;
            }
            Console.ResetColor();
            Console.WriteLine("It was a " + (boardState[x, y] == Location.Hit ? "Hit" : "Miss") + "!");
            Console.ResetColor();
        }

        public bool ValidPlacement(Tuple<int, int>[] shipCoordinates)
        {
            foreach (Tuple<int, int> coordinate in shipCoordinates)
            {
                if (LocationOccupied(coordinate.Item1, coordinate.Item2))
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
                    moveHistory.Add(new Tuple<int, int>(x, y));
                    return current == Location.Hit;
                }
            }
            Console.WriteLine("Should not reach here! Need to debug");
            return false;
        }

        private void DisplayLocation(Location location)
        {
            if (location == Location.Empty)
            {
                Console.Write("  ");
            }
            else if (location == Location.Hit)
            {
                Console.ForegroundColor = SUNK_COLOR;
                Console.Write(" o");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ALIVE_COLOR;
                Console.Write(" x");
                Console.ResetColor();
            }

        }

        public void DisplayShipState()
        {
            foreach (Ship s in ships)
            {
                s.DisplayState();
                Console.Write(" | ");
            }
            Console.WriteLine();
            for (int i = 0; i < 4 + BOARDWIDTH * 2; i++)
            {
                Console.Write("=");
            }
            Console.WriteLine();
        }

        public void DisplayOpponentRow(int row, int highlightedRow = -1, int highlightedColumn = -1)
        {
            for (int j = 0; j < BOARDWIDTH; j++)
            {
                if ((highlightedRow > -1 && highlightedRow == row)
                    || (highlightedColumn > -1 && highlightedColumn == j)) {
                    Console.BackgroundColor = HIGHLIGHT;
                }
                DisplayLocation(boardState[row, j]);
                Console.ResetColor();
            }
        }
        public void DisplayOwnRow(int row)
        {
            for (int j = 0; j < BOARDWIDTH; j++)
            {
                if (allShipLocations.Contains(new Tuple<int, int>(row, j))) {
                    Console.BackgroundColor = HIGHLIGHT;
                }
                DisplayLocation(boardState[row, j]);
                Console.ResetColor();
            }
        }

        public void DisplayOwnShipOverlapRow(int row, Tuple<int, int>[] newShipCoords)
        {
            bool shipAlreadyHere;
            bool newShipHere;
            for (int j = 0; j < BOARDWIDTH; j++)
            {
                shipAlreadyHere = allShipLocations.Contains(new Tuple<int, int>(row, j));
                newShipHere = false;

                // If checking overlap, go through list of new coordinates
                foreach (Tuple<int, int> coord in newShipCoords)
                {
                    if (coord.Item1 == row && coord.Item2 == j)
                    {
                        if (shipAlreadyHere)
                        {
                            Console.BackgroundColor = SHIP_OVERLAP;
                        }
                        else
                        {
                            Console.BackgroundColor = PLACE_SHIP_COLOR;
                        }
                        newShipHere = true;
                        break;
                    }
                }
                if (!newShipHere && shipAlreadyHere)
                {
                    Console.BackgroundColor = SHIP_COLOR;
                }

                DisplayLocation(boardState[row, j]);
                Console.ResetColor();
            }
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
            return !moveHistory.Contains(new Tuple<int, int>(x, y));
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

        public bool IsEmpty(int x, int y)
        {
            if (x > -1 && x < BOARDHEIGHT && y > -1 && y < BOARDWIDTH)
            {
                return boardState[x, y] == Location.Empty;
            }
            Console.WriteLine("Need to debug");
            return false;
        }

        public bool IsHit(int x, int y)
        {
            return boardState[x, y] == Location.Hit;
        }
    }
}
