using Godot;
using System;
using System.Collections.Generic;
public class EnemyDeath : StateNode<Enemy>
{
    public enum AnimationState:int
    {

    }
    public override void Enter()
    {
        // Bug!!!
        target.animation_state.Travel("Death");
    }
    public override void Exit()
    {

    }
    public override void _PhysicsUpdate(float delta)
    {

    }
#region HandleCollision
    
#endregion
}