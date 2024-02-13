
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

        private void ev3Play()
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

        private void checkSensors(Brick<Sensor, Sensor, Sensor, Sensor> brick, bool debugSensors = false)
        {
            var colorSensor = new ColorSensor();
            var irSensor = new IRSensor();
            var touchSensor = new TouchSensor();
            brick.Sensor1 = touchSensor;
            brick.Sensor2 = colorSensor;
            brick.Sensor3 = irSensor;
            var smallMotor = brick.MotorB;
            var bigMotor = brick.MotorA;
            smallMotor.On(50);
            bigMotor.On(50);
            Thread.Sleep(2000);
            smallMotor.Off(true);
            bigMotor.Off(true);

            if (colorSensor.ReadColor == null
                || irSensor.ReadAsString() == null
                || touchSensor.ReadAsString == null)
            {
                throw (new Exception("Some sensors are null!"));
            }

            for (int i = 0; i < 10 && debugSensors; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"Color Sensor: ${colorSensor.ReadColor()}\nIR Sensor {irSensor.ReadAsString()}\n" +
                    $"Touch Sensor {touchSensor.ReadAsString()}\n\n");
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
                throw (new Exception(ex.Message));
            }
            Console.WriteLine("Brick is connected!");

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

        private void debugBoard()
        {
            while(algorithm.checkWinner() == ' ') {
                ev3Play();
            }
            Console.WriteLine($"Winner: {algorithm.checkWinner()} \n");
   
            for (int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    var consoleOut = board[i, j] == ' ' ? 'N' : board[i, j];
                    Console.Write(consoleOut);
                }
                Console.WriteLine("\n");
            }
                
           
        }

        public void init(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {

            connectBrick(brick);
            checkSensors(brick, true);
              while(algorithm.checkWinner() == ' ' || algorithm.isBoardFull() || !getStart(brick))
              {
                  turnPlayerO(brick);

                  turnPlayerX(brick);
                  checkPlayerInput(brick);
                  readGrid(brick);

              }
             disconnectBrick(brick);
        }

    }
}
