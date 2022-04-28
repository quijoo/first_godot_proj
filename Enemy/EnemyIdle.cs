using Godot;
using System;
using System.Collections.Generic;
public class EnemyIdle : StateNode<Enemy>
{
    public enum AnimationState:int
    {
        Error = -1,
        Stop = 0,
        Runing = 1,
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
        if(target.collision.Null(Direction.RIGHT) || target.collision.Null(Direction.LEFT))
        {
            _machine.Transition<EnemyWalk>();
            return;
        }

    }
#region HandleCollision
    
#endregion
}