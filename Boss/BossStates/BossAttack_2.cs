using Godot;
using System;
using System.Collections.Generic;
public class BossAttack_2 : StateNode<Boss_1>
{
    public enum AnimationState
    {
        Error = -1,
        Running = 0,
        Stop,
    }
	public override void Enter()
	{
        base.Enter();
		target.animation_state.Travel("Attack_2");     
        target.SetAnimationState("Attack_2", 0);   
	}
	public override void _PhysicsUpdate(float delta)
	{
        base._PhysicsUpdate(delta);
        if(target.GetAnimationState("Attack_2") == (int)AnimationState.Stop)
        {
        	target.Idle();
        }
		target.GravityControllHandler(10, delta);
		target.SnapControlHandler();
	}
#region HandleCollision
	
#endregion
}