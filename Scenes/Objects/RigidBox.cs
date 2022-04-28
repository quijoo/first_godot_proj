using Godot;
using System;

public class RigidBox : RigidBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    int max_speed = 10;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    
    // 
    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        // 把力量改小就好了。为什么要修改这种函数？？？？？没整明白？？
        // 如果改力的话，角色需要适配推动很多物体的力， 但是修改物体移动速度的话只用保留一个力！
        if(LinearVelocity.Length() > max_speed)
        {
            LinearVelocity = LinearVelocity.Normalized() * max_speed;
        }
    }
}
