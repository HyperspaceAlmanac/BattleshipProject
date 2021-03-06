﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

/**
 * Disclaimer: I do not own the rights to Battleship.
 * This is purely a project for educational purposes in learning and practicing programming
 **/
namespace BattleshipProject
{
    // Changed my mind, just use Console.ReadKey() and update display on moves made
    class GameEngine
    {
        private bool playerOneTurn;
        private Player p1;
        private Player p2;

        public GameEngine()
        {
            playerOneTurn = true;
            RunGame();
            
        }

        private void DisplayIntro()
        {
            Console.Clear();
            Console.WriteLine("===============================");
            Console.WriteLine("Welcome to Battleship!");
            Console.WriteLine("===============================");
        }

        public void RunGame()
        {
            bool gameOver;
            bool exitGame = false;
            while (!exitGame)
            {
                playerOneTurn = true;
                gameOver = false;
                DisplayIntro();
                SelectMode();

                p1.PlaceShips();
                p1.EndTurn();
                p2.PlaceShips();
                p2.EndTurn();
                while (!gameOver)
                {
                    if (playerOneTurn)
                    {
                        p1.StartTurn();
                        p1.TakeTurn();
                        p1.EndTurn();
                    }
                    else
                    {
                        p2.StartTurn();
                        p2.TakeTurn();
                        p2.EndTurn();
                    }
                    if (p1.AllShipsSunk()) {
                        DisplayWinner(true);
                        gameOver = true;
                    } else if (p2.AllShipsSunk())
                    {
                        DisplayWinner(false);
                        gameOver = true;
                    }
                    playerOneTurn = !playerOneTurn;
                }
                exitGame = RestartGame();
            }
        }

        private void SelectMode()
        {
            bool done = false;
            while (!done)
            {
                Console.WriteLine("Please enter 1 for single player, or 2 for multiplayer");
                string val = Console.ReadLine();
                switch (val)
                {
                    case "1":
                    case "2":
                        GameState state1 = new GameState();
                        GameState state2 = new GameState();
                        p1 = new HumanPlayer(1, state1, state2);
                        if (val == "1")
                        {   
                            p2 = new NPC(2, state2, state1);
                        }
                        else
                        {
                            p2 = new HumanPlayer(2, state2, state1);
                        }

                        done = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private void DisplayWinner(bool playerOne)
        {
            Console.Clear();
            Console.WriteLine("============================");
            Console.WriteLine("Player" + (playerOne ? 1 : 2) + " is the winner!");
            Console.WriteLine("============================");
        }

        // Maybe a bit confusing, but this method saves to the variable "exit"
        // Return false to replay, otherwise exit the game
        private bool RestartGame() {
            Console.WriteLine("Play again? Please Enter \"yes\" to play again, otherwise the game will exit");
            string val = Console.ReadLine();
            if (val == "yes")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
