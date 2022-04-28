using Godot;
using System;
using System.Collections.Generic;
public class Attach : StateNode<Player>
{   
    // public enum AnimationState:int
    // {

    // }
    private float attack_away_timer = 0f;
    public override void Enter()
    {
        target.animation_state.Travel("Attach");
        target.velocity.y = 0;
        // 重置Dash
        if(!target.HasDashes())
		{
			target.ResetDashCounter(1);
		}
    }
    public override void _Update(float delta)
    {
        base._Update(delta);
    }
    public override void _PhysicsUpdate(float delta)
    {
        if(Input.IsActionJustPressed("jump") || target.JumpBufferTimer > 0)
        {
            // jump buffer
            target.JumpBufferTimer = 0;
            _machine.Transition<WallJump>();
            return;
        }
        if(Input.IsActionJustPressed("dash") && target.HasDashes())
        {
            _machine.Transition<Dash>();
            return;
        }
        if((target.horizontal_input > 0 && target.direction == Direction.LEFT) 
                || target.horizontal_input < 0 && target.direction == Direction.RIGHT)
        {
            // 处理墙壁的滞粘时间
            attack_away_timer += delta;
            if(attack_away_timer > 0.2)
            {
                _machine.Transition<Fall>();
                return;
            }
        }
        else
        {
            attack_away_timer = 0f;
        }

        if(!target.collision.All(target.direction))
        {
            _machine.Transition<Fall>();
            return;
        }
        if(target.collision.All(Direction.DOWN))
        {
            _machine.Transition<Idle>();
            return;
        }
        if(target.collision.AnyDected(Direction.LEFT)) target.direction = Direction.LEFT;
        if(target.collision.AnyDected(Direction.RIGHT)) target.direction = Direction.RIGHT;
        target.UpdateDirection();
        target.GravityControllHandler(target.WallSlideGravity, delta);
        target.SnapControlHandler();

    }
    public Vector2 GetFlipH(Vector2 v, bool flag)
    {
        if(flag)
        {
            return new Vector2(- v.x, v.y);
        }
        else
        {
            return new Vector2(v.x, v.y);
        }
    }
    public Direction HorizontalInput()
    {
        if(Mathf.Abs(target.horizontal_input) < 0.1)
        {
            return target.direction.GetFlip();
        }
        else if(target.horizontal_input < 0) 
        {
            return Direction.LEFT;
        }
        else
        {
            return Direction.RIGHT;
        }
    }
#region HandleCollision
    
#endregion
}