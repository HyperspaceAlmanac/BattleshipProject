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
        public static readonly ConsoleColor REPORT = ConsoleColor.DarkBlue;
        public static readonly ConsoleColor REPORT_BACKGROUND = ConsoleColor.Gray;
        public static readonly ConsoleColor PLACE_SHIP_COLOR = ConsoleColor.Green;
        public static readonly ConsoleColor SHIP_OVERLAP = ConsoleColor.DarkGray;
        public static readonly int BOARDWIDTH = 20;
        public static readonly int BOARDHEIGHT = 20;
        private static readonly string BORDER = " A B C D E F G H I J K L M O P Q R S T U ";
        private static readonly string NUMTOALPHABET = "ABCDEFGHIJKLNOPQRSTUVWXYZ";
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
            this.opponentBoard = true;
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
            Console.ForegroundColor = REPORT;
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
        public void DisplayAll()
        {
            DisplayShipState();
            DisplayBoard();
        }

        public void DisplayPlaceShip(Tuple<int, int>[] newShipCoords = null) {
            DisplayShipState();
            DisplayBoard(true, newShipCoords);
        }

        private void DisplayShipState()
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

        private void DisplayBoard(bool checkOverlap = false, Tuple<int, int>[] newShipCoords = null)
        {
            // Display letters at top
            Console.WriteLine(" " + BORDER);
            bool shipLocation;
            bool newShipHere;
            for (int i = 0; i < boardState.GetLength(0); i++)
            {
                Console.Write((i < 9 ? " " : "") + (i + 1));
                if (opponentBoard && highlightRow && rowSelected == i)
                {
                    Console.BackgroundColor = HIGHLIGHT;
                }
                for (int j = 0; j < boardState.GetLength(1); j++)
                {
                    ConsoleColor prev = Console.BackgroundColor;
                    if (opponentBoard && highlightColumn && columnSelected == j)
                    {
                        Console.BackgroundColor = HIGHLIGHT;
                    }
                    // Display your own board, check if this is a ship location
                    else if (!opponentBoard)
                    {
                        shipLocation = allShipLocations.Contains(new Tuple<int, int>(i, j));
                        // now need to check for overlap
                        if (checkOverlap)
                        {
                            newShipHere = false;
                            // If checking overlap, go through list of new coordinates
                            foreach (Tuple<int, int> coord in newShipCoords)
                            {
                                if (coord.Item1 == i && coord.Item2 == j)
                                {
                                    if (shipLocation)
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
                            if (!newShipHere && shipLocation)
                            {
                                Console.BackgroundColor = SHIP_COLOR;
                            }
                        }
                        else if (shipLocation)
                        {
                            Console.BackgroundColor = SHIP_COLOR;
                        }
                    }
                    DisplayLocation(boardState[i, j]);
                    Console.BackgroundColor = prev;
                }
                Console.ResetColor();
                Console.Write((i + 1) + (i < 9 ? " " : ""));
                Console.WriteLine();
            }
            Console.WriteLine(" " + BORDER);
            for (int i = 0; i < 4 + BOARDWIDTH * 2; i++)
            {
                Console.Write("=");
            }
            Console.WriteLine();
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

        public void DisplayOpponentBoard()
        {
            opponentBoard = true;
        }
        public void DisplayOwnBoard()
        {
            opponentBoard = false;
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
