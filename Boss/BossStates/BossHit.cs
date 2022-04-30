using Godot;
using System;
using System.Collections.Generic;
public class BossHit : StateNode<Boss_1>
{
    public enum AnimationState
    {
        Error = -1,
        Running = 0,
        Stop,
    }
    public override void Enter()
    {
        // GD.Print();
        target.SetAnimationState("BossHit", (int)AnimationState.Running);
        target.animation_state.Travel("Hit");        
        // target.velocity.
    }
    public override void Exit()
    {
        
    }
    public override void _PhysicsUpdate(float delta)
    {
        if(target.GetAnimationState("BossHit") == (int)AnimationState.Stop)
        {
            target.Idle();
        }
    }
#region HandleCollision
    
#endregion
}