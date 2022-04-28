using Godot;
using System.Collections.Generic;
static public class ExtendDirection
{
    const uint M1 = 0x55555555; // 01010101010101010101010101010101
    const uint M2 = 0x33333333; // 00110011001100110011001100110011
    const uint M4 = 0x0f0f0f0f; // 00001111000011110000111100001111
    const uint M8 = 0x00ff00ff; // 00000000111111110000000011111111

    static uint cache = 0;

    static public Direction GetFlip(this Direction direction)
    {
        return direction.Reverse();
    }
    static public bool Match(this Direction d1, Direction d2)
    {
        return d1 == d2;
    }
    static private Direction Reverse(this Direction obj)
    {
        if(obj == Direction.LEFT) return Direction.RIGHT;
        else if(obj == Direction.RIGHT) return Direction.LEFT;
        else if(obj == Direction.UP) return Direction.DOWN;
        else return Direction.UP;
    }
}

