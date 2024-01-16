
using Mindstorms.Core.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoBrick.EV3;

namespace Mindstorms_EV3.EV3
{
    public class EV3Controls
    {

        
        private void connectBrick()
        {
            var brick = new Brick<Sensor, Sensor, Sensor, Sensor>("USB");
            try{
                brick.Connection.Open();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            var connectionString = brick.Connection == null ? "Brick is not connected!" : "Brick is successfully connected!";
            Console.WriteLine(connectionString);
            
        }

        public void init()
        {
            connectBrick();
        }

    }
}
