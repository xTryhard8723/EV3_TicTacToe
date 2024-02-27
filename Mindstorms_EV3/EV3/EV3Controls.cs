
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

        private void moveOnGridToRead(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            //TODO: move the whole grid, execute readGrid() and in readgrid or here write into the table with values of physical player
            var tempBoard = getBoard();

        }

        private void ev3Play(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            var move = algorithm.ev3Move();
            var armMotor = brick.MotorA;
            switch (move[0])
            {
           
                case 0:
                    {
                        //motors bring first row of board
                        armMotor.On(5, 26, true);
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
            Console.WriteLine("Testing sensors...");
            var colorSensor = new ColorSensor();
            var irSensor = new IRSensor();
            var touchSensor = new TouchSensor();
            brick.Sensor1 = touchSensor;
            brick.Sensor2 = colorSensor;
            brick.Sensor3 = irSensor;


            if (colorSensor.ReadColor() == Color.None
                || irSensor.ReadAsString() == "128"
                || touchSensor.ReadAsString == null)
            {
                throw (new Exception("Some sensors are null!"));
            }else
            {
                Console.WriteLine("Sensors tested!");
            }

            for (int i = 0; i < 10 && debugSensors; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"\n\nColor Sensor: ${colorSensor.ReadColor()}\nIR Sensor {irSensor.ReadAsString()}\n" +
                    $"Touch Sensor {touchSensor.ReadAsString()}\n\n");
            }
        }

        private void checkFullArmMovement(Brick<Sensor, Sensor, Sensor, Sensor> brick, bool debugConsole = false)
        {
            Console.WriteLine("Starting to test arm movement!");
            var touchSensorDesk = new TouchSensor();
            brick.Sensor1 = touchSensorDesk;
            uint thirdPosition = 26;
            while (touchSensorDesk.Read() == 0)
            {
                brick.MotorA.ResetTacho(true);
                if (debugConsole) { Console.WriteLine($"Tacho count: {brick.MotorA.GetTachoCount()}"); }
                Thread.Sleep(500);
                brick.MotorA.On(5, thirdPosition, true);
                WaitForMotorToStop(brick);
                if (debugConsole) { Console.WriteLine("Position: " + brick.MotorA.GetTachoCount()); }
                brick.MotorA.On(5, thirdPosition, true);
                WaitForMotorToStop(brick);
                if (debugConsole) { Console.WriteLine("Position: " + brick.MotorA.GetTachoCount()); }
                brick.MotorA.On(5, (thirdPosition - 4), true);
                WaitForMotorToStop(brick);
                if (debugConsole) { Console.WriteLine("Position: " + brick.MotorA.GetTachoCount()); }
                brick.MotorA.On(-5, 67, true);
                WaitForMotorToStop(brick);
                if (debugConsole) { Console.WriteLine("Position: " + brick.MotorA.GetTachoCount()); }
                brick.MotorA.Off(true);
                
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
            brick.Sensor2 = colorReadSensor;
            Color readColor = colorReadSensor.ReadColor();
            char returnValue;

            do
            {
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
            } while(returnValue != 1 || returnValue.ToString().Length > 0);

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

        private void debugBoard(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            while(algorithm.checkWinner() == ' ') {
                ev3Play(brick);
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
        void WaitForMotorToStop(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            Thread.Sleep(500);
            while (brick.MotorA.IsRunning()) { Thread.Sleep(50); }
        }

        public void init(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            
            connectBrick(brick);
            turnPlayerO(brick);
            //checkFullArmMovement(brick);
            //     checkSensors(brick, true);
            /*
              while(algorithm.checkWinner() == ' ' || algorithm.isBoardFull() || !getStart(brick))
              {
                  turnPlayerO(brick);

                  turnPlayerX(brick);
                  checkPlayerInput(brick);
                  readGrid(brick);

              }*/
          //  disconnectBrick(brick);
           
        }

    }
}
