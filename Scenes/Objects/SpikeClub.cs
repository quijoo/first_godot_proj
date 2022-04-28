using Godot;
using System;

public class SpikeClub : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    [Export] float rotation_speed = 50;
    float start_rotation;
    float end_rotation;
    int direction = 1;
    [Export] public bool left = false, right = false, up = false, down = false;
    // 什么脑瘫， 直接设置两个滑杆不就行了？
    public override void _Ready()
    {
        if(left)
        {
            start_rotation = 0; 
            end_rotation = 180;
        }
        else if(up)
        {
            start_rotation = 90; 
            end_rotation = 270;
        }
        else if(right)
        {
            start_rotation = 180; 
            end_rotation = 360;
        }
        else if(down)
        {
            start_rotation = -90; 
            end_rotation = 90;
        }
        RotationDegrees = start_rotation;
    }

    public override void _PhysicsProcess(float delta)
    {
        if(RotationDegrees  < start_rotation)
        {
            direction = 1;
        }
        else if(RotationDegrees  > end_rotation)
        {
            direction = -1;
        }
        RotationDegrees += direction * rotation_speed * delta;
    }
}
