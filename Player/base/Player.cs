using Godot;
using System;
using System.Collections.Generic;
using Events;

public partial class Player : Warrior, IAttacker, IHitable, Archivable
{
    // IAttacker
    [Export] private float _Damage;
    public float Damage { get => _Damage;}
	// Kinematic parameters
	[Export] public int num_dash    = 1;
    [Export] public int dash_speed = 300;
	[Export] public Vector2 rigid_push  = new Vector2(255, 0);
	
	// Trail effect (move to dash state).
	[Export(PropertyHint.File, "*.tscn")] private String TrailPath;
    [Export] private PackedScene packed_trail;
    public float horizontal_input { get; private set;}
    // 获取粒子对象
    public CPUParticles2D particles;

    // player attack related.
    public struct AttackArea
    {
        public Area2D horizontal_area;
        public Area2D down_area;
        public AttackArea(Area2D h_area, Area2D d_area)
        {
            horizontal_area = h_area;
            down_area = d_area;
        }
    }
    public AttackArea attack {get; private set;}
	// 残影相关
    List<string> properties = new List<string>()
			{
				"hframes",
				"vframes",
				"frame",
				"texture",
				"global_position",
				"flip_h"
			};
    // sprite
    public Sprite sprite;
	// AnimationTree relate.
    Dictionary<string, int> AnimationStateDict = new Dictionary<string, int>();
    // 隐藏成员
    public new enum AnimationState : int
    {
        Error = -1,
        Running = 0,
        Stop,
    }
    // 背包对象
    public Bag bag;

    public override void _Ready()
	{
        base._Ready();
		// GetNode<CollisionShape2D>("HitboxPosition/Hitbox/CollisionShape2D").SetDeferred("disabled", true);
        sprite    = GetNode<Sprite>(nameof(Sprite));
        particles = GetNode<CPUParticles2D>(nameof(CPUParticles2D));
        // get attack area info
        attack = new AttackArea(
                GetNode<Area2D>("AttackArea/HorizontalAttackArea"),
                GetNode<Area2D>("AttackArea/DownAttackArea")
            );
        DisableDownArea();
        DisableHorizontalArea();
        // 等待所有静态节点加载完毕, 这里需要等待根节点加载完毕，那么
        // just for test
        UIManager.hud.Health.Set((int)Health);
        // bag init
        bag = GetNode<Bag>("Bag");
        BasicFollowCamera.Follow(this);
	}

    public override void _Process(float delta)
    {
        horizontal_input = GetHorizontalInput();   
    }

}


