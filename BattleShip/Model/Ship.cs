using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShip.Model
{
    public class Ship
    {
        public string ID { get; private set; }
        public string Name { get; private set; }
        public int Lenght { get; set; }
        public bool Sunk { get; set; }
        public string ProtocolCode { get; set; }

        public Ship(string id, string name, int lenght, string protocolCode)
        {
            ID = id;
            Name = name;
            Lenght = lenght;
            ProtocolCode = protocolCode;
        }
    }
}
