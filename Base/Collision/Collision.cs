using Godot;
using System;
public enum  Direction : int
{
    LEFT    = 0,
    RIGHT   = 1,
    UP      = 2,
    DOWN    = 3,
}
// public static class Direction
// {
//     public const uint LEFT    = 0b10000000000000000000000000000000;
//     public const uint RIGHT   = 0b00000000000000000000000000000001;
//     public const uint UP      = 0b01000000000000000000000000000000;
//     public const uint DOWN    = 0b00000000000000000000000000000010;
// }
public struct WarroirCollision
{
    public uint left;
    public uint right;
    public uint down;
    public uint up;
    public uint horizontal_mask;
    public uint vertical_mask;
    public uint horizontal_end;
    public uint vertical_end;

    public void Clear()
    {
        left    = 0b0;
        right   = 0b0;
        down    = 0b0;
        up      = 0b0;
    }
    public WarroirCollision(int HorizontalRayNum, int VerticalRayNum)
    {
        horizontal_mask = 0b0;
        vertical_mask = 0b0;
        horizontal_end = 0b1;
        vertical_end = 0b1;
        left    = 0b0;
        right   = 0b0;
        down    = 0b0;
        up      = 0b0;

        while(HorizontalRayNum > 0)
        {
            horizontal_mask |= horizontal_end;
            horizontal_end <<= 1;HorizontalRayNum--;
        }
        while(VerticalRayNum > 0)
        {
            vertical_mask |= vertical_end;
            vertical_end <<= 1;VerticalRayNum--;
        }
        vertical_end >>= 1; horizontal_end >>= 1;
    }
    public bool AnyUndected(Direction direct)
    {
        switch(direct)
        {
            case Direction.LEFT:    return (left & vertical_mask) != vertical_mask;
            case Direction.RIGHT:   return (right & vertical_mask) != vertical_mask;
            case Direction.UP:      return (up & horizontal_mask) != horizontal_mask;
            case Direction.DOWN:    return (down & horizontal_mask) != horizontal_mask;
            default: return false;
        }
    }
    public bool AnyDected(Direction direct)
    {
        switch(direct)
        {
            case Direction.LEFT:    return (left & vertical_mask) != 0;
            case Direction.RIGHT:   return (right & vertical_mask) != 0;
            case Direction.UP:      return (up & horizontal_mask) != 0;
            case Direction.DOWN:    return (down & horizontal_mask) != 0;
            default: return false;
        }
    }
    public bool All(Direction direct)
    {
        switch(direct)
        {
            case Direction.LEFT:    return (left & vertical_mask) == vertical_mask;
            case Direction.RIGHT:   return (right & vertical_mask) == vertical_mask;
            case Direction.UP:      return (up & horizontal_mask) == horizontal_mask;
            case Direction.DOWN:    return (down & horizontal_mask) == horizontal_mask;
            default: return false;
        }
        
    }
    public bool Null(Direction direct)
    {
        switch(direct)
        {
            case Direction.LEFT:    return (left | (~vertical_mask)) == (~vertical_mask);
            case Direction.RIGHT:   return (right | (~vertical_mask)) == (~vertical_mask);
            case Direction.UP:      return (up | (~horizontal_mask)) == (~horizontal_mask);
            case Direction.DOWN:    return (down | (~horizontal_mask)) == (~horizontal_mask);
            default: return false;
        }
    }
    public bool Get(Direction direct, int index)
    {
        if(index > 32) return false;
        uint mask = 0b1;
        for (;index > 0; index--) { mask <<=1; }
        switch(direct)
        {
            case Direction.LEFT:    return (left & vertical_mask) == vertical_mask;
            case Direction.RIGHT:   return (right & vertical_mask) == vertical_mask;
            case Direction.UP:      return (up & horizontal_mask) == horizontal_mask;
            case Direction.DOWN:    return (down & horizontal_mask) == horizontal_mask;
            default: return false;
        }
    }
    /// <summary>
    /// 返回下方两个角的检测（以down为准）
    /// </summary>
    /// <returns></returns>
    public bool CornerDown(Direction direct) 
    { 
        switch(direct)
        {
            case Direction.LEFT:    return (down & 0b1) == 0b1;
            case Direction.RIGHT:   return (down & horizontal_end) == horizontal_end;
            default: return false;
        }
    }
    public bool CornerUp(Direction direct)
    { 
        switch(direct)
        {
            case Direction.LEFT:    return (down & 0b1) == 0b1;
            case Direction.RIGHT:   return (down & vertical_end) == vertical_end;
            default: return false;
        }
    }


}