using Godot;
using Godot.Collections;
using System;
using Events;
public partial class Boss_1 : Warrior, IHitable, IAttacker, IElastic, Archivable
{
	// IAttacker
	[Export] private float _Damage;
	public float Damage { get => _Damage;}
	public Sprite sprite;
	// 当玩家进入 boss 领地，boss锁定玩家
	[Export] public NodePath Land;
    [Export] public NodePath gate_path; 
	private Player target;
	// 状态机对象（这里循环引用了）
	private StateMachine _machine;
	private Console _wrapper;
	// IElastic
	[Export] Vector2 _ReboundSpeed = Vector2.Up * 300;
	public Vector2 ReboundSpeed { get=>_ReboundSpeed; }

	public Vector2 Rebound(Vector2 collision, Vector2 collided)
	{
		return _ReboundSpeed;
	}
	// Weapon
	public FireballWeapon weapon;
    // 这个可以用编辑器脚本来设置
    // 定位相关
    public Node ScenePoints;
    [Export] public NodePath scene_point_path;
    // 击败后的 fadeback（是一些回调，之后统一封装）
    // 可以写成一个 fadeback 类，然后定义很多回调，boss死后直接执行所有回调即可
    [Export] NodePath FaildTile;
    [Export] NodePath Gate_1;
    [Export] NodePath Gate_2;

	public override void _Ready()
	{
		base._Ready();
		_machine = GetNode<StateMachine>("StateMachine");
		Area2D LandArea = GetNode<Area2D>(Land);
        LandArea.Connect("body_entered", this, "_on_Land_body_entered");
        LandArea.Connect("body_exited", this, "_on_Land_body_exited");


		_wrapper = GetTree().Root.GetNode<Console>("CSharpConsole");        
		_wrapper.AddCommand("boss_attack", this, nameof(Attack))
				.SetDescription("let boss use enter attack_%index% attack")
				.AddArgument("index", Variant.Type.Int)
				.Register();

		_wrapper.AddCommand("boss_dash", this, nameof(Dash))
				.SetDescription("let boss dash between few points")
				.Register();

		sprite = GetNode<Sprite>("Sprite");
		// 暂时放到sprite下
		weapon = GetNode<FireballWeapon>("Sprite/FireballWeapon");
        // boss场景点位设置
        ScenePoints = GetNode<Node>(scene_point_path);

	}
	
	public void DirectionToTarget()
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
    public override void UpdateDirection()
	{
        if(velocity.x < 0)
        {
            sprite.FlipH = true;
            direction = Direction.LEFT;
        }
        if(velocity.x > 0)
        {
            sprite.FlipH = false;
            direction = Direction.RIGHT;
        }
	}

	private void _on_Land_body_entered(Node body)
	{
		// Replace with function body.
		if(!(body is Player)) return;
		target = body as Player;
		LookAt(target);
        GetNode<IGate>(gate_path).Lock();
	}
	private void _on_Land_body_exited(Node body)
	{
		if(Target == body)
		{
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
        SoundManager.PlaySound("Attack");
		Health -= attacker.Damage;
        // _machine.SetInfo("prev_state", _machine.state.Name);
		// _machine.Transition<BossHit>();
        if(Health < 0)
        {
            Dead("killed");
        }
        animation_state.Travel("Hit");        

		// TOOD: boss Dead
		// 1. Event trigger
		// 2. Achievement
		// 3. Reward
	}
    public override void Dead(string reason)
    {
        base.Dead(reason);
        GetNode<NullableTile>(FaildTile).QueueFree();
        GetNode<IGate>(Gate_1).UnLock();
        GetNode<IGate>(Gate_1).UnLock();
        QueueFree();
    }
    public void Save(in Dictionary data)
    {      
        data["PosX"] = Position.x; 
        data["PosY"] = Position.y; 
        data["Health"] = Health;
        data["_Damage"] = _Damage;
        data["gate_path"] = gate_path.ToString();
        data["_ReboundSpeed.x"] = _ReboundSpeed.x;
        data["_ReboundSpeed.y"] = _ReboundSpeed.y;

        data["scene_point_path"] = scene_point_path.ToString();
        data["Gate_1"] = Gate_1.ToString();
        data["Gate_2"] = Gate_2.ToString();
        data["FaildTile"] = FaildTile.ToString();
        data["Land"] = Land.ToString();
    }
    public void Load(in Dictionary data)
    {
        Position = new Vector2((float)data["PosX"], (float)data["PosY"]);
        Health = (float)data["Health"];
        _Damage = (float)data["_Damage"];
        gate_path = new NodePath((string)data["gate_path"]);
        _ReboundSpeed.x = (float)data["_ReboundSpeed.x"];
        _ReboundSpeed.y = (float)data["_ReboundSpeed.y"];

        scene_point_path = new NodePath((string)data["scene_point_path"]);
        Gate_1 = new NodePath((string)data["Gate_1"]);
        Gate_2 = new NodePath((string)data["Gate_2"]);

        FaildTile = new NodePath((string)data["FaildTile"]);
        Land = new NodePath((string)data["Land"]);
        // 状态机默认就是Idle，这里节点还未准备好，
        // GetNode<StateMachine>("StateMachine").Transition<BossIdle>();
    }
}





