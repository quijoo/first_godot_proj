using Godot;
using Godot.Collections;
using System;
using Events;
public partial class Boss_1 : Warrior, IHitable, IAttacker, IElastic
{
	// IAttacker
	[Export] private float _Damage;
	public float Damage { get => _Damage;}
	public Sprite sprite;
	// 当玩家进入 boss 领地，boss锁定玩家
	public Area2D Land;
	private Player target;
	// 状态机对象（这里循环引用了）
	private StateMachine _machine;
	private Console _wrapper;
	public Array<RayCast2D> NavigationRay = new Array<RayCast2D>();
	// IElastic
	[Export] Vector2 _ReboundSpeed = Vector2.Up * 300;
	public Vector2 ReboundSpeed { get=>_ReboundSpeed; }

	public Vector2 Rebound(Vector2 collision, Vector2 collided)
	{
		return _ReboundSpeed;
	}
	// Weapon
	public FireballWeapon weapon;

	public override void _Ready()
	{
		base._Ready();
		_machine = GetNode<StateMachine>("StateMachine");
		Land = GetNodeOrNull<Area2D>("Land");

		_wrapper = GetTree().Root.GetNode<Console>("CSharpConsole");        
		_wrapper.AddCommand("boss_attack", this, nameof(Attack))
				.SetDescription("let boss use enter attack_%index% attack")
				.AddArgument("index", Variant.Type.Int)
				.Register();

		_wrapper.AddCommand("boss_dash", this, nameof(Dash))
				.SetDescription("let boss dash between few points")
				.Register();

		sprite = GetNode<Sprite>("Sprite");
		foreach(RayCast2D ray in GetNode<Node>("Navigation/Ray").GetChildren())
		{
			NavigationRay.Add(ray);
		}
		// 暂时放到sprite下
		weapon = GetNode<FireballWeapon>("Sprite/FireballWeapon");
	}
	
	public override void UpdateDirection()
	{
		if(Target != null)
		{
		    var targ_posi = Target.GetParent<Node2D>().ToGlobal(Target.Position);
		    var self_posi = GetParent<Node2D>().ToGlobal(Position);
		    if(targ_posi.x < self_posi.x)
		    {
		        sprite.FlipH = true;
		        direction = Direction.LEFT;
		    }
		    if(targ_posi.x > self_posi.x)
		    {
		        sprite.FlipH = false;
		        direction = Direction.RIGHT;
		    }
		}
	}

	private void _on_Land_body_entered(Node body)
	{
		// Replace with function body.
		if(!(body is Player)) return;
		GD.Print("Player Enter Boss's Land.");
		target = body as Player;
		LookAt(target);
	}
	private void _on_Land_body_exited(Node body)
	{
		if(Target == body)
		{
			GD.Print("Target Exit Boss's Land.");
			Ignore();
		}
	}

	private void _on_Hitbox_body_entered(Node body)
	{
		if(body is IHitable)
		{
			(body as IHitable).OnHit(this, "touch boss");
		}
	}
	public void OnHit(IAttacker attacker, string info)
	{
		Health -= attacker.Damage;
		_machine.Transition<BossHit>();
		// TOOD: boss Dead
		// 1. Event trigger
		// 2. Achievement
		// 3. Reward
	}

}



