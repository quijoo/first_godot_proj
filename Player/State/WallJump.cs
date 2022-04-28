using Godot;
using System;

public class WallJump : StateNode<Player>
{
    private float CachedPositionX;
    private float time;
    public override void Enter()
    {
        // Debug
        int wall_direction = target.collision.All(Direction.RIGHT) ? 1 : -1;
        target.direction = target.direction.GetFlip();
        target.UpdateDirection();
        if(target.horizontal_input * wall_direction > 0)
        {
            // 爬墙
            target.velocity.x = target.WallClimbSpeed.x * wall_direction;
            target.velocity.y = target.WallClimbSpeed.y;
        }
        else if(target.horizontal_input * wall_direction < 0)
        {
            // 超级蹬墙跳
            target.velocity.x = target.SuperWallJumpSpeed.x * wall_direction;
            target.velocity.y = target.SuperWallJumpSpeed.y;
        }
        else if(target.horizontal_input == 0)
        {
            // 蹬墙跳
            target.velocity.x = target.WallJumpSpeed.x * wall_direction;
            target.velocity.y = target.WallJumpSpeed.y;
        }
        target.animation_state.Travel("Jump");
        SoundManager.PlaySound("Jump");
        time = 0f;
    }
    public override void _PhysicsUpdate(float delta)
    {
        if(target.collision.AnyDected(Direction.DOWN))
        {
            _machine.Transition<Idle>();
            return;
        }
        if(target.velocity.y > 0)
        {
            _machine.Transition<Fall>();
            return;
        }
        if(time > target.WallJumpLockTime) 
        {
            target.HorizontalControllHandler(false);
        }
        target.GravityControllHandler(target.FixedGravity, delta);
        target.SnapControlHandler();
        time += delta;
    }
// 处理各种碰撞 射线，Area，Rigidbody，Kinematic
#region HandleCollision
    
#endregion
}
