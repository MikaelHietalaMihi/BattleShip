using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace BattleShipTcpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            BSServer bSServer = new BSServer();
            bSServer.Listen();

        }
    }


}