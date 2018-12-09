using BattleShip.Managers;
using BattleShip.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace BattleShipTcpClient
{
    class BSClient
    {
        public void Host()
        {
            string sendCommand = "Fire B2"; //TODO ta bort 

            Console.WriteLine("Ange host:");
            var host = Console.ReadLine();
            Console.WriteLine("Ange port:");
            var port = int.Parse(Console.ReadLine());
          
            using (var client = new TcpClient(host, port))//IP + Port = ENDPOINT
            using (var networkStream = client.GetStream())
            using (StreamReader reader = new StreamReader(networkStream, Encoding.UTF8))
            using (var writer = new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true })
            {
                Console.Clear();

                ShipManager shipManager = new ShipManager();
                List<Ship> ships = shipManager.CreateShip();

                Console.WriteLine("Target grid");
                GameManager targetGridGameManager = new GameManager();
                Console.WriteLine("Ocean grid");
                GameManager oceanGridGameManager = new GameManager(ships);
              

                Console.WriteLine($"Ansluten till {client.Client.RemoteEndPoint}");
                Console.WriteLine($"Battleship1.1");

                writer.WriteLine(sendCommand); // Sätter igång spelet 

                while (client.Connected)
                {
                    
                    //gameManager.DrawBoard();                                              
                    var receivedCommand = reader.ReadLine().ToUpper();
                    Console.WriteLine($"Mottaget: {receivedCommand}");

                    if (receivedCommand.ToUpper().Contains("HIT") || receivedCommand.ToUpper().Contains("MISS"))
                    {
                        Console.Clear();                         
                        Console.WriteLine("Target grid");
                        var targetShot = targetGridGameManager.TrimShot(sendCommand.ToUpper());                        
                        targetGridGameManager.markTargetGrid(targetShot[0], targetShot[1], receivedCommand.ToUpper().Contains("HIT"));
                        targetGridGameManager.DrawBoard();
                        Console.WriteLine("Ocean grid");
                        oceanGridGameManager.DrawBoard();
                        Console.WriteLine($"Mottaget: {receivedCommand}");
                        Console.WriteLine("waiting for your turn..."); continue;
                    }
                    //Ta hand om svar skicka respons
                    if (receivedCommand.Contains("FIRE", StringComparison.InvariantCultureIgnoreCase))
                    {

                        var shot = oceanGridGameManager.TrimShot(receivedCommand.ToUpper());
                        var hitormiss = oceanGridGameManager.Fire(shot[0], shot[1]);
                        Console.Clear();
                        Console.WriteLine("Target grid");
                        targetGridGameManager.DrawBoard();
                        Console.WriteLine("Ocean grid");
                        oceanGridGameManager.DrawBoard();
                       
                        writer.WriteLine(hitormiss);
                    }

                    Console.WriteLine("your turn");
                    sendCommand = Console.ReadLine();
                    //TODO kolla ifall man redan skjutit på de stället
                    writer.WriteLine(sendCommand);
                }

                //while (client.Connected)
                //{                   

                //    if (!client.Connected) break;

                //    string receivedCommand;

                //    // Läs minst en rad
                //    do
                //    {
                //        receivedCommand = reader.ReadLine().ToUpper();                        
                //        Console.WriteLine($"Mottaget: {receivedCommand}");

                //        // Extra meddelanden som hoppar 
                //        //if (receivedCommand.ToUpper().Contains("HIT") || receivedCommand.ToUpper().Contains("MISS"))
                //        //{ continue; }

                //        //Ta hand om svar skicka respons
                //        if (receivedCommand.Contains("FIRE", StringComparison.InvariantCultureIgnoreCase))
                //        {
                //            var shot = gameManager.TrimShot(receivedCommand.ToUpper());
                //            var hitormiss = gameManager.Fire(shot[0], shot[1]);
                //            gameManager.DrawBoard();
                //            writer.WriteLine(hitormiss);
                //            continue;
                //        }          

                //    } while (networkStream.DataAvailable);

                //    //Hanterar "mellan meddelanden" Serverns tur att spela  
                //    if (receivedCommand.ToUpper().Contains("HIT") || receivedCommand.ToUpper().Contains("MISS"))
                //    {
                //        Console.WriteLine("Waiting for your turn");
                //        continue;
                //    }

                //    Console.WriteLine("your turn:");
                //    var sendCommand = Console.ReadLine();
                //    writer.WriteLine(sendCommand);

                //};



            }

        }
    }
}

