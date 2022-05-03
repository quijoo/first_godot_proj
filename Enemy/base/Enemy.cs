using Events;
using Godot;
using System;
/// 包含所有的字段定义以及生命周期函数
public partial class Enemy : Warrior, IElastic, IHitable, IAttacker
{
    // IAttacker
    [Export] public float Damage { get; set;} = 1f;
    // IElastic
    [Export] Vector2 _ReboundSpeed = Vector2.Up * 200;
    public Vector2 ReboundSpeed { get => _ReboundSpeed; }
    // Controller 相关
    public Sprite sprite;
    // 生命周期函数
    public override void _Ready()
    {
        base._Ready();
        sprite = GetNodeOrNull<Sprite>(nameof(Sprite));
    }
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
    }
    public override void _EnterTree()
    {
        base._EnterTree();
    }
    public override void _ExitTree()
    {
        base._ExitTree();
    }    
}
