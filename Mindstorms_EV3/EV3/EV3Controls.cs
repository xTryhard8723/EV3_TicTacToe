
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
using Mindstorms_EV3.Algorithms;

namespace Mindstorms_EV3.EV3
{
    public class EV3Controls
    {
        private TicTacToeAlgorithm algorithm = new TicTacToeAlgorithm();

        private static char[,] board = {
        {' ', ' ', ' '},
        {' ', ' ', ' '},
        {' ', ' ', ' '}
        };


        private bool getStart(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            var touchSensor = new TouchSensor();
            brick.Sensor3 = touchSensor;
            touchSensor.Initialize();

            return touchSensor.Read() == 1;
        }

        private void moveOnGrid(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            ///TODO: Check the whole grid and read it, if the return value from readgrid is 1, read the one that it returned again


        }

        private void ev3Play(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            var move = algorithm.ev3Move();
            switch (move[0])
            {
                case 0:
                    {
                        //motors bring first row of board
                        break;
                    }
                case 1:
                    {
                        //motors bring middle row of board
                        break;
                    }
                case 2:
                    {
                        //motors bring last row of board
                        break;
                    }
            }

            switch (move[1])
            {
                case 0:
                    {
                        //motors bring go left or right and drop cube
                        break;
                    }
                case 1:
                    {
                        //motors bring go left or right and drop cube
                        break;
                    }
                case 2:
                    {
                        //motors bring go left or right and drop cube
                        break;
                    }
            }

        }

        private void checkPlayerInput(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            var distanceSensor = new IRSensor();
            brick.Sensor2 = distanceSensor;
            distanceSensor.Initialize();

            while (distanceSensor.Read() < 255)
            {
                Thread.Sleep(500);
            }
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
            if (brick.Connection != null)
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

        public static void setBoard(char[,] updatedBoard)
        {
            board = updatedBoard;
        }

        public void init(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
              connectBrick(brick);
              while(algorithm.checkWinner() == ' ' || algorithm.isBoardFull() || !getStart(brick))
              {
                  turnPlayerO(brick);
                  ev3Play(brick);
                  turnPlayerX(brick);
                  checkPlayerInput(brick);
                  readGrid(brick);

              }
              disconnectBrick(brick);
        }

    }
}
