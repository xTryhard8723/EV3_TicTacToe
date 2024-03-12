
using EV3_TicTacToe.Audio;
using Mindstorms.Core.EV3;
using Mindstorms_EV3.EV3;
using MonoBrick.EV3;
using System.Speech.Recognition;



public class Program 
{
    public static void Main(string[] args)
    {
        //SOME KIND OF DOCS? :http://www.monobrick.dk/software/ev3firmware/
        //bigger docs xd: https://github.com/smallrobots/monoev3

        
        EV3Controls eV3Controls = new EV3Controls();
           var brick = new Brick<Sensor, Sensor, Sensor, Sensor>("usb");
        eV3Controls.init(brick);
      
    }
}