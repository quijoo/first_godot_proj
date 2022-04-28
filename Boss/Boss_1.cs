using Godot;
using System;
using Events;
public class Boss_1 : Warrior, IHitable, IAttacker
{
    // IAttacker
    [Export] private float _Damage;
    public float Damage { get => _Damage;}
	public AnimatedSprite sprite;
	// 当玩家进入 boss 领地，boss锁定玩家
	[Export] public Area2D Land;
	private Player target;
	private bool IsLockPlayer = false;
	// 状态机对象（这里循环引用了）
	private StateMachine _machine;
	public override void _Ready()
	{
		base._Ready();
		_machine = GetNode<StateMachine>("StateMachine");
		Land = GetNodeOrNull<Area2D>("Land");
	}
	
	public override void UpdateDirection()
	{
		if(direction == Direction.RIGHT)
		{
			sprite.FlipH = false;
		}
		if(direction == Direction.LEFT)
		{
			sprite.FlipH = true;
		}
	}

#region BossApi
	// 强制转换 boss 状态, 由 boss 控制器（行为树或者普通状态机）调用
	public void Idle()
	{
		_machine.Transition<BossIdle>();
	}
	public void Run()
	{
		_machine.Transition<BossRun>();
	}
	public void TurnLeft()
	{
		direction = Direction.LEFT;
		UpdateDirection();
	}
	public void TurnRight()
	{
		direction = Direction.RIGHT;
		UpdateDirection();
	}
	public void Attack_1()
	{
		_machine.Transition<BossAttack_1>();
	}
	public void Attack_2()
	{
		_machine.Transition<BossAttack_2>();
	}
	public void Jump()
	{
		_machine.Transition<BossJump>();
	}
	public void Death()
	{
		_machine.Transition<BossDeath>();
	}
	public void Walk()
	{
		_machine.Transition<BossWalk>();
	}
    public override void RigisterEvents()
    {
        
    }
    public override void UnrigisterEvents()
    {
        
    }
#endregion
	private void _on_Land_body_entered(Node body)
	{
		// Replace with function body.
		if(!(body is Player)) return;
		GD.Print("Player Enter Boss's Body.");
		target = body as Player;
		IsLockPlayer = true;
	}

    public void OnHit(IAttacker attacker, string info)
    {
        Health -= attacker.Damage;
        GD.Print("Boss dead");
        // TOOD: boss Dead
        // 1. Event trigger
        // 2. Achievement
        // 3. Reward
    }

}



