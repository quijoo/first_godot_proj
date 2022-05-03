using Godot;
using System;
using System.Collections.Generic;
public class BossIdle : StateNode<Boss_1>
{
	private Node2D IdlePosition;
	public override void Enter()
	{
		target.velocity = Vector2.Zero;
		target.animation_state.Travel("Idle"); 
		IdlePosition = target.ScenePoints.GetNode<Position2D>("IdlePoint");  
		target.GlobalPosition = IdlePosition.GlobalPosition;
		target.velocity = Vector2.Zero;
		timer = 0f;
	}
	public override void Exit()
	{

	}
	// 覆盖父类方法， 仅仅处理重力
	float timer = 0f;
 	public override void _PhysicsUpdate(float delta)
	{

		if(target.Target == null) return;
		// target.GravityControllHandler(10, delta);
		target.SnapControlHandler();
		target.UpdateDirection();
		if(timer < 0.9)
		{
		   timer += delta;
		   return; 
		}
		if(target._random.RandiRange(0, 10) > 3)
		{
			_machine.Transition<BossDash>();
		}
		else
		{
			_machine.Transition<BossAttack_1>();
		}
	}
#region HandleCollision
	
#endregion
}
