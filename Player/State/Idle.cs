using Godot;
using System;

public class Idle : StateNode<Player>
{
	public override void Enter()
	{
		target.velocity.x = 0;
        target.animation_state.Stop();
		target.animation_state.Travel("Idle");
		if(!target.HasDashes())
		{
			target.ResetDashCounter(1);
		}

	}
	public override void Exit()
	{

	}
	public override void _PhysicsUpdate(float delta)
	{
		target.GravityControllHandler(target.FixedGravity, delta);
		target.SnapControlHandler();
        if(target.IsOnFloor())
        {
            target.velocity += target.GetFloorVelocity();
        }
        if(!target.IsOnFloor())
		{
			if(target.velocity.y >= 0)
			{
				_machine.Transition<Fall>();
				return;
			}
		}
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
		// handle collisions in future.
		foreach(var collision in target.Extend_GetCollison())
		{
			var collider = collision.Collider;
		}
		// handle other transitions
		if(Input.IsActionJustPressed("jump") || target.JumpBufferTimer > 0)
		{
            // jump buffer         
            _machine.SetInfo("JumpInfo", new Vector2(target.velocity.x, target.JumpSpeed));
			_machine.Transition<Jump>();
            target.JumpBufferTimer = 0;
		}
		else if(Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
		{
			_machine.Transition<Walk>();
            return;
		}
		else if(Input.IsActionJustPressed("attack"))
		{
			_machine.Transition<Attack>();
            return;
		}
		else if(Input.IsActionJustPressed("dash") && target.HasDashes())
		{
			_machine.Transition<Dash>();
            return;
		}
        
	}
}
