using Godot;
using System;

public class FireBall : KinematicBody2D, IElastic
{
    private Area2D hurt_area;
    private AnimatedSprite sprite;
    [Export] private Vector2 velocity = Vector2.Right;
    // IElastic
    [Export] Vector2 _ReboundSpeed = Vector2.Up * 200;
    public Vector2 ReboundSpeed { get=>_ReboundSpeed; }
    public Vector2 Rebound(Vector2 p1, Vector2 p2)
    {
        return _ReboundSpeed;
    }
    public override void _Ready()
    {
        hurt_area = GetNode<Area2D>(nameof(Area2D));
        sprite = GetNode<AnimatedSprite>(nameof(AnimatedSprite));
    }
    

    public override void _PhysicsProcess(float delta)
    {
        foreach(var collision in this.Extend_GetCollison())
        {
            if(collision.Collider is Player)
            {
                // process player collision
            }
            else if(collision.Collider is TileMap)
            {
                // 1. 播放爆炸动画
                // 2. 播放碰撞特效
                // 3. 等待动画结束 await
                // 4. 删除对象queue_free
                QueueFree();
            }
            // GD.Print(collision.Collider);
        }
        velocity = MoveAndSlide(velocity);
    }
}
