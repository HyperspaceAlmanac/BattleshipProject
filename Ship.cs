﻿using System;
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
            shipCoordinates = new HashSet<Tuple<int, int>>();
            shipName = name;
            shipSize = coordinates.Length;
            hits = 0;
            // Deep copy
            for (int i = 0; i < coordinates.Length; i++) {
                shipCoordinates.Add(coordinates[i]);
            }
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
            Console.Write(ShipName());
            if (IsAlive())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(" is in operation");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" has been sunk");
            }
            Console.ResetColor();
        }

        public void FillInCoordinates(List<Tuple<int, int>> allCoord) {
            foreach (Tuple<int, int> temp in shipCoordinates)
            {
                allCoord.Add(new Tuple<int, int>(temp.Item1, temp.Item2));
            }
        }

        public int GetPoints()
        {
            return shipSize;
        }
    }
}
