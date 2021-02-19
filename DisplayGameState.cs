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
    // Will pretty much be for displaying gameboards side by side
    // Just 
    static class DisplayGameState
    {
        private static readonly string BORDER = "  A B C D E F G H I J K L M N O P Q R S T ";
        private static readonly string OFFSET = "                                            ";
        public static void DisplayBothBoards(GameState opponentBoard, GameState ownBoard, int x = -1, int y = -1)
        {
            // Opponent first, and then ownboard
            Console.WriteLine("Opponent Fleet Status");
            opponentBoard.DisplayShipState();
            Console.WriteLine(BORDER);
            int j;
            for (int i = 0; i < GameState.BOARDHEIGHT + 5; i++)
            {
                if (i < GameState.BOARDHEIGHT)
                {
                    RowNumber(i);
                    opponentBoard.DisplayOpponentRow(i, x, y);
                    RowNumber(i, false);
                } else {
                    if (i == GameState.BOARDHEIGHT)
                    {
                        Console.Write(BORDER + "  ");
                    }
                    else
                    {
                        Console.Write(OFFSET);
                    }
                }
                Separator();

                j = i - 5;
                if (j == -1)
                {
                    Console.Write(BORDER);
                }
                if (j > -1)
                {
                    RowNumber(j);
                    ownBoard.DisplayOwnRow(j);
                    RowNumber(j, false);
                }
                Console.WriteLine();
            }
            Console.Write(OFFSET);
            Separator();
            Console.WriteLine(BORDER);
            Console.WriteLine("Fleet Status");
            ownBoard.DisplayShipState();
        }

        public static void DisplayOwnBoard(GameState board, Tuple<int, int>[] shipCoordinates = null)
        {
            board.DisplayShipState();
            Console.WriteLine(BORDER);
            for (int i = 0; i < GameState.BOARDHEIGHT; i++)
            {
                RowNumber(i);
                if (shipCoordinates is null)
                {
                    board.DisplayOwnRow(i);
                }
                else
                {
                    board.DisplayOwnShipOverlapRow(i, shipCoordinates);
                }
                RowNumber(i, false);
                Console.WriteLine();
            }
            Console.WriteLine(BORDER);
        }

        public static void DisplayOpponentBoard(GameState board, int row = -1, int column = -1)
        {
            board.DisplayShipState();
            Console.WriteLine(BORDER);
            for (int i = 0; i < GameState.BOARDHEIGHT; i++)
            {
                RowNumber(i);
                board.DisplayOpponentRow(i, row, column);
                RowNumber(i, false);
                Console.WriteLine();
            }
            Console.WriteLine(BORDER);
        }

        private static void Separator()
        {
            // 10 empty spaces
            Console.Write("          ");
            Console.Write("|");
            Console.Write("          ");
        }

        private static void RowNumber(int row, bool shiftRight = true)
        {
            // index of 10 is 9
            if (row < 9)
            {
                if (shiftRight)
                {
                    Console.Write(" " + (row + 1));
                }
                else
                {
                    Console.Write((row + 1) + " ");
                }
            }
            else
            {
                Console.Write((row + 1).ToString());
            }
        }
    }
}
