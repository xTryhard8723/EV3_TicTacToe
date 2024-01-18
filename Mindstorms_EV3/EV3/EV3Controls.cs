
using Mindstorms.Core.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoBrick.EV3;
using System.Speech.Synthesis;
using EV3_TicTacToe.Audio;
using Mindstorms.Core.Commands.Speaker;

namespace Mindstorms_EV3.EV3
{
    public class EV3Controls
    {

        private static char[,] board = {
        {' ', ' ', ' '},
        {' ', ' ', ' '},
        {' ', ' ', ' '}
        };

        private void moveOnGrid(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            ///TODO: Check the whole grid and read it, if the return value from readgrid is 1, read the one that it returned again
        }

        private char readGrid(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            var colorReadSensor = new ColorSensor();
            brick.Sensor1 = colorReadSensor;
            colorReadSensor.Initialize();

            Color readColor = colorReadSensor.ReadColor();
            char returnValue;
            switch (readColor)
            {
                case Color.Red:
                    {
                        returnValue = 'X';
                        break;
                    }
                case Color.Green:
                    {
                        returnValue = 'O';
                        break;
                    }
                default:
                    {
                        returnValue = '1';
                        break;
                    }
            }

            return returnValue;

        }

        private void turnPlayerX(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            brick.PlayTone(100, 2600, 50);
        }

        private void turnPlayerO(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            brick.PlayTone(100, 1600, 50);
            //brick.Beep(100, 20);
        }

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
            turnPlayerO(brick);
            Thread.Sleep(1500);
            turnPlayerX(brick);
            // disconnectBrick(brick);
        }

    }
}
