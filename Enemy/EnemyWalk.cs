using Godot;
using System;
using System.Collections.Generic;
public class EnemyWalk : StateNode<Enemy>
{
    public enum AnimationState:int
    {
        Error = -1,
        Stop = 0,
        Runing = 1,
    }
    public Godot.Object CacheGround = null;

    public override void Enter()
    {
        target.animation_state.Travel("Walk");
    }
    public override void _PhysicsUpdate(float delta)
    {
        
        // foreach(var collision in target.Extend_GetCollison())
        // {
        //     var collider = collision.Collider;

        //     if(collider is Player)
        //     {
        //         ((Player)collider).Dead("touch enemy");
        //     }
        // }
        if(target.collision.AnyDected(target.direction)) 
        {
            // just use for testing EventSystem. we dont care about enemy's collision.
            EventManager.Send(target, new Events.TouchWallEvent(target.direction));
            Direction cached_direction = target.direction;
            target.direction = target.direction.GetFlip();
            target.UpdateDirection();
            if(target.collision.AnyDected(cached_direction.GetFlip()))
            {
                _machine.Transition<EnemyIdle>();
                return;
            }
        }
        if(!target.collision.CornerDown(target.direction))
        {   
            target.direction = target.direction.GetFlip();
        } 

        target.UpdateDirection();
        target.HorizontalControlHandler(delta);
        target.GravityControllHandler(target.FixedGravity, delta);
        target.SnapControlHandler();
    }
}