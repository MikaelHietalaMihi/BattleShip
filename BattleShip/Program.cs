using BattleShip.Managers;
using BattleShip.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShip
{
    class Program
    {
        static void Main(string[] args)
        {
            ShipManager shipManager = new ShipManager();
            List<Ship> ships = shipManager.CreateShip();

            GameManager gameManager = new GameManager(ships);
            gameManager.SetupGame();

            gameManager.PlaceShip(ships[0], 2, 2, "V"); // Length 5
            gameManager.PlaceShip(ships[1], 1, 3, "H"); // Length 4
            gameManager.PlaceShip(ships[2], 6, 5, "V"); // Length 3
            gameManager.PlaceShip(ships[3], 8, 7, "H"); // Length 3
            gameManager.PlaceShip(ships[4], 1, 9, "V"); // Length 2

            gameManager.DrawBoard();

            while (true)
            {
                Console.WriteLine("Skjut");
                string shot = Console.ReadLine();

                var shot2 = TrimShot(shot.ToUpper());
                gameManager.Fire(shot2[0], shot2[1]);

                gameManager.DrawBoard();
            }
        }


        public static int[] TrimShot(string str)
        {
            char verticalPos = str[5];
            char horizontalPos = str[6];

            var ascii = Convert.ToByte(verticalPos) - 64;

            int[] pos = new int[] { horizontalPos - 48, ascii };

            return pos;
        }






    }
}
