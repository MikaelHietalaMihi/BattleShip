using BattleShip.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShip.Managers
{
    public class ShipManager
    {
        public List<Ship> CreateShip()
        {
            List<Ship> ships = new List<Ship>();

            ships.Add(new Ship("1","CARRIER", 5, "241"));
            ships.Add(new Ship("2", "BATTLESHIP", 4, "242"));
            ships.Add(new Ship("3", "DESTROYER", 3, "243"));
            ships.Add(new Ship("4", "SUBMARINE", 3, "244"));
            ships.Add(new Ship("5", "PATROL BOAT", 2, "245"));

            return ships;

        }

    }
}
