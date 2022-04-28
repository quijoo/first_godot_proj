using Godot;
using System;

public class FollowLight : Light2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    private Node2D player;
    public override void _Ready()
    {
        player = BasicFollowCamera.Target;
    }
    public override void _PhysicsProcess(float delta)
    {
        Vector2 p = ToLocal(player.ToGlobal(player.Position));

        Position = Position.LinearInterpolate(p, delta * 10);
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
