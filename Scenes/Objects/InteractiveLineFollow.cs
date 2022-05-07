using Godot;
using System;
using System.Collections.Generic;
public class InteractiveLineFollow : StateNode<InteractiveLine>
{
    public enum AnimationState:int
    {

    }
    public override void Enter()
    {
        GD.Print("[Follow] ", target.Name);
        target.SetCollision(target.window.FollowLayer, target.window.FollowMask);

    }
    public override void Exit()
    {
    }
    public override void _PhysicsUpdate(float delta)
    {
        if(BasicFollowCamera.Target.IsInsideTree())
        {
            target.velocity = (BasicFollowCamera.Target.GlobalPosition + target.DefaultBias - target.GlobalPosition) * 20;
            target.velocity = target.MoveAndSlide(target.velocity, null, false, 4, 0.78539f, false);
        }

        if(target.ColllisionCount != 0)
        {
            _machine.Transition<InteractiveLineLock>();
            return;
        }
    }
#region HandleCollision
    
#endregion
}
