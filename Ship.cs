using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipProject
{
    class Ship
    {
        string shipName;
        int shipSize;
        HashSet<Tuple<int, int>> shipCoordinates;
        int hits;

        // It is the responsibility of the module creaing the ship to do
        // handle rotation and confirming that the ship location is legal
        public Ship(string name, Tuple<int,int>[] coordinates)
        {
            shipName = name;
            shipSize = coordinates.Length;
            hits = 0;
            for (int i = 0; i < coordinates.Length; i++) {
                shipCoordinates.Add(coordinates[i]);
            }
        }
        public bool IsAlive()
        {
            return hits == shipSize;
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
            return shipName;
        }
    }
}
