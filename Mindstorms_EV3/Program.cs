﻿
using Mindstorms.Core.EV3;
using Mindstorms_EV3.EV3;
using MonoBrick.EV3;


public class Program 
{
    public static void Main(string[] args)
    {
        EV3Controls eV3Controls = new EV3Controls();
        var brick = new Brick<Sensor, Sensor, Sensor, Sensor>("USB");
        eV3Controls.init(brick);
    }
}