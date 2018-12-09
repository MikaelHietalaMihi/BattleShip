using BattleShipTcpServer;
using BattleShipTcpClient;
using System;
using BattleShip.Managers;
using BattleShip.Model;
using System.Collections.Generic;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {

            ShipManager shipManager = new ShipManager();
            List<Ship> ships = shipManager.CreateShip();

            Console.WriteLine("Target grid");
            GameManager targetGridGameManager = new GameManager();

            Console.WriteLine("Ocean grid");
            GameManager oceanGridGameManager = new GameManager(ships);            

            Console.WriteLine("Ange host:");
            var host = Console.ReadLine();
            Console.WriteLine("Ange port:");
            var port = int.Parse(Console.ReadLine());

            if (String.IsNullOrWhiteSpace(host))
            {
                BSServer bSServer = new BSServer();
                bSServer.Listen();
            }
            else {
                BSClient bSClient = new BSClient();
                bSClient.Host(host, port, targetGridGameManager, oceanGridGameManager);
            }


        }
    }
}
