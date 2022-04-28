using Godot;
using System;

public sealed class Logger : Node
{
    private Logger() {}
    public static Logger Instance { get { return Nested.instance; } }

    private class Nested
    {
        //Explicit static constructor to tell C# compiler
        //not to mark type as beforefieldinit
        static Nested()
        {

        }

        internal static readonly Logger instance = new Logger();
    }

    static public void Log(string msg)
    {
        GD.Print("[Log] " + msg);
    }
    static public void Error(string msg)
    {
        GD.Print("[Err] " + msg);
    }
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}