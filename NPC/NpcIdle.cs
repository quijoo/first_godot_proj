using Godot;
using System;

public class NpcIdle : StateNode<Npc>
{
    public enum AnimationState:int
    {

    }
    public override void Enter()
    {
        target.animation_state.Travel("Idle");
    }
    public override void Exit()
    {

    }
    public override void _PhysicsUpdate(float delta)
    {
        target.GravityControllHandler(100, delta);
        target.SnapControlHandler();
    }
#region HandleCollision
    
#endregion
}
