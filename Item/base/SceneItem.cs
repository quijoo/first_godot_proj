using Godot;
using System;

public class SceneItem : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public Vector2 velocity;
    private RayCast2D ray;
    private Area2D area;
    private Sprite sprite;
    public override void _Ready()
    {
        velocity = Vector2.Up * 10;
        ray = GetNode<RayCast2D>(nameof(RayCast2D));
        area = GetNode<Area2D>(nameof(Area2D));
        sprite = GetNode<Sprite>(nameof(Sprite));
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        // 如果解除到地面则往下掉
        if(ray.IsColliding() && ray.GetCollider() is TileMap)
        {
            return;
        }
        velocity.y += 80 *delta;
        velocity = MoveAndSlideWithSnap(
			velocity,    Vector2.Down,     Vector2.Up,
			true, 4,            12, false);
    }
    public void Enable()
    {
        Visible = true;
        ray.Enabled = true;
        sprite.Visible = true;
        area.SetDeferred("monitorable", true);
        area.SetDeferred("monitoring", true);
    }
    public void Disable()
    {
        SetDeferred("visible", false);
        ray.SetDeferred("enable", false);
        sprite.SetDeferred("visible", false);
        area.SetDeferred("monitorable", false);
        area.SetDeferred("monitoring", false);

    }
    private void _on_Area2D_body_entered(Node body)
    {
        if(body is Player)
        {
            (body as Player).Pick(GetParent() as Item<Node>);
        }
    }
}
