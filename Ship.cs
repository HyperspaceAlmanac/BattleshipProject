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
    class Ship
    {
        string shipName;
        int shipSize;
        HashSet<Tuple<int, int>> shipCoordinates;
        int hits;

        public static readonly Tuple<string, int>[] PIECES = new Tuple<string, int>[] {
            new Tuple<string, int>("destroyer", 2),
            new Tuple<string, int>("submarine", 3),
            new Tuple<string, int>("battleship", 4),
            new Tuple<string, int>("aircraft carrier", 5)};

        public static readonly Tuple<int, int>[] DIRECTIONS = new Tuple<int, int>[] {
            new Tuple<int, int>(0, 1), new Tuple<int, int>(0, -1),
            new Tuple<int, int>(1, 0), new Tuple<int, int>(-1, 0)};

        // It is the responsibility of the module creaing the ship to do
        // handle rotation and confirming that the ship location is legal
        public Ship(string name, Tuple<int,int>[] coordinates)
        {
            shipCoordinates = new HashSet<Tuple<int, int>>();
            shipName = name;
            shipSize = coordinates.Length;
            hits = 0;
            // Deep copy
            for (int i = 0; i < coordinates.Length; i++) {
                shipCoordinates.Add(coordinates[i]);
            }
        }

        public static Tuple<int, int>[] ToShipCoordinates(Tuple<string, int> pair, int startX, int startY, Tuple<int, int> modifier)
        {
            Tuple<int, int>[] result = new Tuple<int, int>[pair.Item2];
            int modifiedX, modifiedY;
            for (int i = 0; i < pair.Item2; i++)
            {
                modifiedX = startX + modifier.Item1 * i;
                modifiedY = startY + modifier.Item2 * i;
                if (modifiedX < 0 || modifiedX >= GameState.BOARDHEIGHT ||
                    modifiedY < 0 || modifiedY >= GameState.BOARDWIDTH)
                {
                    return null;
                }
                else
                {
                    result[i] = new Tuple<int, int>(modifiedX, modifiedY);
                }
            }
            return result;
        }
        public bool IsAlive()
        {
            return hits < shipSize;
        }

        // It is the responsibility of the module calling this to ensure
        // That the provided value is not a duplicate.
        public bool hitShip(int x, int y)
        {
            Tuple<int, int> temp = new Tuple<int, int>(x, y);
            if (shipCoordinates.Contains(temp))
            {
                hits += 1;
                return true;
            }
            return false;
        }

        public string ShipName()
        {
            return shipName + $"({shipSize})";
        }

        public void DisplayState()
        {
            Console.Write(ShipName() + ":");
            if (IsAlive())
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(" Alive");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" Sunk");
            }
            Console.ResetColor();
        }

        // Has to be done this way due to hashSet being used to check for hits
        // Kind of a mess
        public void fillInCoordinates(Dictionary<Tuple<int, int>, string> shipToName) {
            foreach (Tuple<int, int> temp in shipCoordinates)
            {
                shipToName[temp] = shipName[0].ToString();
            }
        }

        public int GetPoints()
        {
            return shipSize;
        }
    }
}
