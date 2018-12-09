using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace BattleShipTcpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            BSClient bSClient = new BSClient();
            bSClient.Host();
        }
    }
}