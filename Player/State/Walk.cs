using Godot;
using System;
public class Walk : StateNode<Player>
{
    public override void Enter()
    {
        // Debug
        target.animation_state.Travel("Walk");
        target.particles.Emitting = true;
    }
    public override void Exit()
    {
        target.particles.Emitting = false;
    }
    public override void _PhysicsUpdate(float delta)
    {
        if(!target.IsOnFloor())
        {
            if(target.velocity.y > 0)
            {
                _machine.Transition<Fall>();
                return;
            }
        }
        // PhysicsHandler list
        target.HorizontalControllHandler();
        target.GravityControllHandler(target.FixedGravity, delta);
        target.SnapControlHandler();

        // handle collisions
        foreach(var collision in target.Extend_GetCollison())
        {
            var collider = collision.Collider;
            if (collider is SpikeClub)
            {
                target.Dead("touch spikeclub.");
            }

            else if(collider is RigidBox)
            {
                var cllider = target.GetRay(target.direction, 1).GetCollider();
                if(cllider is RigidBox)
                {
                    _machine.Transition<Push>();
                }
            }
        }
        // handle other transitions
        if(Input.IsActionJustPressed("jump") || target.JumpBufferTimer > 0)
        {   
            _machine.SetInfo("JumpInfo", new Vector2(target.velocity.x, target.JumpSpeed));
            _machine.Transition<Jump>();
            target.JumpBufferTimer = 0;
        }
        else if(CMath.ApproxEqual(target.GetHorizontalInput(), 0.01f))
        {
            _machine.Transition<Idle>();
        }
        else if(Input.IsActionPressed("attack"))
        {
            _machine.Transition<Attack>();
        }
        else if(Input.IsActionJustPressed("dash") && target.HasDashes())
        {
            _machine.Transition<Dash>();
        }
    }
// 处理各种碰撞 射线，Area，Rigidbody，Kinematic
#region HandleCollision
    
#endregion
}
