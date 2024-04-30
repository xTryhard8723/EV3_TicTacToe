using MonoBrick.EV3;
using Mindstorms_EV3.Algorithms;
using Mindstorms.Core.Commands.Mathematics.Arithmetic;

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

        private void test(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            int[] coord = new int[2] {2, 2};
            ev3Play(brick, coord);
        }

        private void moveOnGridToRead(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            //TODO: move the whole grid, execute readGrid() and in readgrid or here write into the table with values of physical player
            var tempBoard = getBoard();
            var boardMotor = brick.MotorC;
            var armMotor = brick.MotorA;
            armMotor.ResetTacho();
            boardMotor.ResetTacho();


            tempBoard[0, 0] = readGrid(brick);
            armMotor.On(5, 20, true);
            WaitForMotorToStop(brick, 'A');
            tempBoard[0, 1] = readGrid(brick);
            armMotor.On(5, 15, true);
            WaitForMotorToStop(brick, 'A');
            tempBoard[0, 2] = readGrid(brick);
            armMotor.On(-5, 40, true);
            WaitForMotorToStop(brick, 'A');

            boardMotor.On(5, 125, true);
            WaitForMotorToStop(brick, 'C');

            armMotor.ResetTacho();
            tempBoard[0, 0] = readGrid(brick);
            armMotor.On(5, 25, true);
            WaitForMotorToStop(brick, 'A');
            tempBoard[0, 1] = readGrid(brick);
            armMotor.On(5, 20, true);
            WaitForMotorToStop(brick, 'A');
            tempBoard[0, 2] = readGrid(brick);
            armMotor.On(-5, 50, true);
            WaitForMotorToStop(brick, 'A');

            boardMotor.On(5, 125, true);
            WaitForMotorToStop(brick, 'C');

            armMotor.ResetTacho();
            tempBoard[0, 0] = readGrid(brick);
            armMotor.On(5, 20, true);
            WaitForMotorToStop(brick, 'A');
            tempBoard[0, 1] = readGrid(brick);
            armMotor.On(5, 15, true);
            WaitForMotorToStop(brick, 'A');
            tempBoard[0, 2] = readGrid(brick);
            armMotor.On(-5, 40, true);
            WaitForMotorToStop(brick, 'A');

        }

        private void ev3Play(Brick<Sensor, Sensor, Sensor, Sensor> brick, int[]? readCoords = null)
        {
            int[]move;
            if(readCoords == null)
            {
                move = algorithm.ev3Move();
            }else
            {
                move = readCoords;
            }
            var armMotor = brick.MotorA;
            var boardMotor = brick.MotorC;

            armMotor.ResetTacho(true);
            boardMotor.ResetTacho(true);

            uint boardMove = 125;
            uint moveBackArm = new uint();

            switch (move[1])
            {
                case 0:
                    {

                        //motors bring go left or right and drop cube
                        break;
                    }
                case 1:
                    {
                        armMotor.On(5, 45, true);
                        moveBackArm = 48;
                        WaitForMotorToStop(brick, 'A');
                        break;
                    }
                case 2:
                    {
                        armMotor.On(5, 60, true);
                        moveBackArm = 63;
                        WaitForMotorToStop(brick, 'A');
                        break;
                    }
            }

            switch (move[0])
            {

                case 0:
                    {
                        dropCube(brick);
                        armMotor.On(-5, moveBackArm, true);
                        WaitForMotorToStop(brick, 'A');
                        break;
                    }
                case 1:
                    {

                        boardMotor.On(5, boardMove, true);
                        WaitForMotorToStop(brick, 'C');
                        dropCube(brick);
                        armMotor.On(-5, moveBackArm, true);
                        WaitForMotorToStop(brick, 'A');
                        boardMotor.On(-5, boardMove+4, true);
                        WaitForMotorToStop(brick, 'C');
                        //motors bring middle row of board
                        break;
                    }
                case 2:
                    {
                        boardMotor.On(5, boardMove, true);
                        WaitForMotorToStop(brick, 'C');
                        boardMotor.On(5, boardMove, true);
                        WaitForMotorToStop(brick, 'C');
                        dropCube(brick);
                        armMotor.On(-5, moveBackArm, true);
                        WaitForMotorToStop(brick, 'A');
                        boardMotor.On(-5, boardMove, true);
                        WaitForMotorToStop(brick, 'C');
                        boardMotor.On(-5, boardMove, true);
                        WaitForMotorToStop(brick, 'C');
                        //motors bring last row of board
                        break;
                    }
            }

        }

        private void checkSensors(Brick<Sensor, Sensor, Sensor, Sensor> brick, bool debugSensors = false, int forLoop = 10)
        {
            Console.WriteLine("Testing sensors...");
            var colorSensor = new ColorSensor();
            var touchSensor = new TouchSensor();
            brick.Sensor1 = touchSensor;
            brick.Sensor2 = colorSensor;


            if (touchSensor.ReadAsString == null)
            {
                throw (new Exception("Some sensors are null!"));
            }else
            {
                Console.WriteLine("Sensors tested!");
            }

            for (int i = 0; i < forLoop && debugSensors; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"\n\nColor Sensor: ${colorSensor.ReadColor()}\n" +
                    $"Touch Sensor {touchSensor.ReadAsString()}\n\n");
            }
        }

        private void checkFullArmMovement(Brick<Sensor, Sensor, Sensor, Sensor> brick, bool debugConsole = false)
        {
            Console.WriteLine("Starting to test arm movement!");
            var touchSensorDesk = new TouchSensor();
            brick.Sensor1 = touchSensorDesk;
            var motorToMoveArm = brick.MotorA;
            while (true)
            {
                motorToMoveArm.ResetTacho(true);
                Thread.Sleep(500);
                readGrid(brick);
                motorToMoveArm.On(5, 22, true);
                WaitForMotorToStop(brick, 'A');
                readGrid(brick);
                motorToMoveArm.On(5, 19, true);
                WaitForMotorToStop(brick, 'A');
                readGrid(brick);
                motorToMoveArm.On(5, 19, true);
                WaitForMotorToStop(brick, 'A');
            }
        }

        private void boardFullMovement(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            var boardMotor = brick.MotorC;

            boardMotor.ResetTacho(true);
            boardMotor.On(-5, 94, true);
            WaitForMotorToStop(brick, 'C');
            boardMotor.On(-5, 94, true);
            WaitForMotorToStop(brick, 'C');
            boardMotor.On(5, 100, true);
            WaitForMotorToStop(brick, 'C');
            boardMotor.On(5, 100, true);
            WaitForMotorToStop(brick, 'C');


        }

        private void dropCube(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            var armMotor = brick.MotorB;

            Console.WriteLine("Dropped cube!");
            armMotor.ResetTacho(true);
            armMotor.On(100, 76, false);
            WaitForMotorToStop(brick, 'B');
            armMotor.On(-100, 76, true);
            WaitForMotorToStop(brick, 'B');
            armMotor.Off(true);
        }

        private void checkPlayerInput(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            //TODO: give player 20s to play their move, after 20s check the grid if something changed if yes, player played if not repeat the wait
        }

        private char readGrid(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            var colorReadSensor = new ColorSensor();
            brick.Sensor2 = colorReadSensor;
            Color readColor = colorReadSensor.ReadColor();
            char returnValue;

            do
            {
                //Console.WriteLine($"Barva prectena (potencionalne spatne) {readColor.ToString()}");
                switch (readColor)
                {
                    case Color.Red:
                        {
                            returnValue = 'X';
                            break;
                        }
                    case Color.Yellow:
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
            } while(returnValue == '1');
            Console.WriteLine($"Barva prectena ted: {readColor.ToString()}");
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
                throw (new Exception($"Brick is not connected! {Environment.NewLine}{ex.Message}"));
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
        void WaitForMotorToStop(Brick<Sensor, Sensor, Sensor, Sensor> brick, char motor)
        {
            switch (motor)
            {
                case 'A':
                    Thread.Sleep(500);
                    while (brick.MotorA.IsRunning()) { Thread.Sleep(50); }
                    break;
                case 'B':
                    Thread.Sleep(500);
                    while (brick.MotorB.IsRunning()) { Thread.Sleep(50); }
                    break;
                case 'C':
                    Thread.Sleep(500);
                    while (brick.MotorC.IsRunning()) { Thread.Sleep(50); }
                    break;
            }
            Thread.Sleep(700);
        }

        public void init(Brick<Sensor, Sensor, Sensor, Sensor> brick)
        {
            
            connectBrick(brick);
            turnPlayerO(brick);
            moveOnGridToRead(brick);
            //test(brick);
            //ev3Play(brick);
            //boardFullMovement(brick);
            //checkSensors(brick, true, 5000);
            //checkFullArmMovement(brick);
            //testDrop(brick);
            //testDrop(brick);
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
