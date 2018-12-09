using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShip.Managers
{
    public class ResponseManager
    {
        public string handleCommand(GameManager gameManager, string receivedCommand)
        {
            if (receivedCommand.Contains("FIRE", StringComparison.InvariantCultureIgnoreCase))
            {
                var shot      = gameManager.TrimShot(receivedCommand.ToUpper());
                var missOrHit = gameManager.Fire(shot[0], shot[1]);
                gameManager.DrawBoard();

                return missOrHit;
                
            }
            else if (string.Equals(receivedCommand, "EXIT", StringComparison.InvariantCultureIgnoreCase))
            {
                //writer.WriteLine("BYE BYE");
                //break;
                return "Hastala vista baby";
            }

            return "";
        }
    }
}
