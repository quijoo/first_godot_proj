using Godot;
using System;

public class Jump : StateNode<Player>
{

    public override void Enter()
    {
        // Debug
        target.velocity.y = target.JumpSpeed;
        target.animation_state.Travel("Jump");
        SoundManager.PlaySound("Jump");
    }
    public override void Exit()
    {

    }
    public override void _PhysicsUpdate(float delta)
    {
        if(target.velocity.y > 0)
        {
            _machine.Transition<Fall>();
            return;
        }
        // PhysicsHandler list
        target.HorizontalControllHandler();

        // 长按短按
        if(Input.IsActionPressed("jump"))
            target.GravityControllHandler(target.FixedGravity, delta);
        else target.GravityControllHandler(target.FixedGravity * target.LowJumpMultiplier, delta);
        target.SnapControlHandler();

        // handle collisions
        foreach(var collision in target.Extend_GetCollison())
        {
            var collider = collision.Collider;
            if(collider is SpikeClub)
            {
                target.Dead("touch spikeclub when jump.");
            }
        }
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
