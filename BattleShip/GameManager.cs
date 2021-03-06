﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleShip
{
    public class GameManager
    {
        string[,] grid = new string[10, 10];
        string[] Letters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        string[] ShipsIDs = new string[] { "1", "2", "3", "4", "5" };

        List<Ship> ships;

        public GameManager(List<Ship> ships)
        {
            this.ships = ships;
        }

        public void SetupGame()
        {
            // Fyller brädet med tomma platser
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    grid[i, j] = " ";
                }
            }
        }

        public void DrawBoard()
        {
            // I == 1 2 3 4
            // J == A B C D

            // Skriver ut bokstäver i toppen
            Console.Write("\t ");
            for (int i = 0; i < Letters.Length; i++)
            {
                Console.Write("  " + Letters[i] + " ");
            }
            Console.WriteLine();

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("\t  ----------------------------------------");
                Console.Write(i + 1 + ")\t");
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(" | ");
                    // Det är en båt på platsen
                    if (grid[j, i] == "1" || grid[j, i] == "2" || grid[j, i] == "3" || grid[j, i] == "4" || grid[j, i] == "5")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("#");
                        Console.ResetColor();
                    }

                    // Båten är träffad
                    else if (grid[j, i] == "H")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("X");
                        Console.ResetColor();
                    }
                    else if (grid[j, i] == "X")
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(grid[j, i]);
                        Console.ResetColor();
                    }
                    else
                        //Console.Write(" ");
                        Console.Write(grid[j, i]);

                }
                Console.Write(" | ");
                Console.WriteLine();
            }
            Console.WriteLine("\t  ----------------------------------------");

        }

        public bool Fire(int vertivcalPos, int horizontalPos)
        {
            vertivcalPos = vertivcalPos - 1;
            horizontalPos = horizontalPos - 1;
            try
            {
                // Här har spelaren redan skjutit
                // Spelaren får ett nytt försök
                if (grid[horizontalPos, vertivcalPos] == "X")
                {
                    return false;
                }
                // Det är en tom ruta men ingen träff
                else if (grid[horizontalPos, vertivcalPos] == " ")
                {
                    grid[horizontalPos, vertivcalPos] = "X";
                    return false;
                }
                // Träff
                else
                {
                    string id = grid[horizontalPos, vertivcalPos];
                    Ship ship = ships.FirstOrDefault(c => c.ID == id);

                    ship.Lenght -= 1;
                    if (ship.Lenght <= 0)
                    {
                        ship.Sunk = true;
                        Console.WriteLine("You sunk my battleship: " + ship.Name);
                    }
                    grid[horizontalPos, vertivcalPos] = "H";
                    return true;
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public bool PlaceShip(Ship ship, int VerticalStartPos, int HorizontalStartPos, string direction)
        {
            if (direction.ToUpper() != "V" && direction.ToUpper() != "H")
            {
                return false;
            }

            // Placera vertikalt 
            if (direction.ToUpper() == "V")
            {
                for (int i = VerticalStartPos; i < VerticalStartPos + ship.Lenght; i++)
                {
                    grid[i - 1, HorizontalStartPos - 1] = ship.ID;
                }
            }

            // Placera horisontellt
            else
            {
                for (int i = HorizontalStartPos; i < HorizontalStartPos + ship.Lenght; i++)
                {
                    grid[VerticalStartPos - 1, i - 1] = ship.ID;
                }
            }

            return true;

        }




        public void testToDraw(int x, int y)
        {
            grid[x, y] = "V";
            DrawBoard();
        }


    }
}
