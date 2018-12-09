using BattleShip.Managers;
using BattleShip.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BattleShipTcpServer
{
    public class BSServer
    {
        TcpListener listener;

        private void StartListen(int port)
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                Console.WriteLine($"Starts listening on port: {port}");
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Misslyckades att öppna socket. Troligtvis upptagen. " + ex);
                Environment.Exit(1);
            }
        }

        public void Listen()
        {
            Console.WriteLine("Välkommen till servern");
            Console.WriteLine("Ange port att lyssna på:");
            var port = int.Parse(Console.ReadLine());

            StartListen(port);

            while (true)
            {
                Console.WriteLine("Väntar på att någon ska ansluta sig...");

                using (var client = listener.AcceptTcpClient())
                using (var networkStream = client.GetStream())
                using (StreamReader reader = new StreamReader(networkStream, Encoding.UTF8))
                using (var writer = new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true })
                {                  

                    Console.WriteLine($"Klient har anslutit sig {client.Client.RemoteEndPoint}!");
                    Console.WriteLine($"Battleship1.1");
                    ShipManager shipManager = new ShipManager();
                    List<Ship> ships = shipManager.CreateShip();
                    Console.Clear();
                    GameManager gameManager = new GameManager(ships);
                    gameManager.DrawBoard();                    

                    writer.WriteLine("210 BATTLESHIP / 1.0");

                    while (client.Connected)
                    {
                        string clientPlayerName;
                        string sendCommand;

                        var receivedCommand = reader.ReadLine().ToUpper();
                        Console.WriteLine($"Mottaget: {receivedCommand}");

                        // HELO <playername>
                        if (receivedCommand.Split(" ")[0] == "HELO")
                        {
                            clientPlayerName = receivedCommand.Split(" ")[1];

                            Console.WriteLine("Ange: <namn>:");                            
                            writer.WriteLine($"220 {Console.ReadLine()}");
                            continue;
                        }

                        // 220 <remote player name>
                        else if (receivedCommand.Split(" ")[0] == "220")
                        {
                            Console.WriteLine(receivedCommand);
                            Console.WriteLine("Ange <start> för att starta spelet: ");
                            writer.WriteLine(Console.ReadLine());
                            continue;
                        }

                        // START
                        else if (receivedCommand == "START")
                        {
                            Random rnd = new Random();
                            int randomStart = 2; //rnd.Next(1, 2);

                            if (randomStart == 1)
                            {
                                writer.WriteLine("221 Client Starts");
                            }
                            else {
                                writer.WriteLine("222 Host Starts");
                                Console.WriteLine("Du börjar:");
                                sendCommand = Console.ReadLine().ToUpper();
                                writer.WriteLine(sendCommand);
                            }
                            continue;
                        }







                        //Tar hand om info meddelanden
                        if (receivedCommand.Contains("HIT") || receivedCommand.Contains("MISS"))
                        { Console.Write("waiting for your turn..."); continue; }

                        //Ta hand om svar skicka respons
                        writer.WriteLine(handleResonpse(receivedCommand, gameManager));

                        Console.WriteLine("your turn..");
                        sendCommand = Console.ReadLine();
                        writer.WriteLine(sendCommand);
                    }
                }
            }
        }               

        private string handleResonpse(string receivedCommand, GameManager gameManager) {

            //TODO Måste hantera extra meddelande 
            if (receivedCommand.Contains("FIRE", StringComparison.InvariantCultureIgnoreCase))
            {
                var shot = gameManager.TrimShot(receivedCommand);
                var hitOrMiss = gameManager.Fire(shot[0], shot[1]);
                Console.Clear();
                gameManager.DrawBoard();
                return hitOrMiss;
            }

            if (receivedCommand.Contains("QUIT", StringComparison.InvariantCultureIgnoreCase))
            {
                Environment.Exit(1);
            }

            if(receivedCommand.Contains("220", StringComparison.InvariantCultureIgnoreCase))
            {
                return "Hello";
            }

            //210 BATTLESHIP / 1.0
            //220 < remote player name>
            //221 Client Starts
            //222 Host Starts           
           
            //260 You win!
            //270 Connection closed
            //500 Syntax error
            //501 Sequence error

            return "";

        }
    }
}

