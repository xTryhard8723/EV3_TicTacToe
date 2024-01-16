using Mindstorms.Core.EV3;
using Mindstorms.Core.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms_EV3.EV3
{
    public class EV3Controls
    {
        Brick brick = new Brick("COM1");
        
        private void connectBrick()
        {
 
            brick.Connect();
            var connectString = brick.IsConnected ? "Kostka pripojena" : "Kostka nepripojena";
            Console.WriteLine(connectString);
            brick.Beep(5, 500);
        }

        public void init()
        {
            connectBrick();
        }

    }
}
