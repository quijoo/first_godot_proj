using Godot;
using System;
using System.Collections.Generic;
public class Fall : StateNode<Player>
{
    enum AnimationState : int
    {
        Error   = -1,
        Stop    = 0,
        Running = 1,
    }
    public override void Enter()
    {
        // Debug
        target.animation_state.Travel("Fall");
    }
    public override void Exit()
    {

    }
    public override void _PhysicsUpdate(float delta)
    {
        // PhysicsHandler list
        target.HorizontalControllHandler(false);
        target.GravityControllHandler(target.FixedGravity, delta);
        // 限制最大下落速度
        target.velocity.y = target.velocity.y > target.MaxFallSpeed ? target.MaxFallSpeed : target.velocity.y;
        target.SnapControlHandler();

        if(target.IsOnFloor())
        {
            _machine.Transition<Idle>();
            return;
        }
        
        if(target.collision.All(target.direction) && target.collision.Null(Direction.DOWN))
        {
            _machine.Transition<Attach>();
            return;
        }

        if(Input.IsActionJustPressed("jump")&& target.GraceTimer > 0)
        {
            _machine.Transition<Jump>();
            return;               
        }
        if(Input.IsActionJustPressed("attack"))
        {
            if(Input.GetActionStrength("ui_down") > 0.5)
            {
                _machine.Transition<DownAttack>();
            }
            else
            {
                _machine.Transition<Attack>();
            }
            return;
        }
        // handle collisions
        foreach(var collision in target.Extend_GetCollison())
        {
            var collider = collision.Collider;

            if (collider is SpikeClub)
            {
                target.Dead("Spike club");
            }
        }

        // 2. 处理踩到 Enemy


        // handle other transitions
        if(Input.IsActionJustPressed("dash") && target.HasDashes())
        {
            _machine.Transition<Dash>();
        }
    }
// 处理各种碰撞 射线，Area，Rigidbody，Kinematic
#region HandleCollision
    
#endregion

}