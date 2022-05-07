using Godot;
using System;

using System.Collections.Generic;
public class InteractiveLineLock : StateNode<InteractiveLine>
{
    Vector2 cachePosition = Vector2.Zero;
    public enum AnimationState:int
    {

    }
    public override void Enter()
    {
        target.velocity = Vector2.Zero;
        target.IsLocked = true;
        cachePosition = target.Position;
        GD.Print("[Lock] ", target.Name);
        target.SetCollision(target.window.LockLayer, target.window.LockMask);
    }
    public override void Exit()
    {
        target.IsLocked = false;
    }
    public override void _PhysicsUpdate(float delta)
    {
        target.velocity = target.MoveAndSlide(Vector2.Zero, null, false, 4, 0.78539f, false);
        if(cachePosition != null) target.Position = cachePosition;

        if(target.ColllisionCount == 0)
        {
            _machine.Transition<InteractiveLineFollow>();
            return;
        }
        if(Input.IsActionJustPressed("mouse_right") && target.NormalToLine(target.GetGlobalMousePosition(), target.GlobalPosition + target.shape.A, target.GlobalPosition + target.shape.B).Length() < 14f)
        {
            // 进入拖动状态
            _machine.Transition<InteractiveLineMove>();
            return;
        }
    }
}
