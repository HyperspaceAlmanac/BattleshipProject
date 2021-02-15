using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipProject
{
    class Program
    {
        static void Main(string[] args)
        {
            GameState state = new GameState(true);
            state.TestAddShip();
            state.DisplayAll();
            state.MakeMove(0, 0);
            state.DisplayAll();
            state.MakeMove(0, 1);
            state.MakeMove(0, 2);
            state.TestDisplayHighlight();
            state.DisplayAll();
            state.RemoveHighlight();
            state.DisplayAll();
            state.opponentBoard = false;
            state.FillInShipLocations();
            state.DisplayAll();
            Console.ReadLine();
        }
    }
}
