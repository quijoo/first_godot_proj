using Godot;
using System;
using System.Collections.Generic;
public class BossIdle : StateNode<Boss_1>
{
	public override void Enter()
	{
		target.animation_state.Travel("Idle");        
	}
	public override void Exit()
	{

	}
	// 覆盖父类方法， 仅仅处理重力
	public override void _PhysicsUpdate(float delta)
	{
		target.GravityControllHandler(10, delta);
		target.SnapControlHandler();
	}
#region HandleCollision
	
#endregion
}
