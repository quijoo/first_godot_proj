using Godot;
using System;
using System.Collections.Generic;
static public class ExtendKinematicCollision2D
{
    static public IEnumerable<KinematicCollision2D> Extend_GetCollison(this KinematicBody2D body)
    {
        for(int i = 0; i < body.GetSlideCount(); i++)
        {
            yield return body.GetSlideCollision(i);
        }
    }
}
