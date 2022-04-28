using Godot;
using System;
using System.Collections.Generic;
public class BossRun : StateNode<Boss_1>
{
	public override void Enter()
	{
        base.Enter();
		target.animation_state.Travel("Idle");        
	}
	public override void _PhysicsUpdate(float delta)
	{
        base._PhysicsUpdate(delta);
		target.GravityControllHandler(10, delta);
		target.SnapControlHandler();
	}
#region HandleCollision
	
#endregion
}
