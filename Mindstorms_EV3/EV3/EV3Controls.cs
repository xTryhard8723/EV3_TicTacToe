
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

        private static char[,] board = {
        {' ', ' ', ' '},
        {' ', ' ', ' '},
        {' ', ' ', ' '}
        };


        private void connectBrick(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {

            try
            {
                brick.Connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            var connectionString = brick.Connection == null ? "Brick is not connected!" : "Brick is successfully connected!";
            Console.WriteLine(connectionString);

        }

        private void disconnectBrick(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            if (brick != null && brick.Connection != null)
            {
                Console.WriteLine("Disconnecting brick!");
                brick.Connection.Close();
                Console.WriteLine("Brick disconnected");
            }
        }

        public static char[,] getBoard()
        {
            return board;
        }

        public static void setBoard(char[,] Algboard)
        {
            board = Algboard;
        }

        public void init(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            connectBrick(brick);
            disconnectBrick(brick);
        }

    }
}
