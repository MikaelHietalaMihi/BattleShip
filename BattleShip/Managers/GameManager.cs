using BattleShip.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleShip.Managers
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
            SetupGame();

            PlaceShip(ships[0], 2, 2, "V"); // Length 5
            PlaceShip(ships[1], 1, 3, "H"); // Length 4
            PlaceShip(ships[2], 6, 5, "V"); // Length 3
            PlaceShip(ships[3], 8, 7, "H"); // Length 3
            PlaceShip(ships[4], 1, 9, "V"); // Length 2

            DrawBoard();
        }

        public GameManager()
        {
            SetupGame();
            DrawBoard();
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

        public string Fire(int vertivcalPos, int horizontalPos)
        {
            // Ändrar positionen till 0 indexering för att anpassa Arrayerna
            vertivcalPos = vertivcalPos - 1;
            horizontalPos = horizontalPos - 1;

            try
            {
                // Här har spelaren redan skjutit
                // Spelaren får ett nytt försök
                if (grid[horizontalPos, vertivcalPos] == "X")
                {
                    return "501 Sequence error";
                }
                // Det är en tom ruta men ingen träff
                else if (grid[horizontalPos, vertivcalPos] == " ")
                {
                    grid[horizontalPos, vertivcalPos] = "X";
                    return "230 Miss!";
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
                        return $"{ship.ProtocolCode} You sunk my battleship: {ship.Name}";
                    }
                    grid[horizontalPos, vertivcalPos] = "H";
                    return $"{ship.ProtocolCode} Hit {ship.Name}";
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

        public string markTargetGrid(int vertivcalPos, int horizontalPos, bool hit)
        {

            // Ändrar positionen till 0 indexering för att anpassa Arrayerna
            vertivcalPos = vertivcalPos - 1;
            horizontalPos = horizontalPos - 1;


            // Redan skjutit där
            if (grid[horizontalPos, vertivcalPos] == "X")
            {
                return "501 Sequence error";
            }
            // Det är en tom ruta men ingen träff
            else if (grid[horizontalPos, vertivcalPos] == " " && hit == false)
            {
                grid[horizontalPos, vertivcalPos] = "X";
                return "Miss!";
            }
            else
            {
                grid[horizontalPos, vertivcalPos] = "H";
                return "Hit!";
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

        public int[] TrimShot(string str)
        {
            char verticalPos = str[5];
            char horizontalPos = str[6];

            var ascii = Convert.ToByte(verticalPos) - 64;
            int[] pos = new int[] { horizontalPos - 48, ascii };

            return pos;
        }



    }
}
