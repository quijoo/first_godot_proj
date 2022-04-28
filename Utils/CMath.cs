using Godot;
using System;


public class CMath
{
    static public bool ApproxEqual(float value, float interval)
    {
        return Math.Abs(value) < interval;
    }
}  



