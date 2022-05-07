using Godot;
using System;

public class FarWeakpon : StaticBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    [Export] PackedScene bullet;
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    float timer = 0f;
    float max_time = 0.3f;
    public override void _Process(float delta)
    {
        if(timer < max_time)
        {
            timer += delta;
            return;
        }
        else timer = 0f;
        if(BasicFollowCamera.Target.IsInsideTree())
        {
            GD.Print(" init a ball");
            FireBall node = bullet.Instance<FireBall>();
            this.AddChild(node);
            node.Init(BasicFollowCamera.Target, this, 200f);
        }
    }
}
