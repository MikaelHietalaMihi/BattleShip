using BattleShip.Managers;
using BattleShip.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace BattleShipTcpClient
{
    public class BSClient
    {
        public void Host(string host, int port, GameManager targetGridGameManager, GameManager oceanGridGameManager)
        {
            string sendCommand = "Fire B2"; //TODO ta bort            

            using (var client = new TcpClient(host, port))//IP + Port = ENDPOINT
            using (var networkStream = client.GetStream())
            using (StreamReader reader = new StreamReader(networkStream, Encoding.UTF8))
            using (var writer = new StreamWriter(networkStream, Encoding.UTF8) { AutoFlush = true })
            {

                //Console.WriteLine($"Ansluten till {client.Client.RemoteEndPoint}");  

                while (client.Connected)
                {
                    string playerName;

                    var receivedCommand = reader.ReadLine().ToUpper();
                    Console.WriteLine($"Mottaget: {receivedCommand}");


                    // 210 BATTLESHIP/1.0
                    if (receivedCommand == "210 BATTLESHIP / 1.0")
                    {
                        Console.WriteLine("Ange: Helo <namn>:");
                        playerName = Console.ReadLine();
                        writer.WriteLine(playerName);
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

                    // 221 Client Starts
                    else if (receivedCommand.Split(" ")[0] == "221")
                    {
                        Console.WriteLine(receivedCommand);
                        Console.WriteLine("Din tur:");
                        sendCommand = Console.ReadLine().ToUpper();
                        //TODO kolla ifall man redan skjutit på de stället
                        writer.WriteLine(sendCommand);
                        continue;
                    }

                    // 222 Host starts
                    else if (receivedCommand.Split(" ")[0] == "222")
                    {
                        Console.WriteLine(receivedCommand);
                        Console.WriteLine("waiting for your turn...");
                        continue;
                    }


                    // 241-255 Hit
                    else if (
                        receivedCommand.ToUpper().Contains("HIT") || receivedCommand.ToUpper().Contains("MISS")|| 
                        receivedCommand.ToUpper().Contains("SUNK"))
                    {
                        Console.Clear();
                        Console.WriteLine("Target grid");
                        var targetShot = targetGridGameManager.TrimShot(sendCommand.ToUpper());

                        //Märker miss eller hit på target griden
                        targetGridGameManager.markTargetGrid(targetShot[0], targetShot[1], receivedCommand.ToUpper().Contains("HIT") || receivedCommand.ToUpper().Contains("SUNK"));

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

                        Console.WriteLine($"Mottaget: {receivedCommand}");
                        writer.WriteLine(hitormiss);
                    }

                    Console.WriteLine("your turn");
                    sendCommand = Console.ReadLine().ToUpper();
                    //TODO kolla ifall man redan skjutit på de stället
                    writer.WriteLine(sendCommand);
                }
            }
        }
    }
}

