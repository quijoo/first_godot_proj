using Godot;
using System;
using Events;
public class SpikePit : StaticBody2D, IElastic, IAttacker
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    [Export] Vector2 _ReboundSpeed = Vector2.Up * 300;
    [Export] public float Damage {get;set;} = 5f;
    public Vector2 ReboundSpeed { get=>_ReboundSpeed; }

    public Vector2 Rebound(Vector2 collision, Vector2 collided)
    {
        return _ReboundSpeed;
    }
    public void _on_Area2D_body_entered(Node body)
    {
        if(body is IHitable)
        {
            (body as IHitable).OnHit(this, "spike");
        }
    }
}
