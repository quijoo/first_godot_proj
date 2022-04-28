using Godot;
using System;
using System.Collections.Generic;
public class DownAttack : StateNode<Player>
{
    public enum AnimationState:int
    {
        Error = -1,
        Running = 0,
        Stop = 1,
        Unstart = 2
    }
    
    public override void Enter()
    {
        target.SetAnimationState("DownAttack", (int)AnimationState.Running);
        target.animation_state.Travel("DownAttack");
    }
    public override void Exit()
    {

    }
    public override void _PhysicsUpdate(float delta)
    {
        // transition
        if(target.GetAnimationState("DownAttack") == (int)AnimationState.Stop)
        {
            _machine.Transition<Fall>();
        }
        if(target.velocity.y < 0)
        {
            target.HorizontalControllHandler();
        }
        // target.HorizontalControllHandler();
        target.GravityControllHandler(target.FixedGravity, delta);
        target.SnapControlHandler();
    }
#region HandleCollision
    private void _on_Area2D_body_entered(Node body)
    {
        GD.Print("是谁触发了？");
        if(body is IElastic)
        {
            IElastic e = body as IElastic;
            target.velocity = e.Rebound(Vector2.Zero, target.Position);
            target.ResetDashCounter(1);
        }
        if(body is IHitable)
        {
            IHitable e = body as IHitable;
            e.OnHit(target, "down attack");
        }
    }

#endregion
}