using Godot;
using System;
using System.Collections.Generic;
public class InteractiveLineMove : StateNode<InteractiveLine>
{
    public enum AnimationState:int
    {

    }
    public override void Enter()
    {
        GD.Print("[Move] ", target.Name);
        target.SetCollision(target.window.MoveLayer, target.window.MoveMask);


    }
    public override void Exit()
    {

    }
    public override void _PhysicsUpdate(float delta)
    {
        // float margin = 30f;
        Vector2 movement = target.NormalToLine(target.GetGlobalMousePosition(), target.GlobalPosition + target.shape.A, target.GlobalPosition + target.shape.B);         
        // if(Name == "Left" && movement.x > 0)
        // {
        //     movement.x = Mathf.Min(GetParent<InteractiveBorder>().window.Size.x - margin, movement.x);
        // }
        // if(Name == "Right" && movement.x < 0)
        // {
        //     movement.x = Mathf.Max(margin - GetParent<InteractiveBorder>().window.Size.x, movement.x);
        // }
        // if(Name == "Top" && movement.y > 0)
        // {
        //     movement.y = Mathf.Min(GetParent<InteractiveBorder>().window.Size.y - margin, movement.y);
        // }
        // if(Name == "Down" && movement.y < 0)
        // {
        //     movement.y = Mathf.Max(margin - GetParent<InteractiveBorder>().window.Size.y, movement.y);
        // }
        target.MoveAndSlide(movement / (delta), null, false, 4, 0.78f, true);
        if(Input.IsActionJustReleased("mouse_right"))
        {
            _machine.Transition<InteractiveLineLock>();
            return;
        }
    }
#region HandleCollision
    
#endregion
}