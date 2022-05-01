using Godot;
using System;

public class FireballWeapon : Position2D
{
    [Export] PackedScene Bullet;
    // TODO:之后添加设计反馈需要
    // [Export] private Controller Holder;
    [Export] public float Speed;
    private Node2D Parent;
    float time = 0f;
    public override void _Ready()
    {
        Parent = GetParent<Node2D>();
    }
    // public Vector2 PositionTransform(Vector2 input)
    // {
    //     return null;
    // }
    public void Fire(Node2D target, float delay)
    {
        // TOODO：调整Bullet的继承关系
        Node2D bullet = Bullet.Instance<Node2D>();
        // TODO：暂时这么写
        GetTree().Root.GetNode<Position2D>("MainMap/BulletPool").AddChild(bullet);
        (bullet as FireBall).Init(target, this, Speed);

    }
}
