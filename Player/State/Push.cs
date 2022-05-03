using Godot;
using System;

public class Push : StateNode<Player>
{
    public override void Enter()
    {
        target.animation_state.Travel("Push");
    }
    public override void Exit()
    {

    }
    public override void _PhysicsUpdate(float delta)
    {
        if(!target.collision.AnyUndected(Direction.DOWN) && target.velocity.y > 0)
        {
            _machine.Transition<Fall>();
            return;
        }
        
        // PhysicsHandler list
        target.HorizontalControllHandler();
        target.GravityControllHandler(target.FixedGravity, delta);
        target.SnapControlHandler();

        // handle collisions
        var ray = target.GetRay(target.direction, 1);
        if(ray.IsColliding())
        {
            var collider = ray.GetCollider();
            var normal = ray.GetCollisionNormal();
            GD.Print("occur collision ", collider is RigidBox);

            if(collider is RigidBox)
            {
                normal.y = 0f;
                ((RigidBox)collider).ApplyCentralImpulse(-normal * target.rigid_push);
            }
        }
        else
        {
            _machine.Transition<Idle>();
        }
        if(!Input.IsActionPressed("ui_left") && !Input.IsActionPressed("ui_right"))
        {
            _machine.Transition<Idle>();
        }
        if(Input.IsActionJustPressed("jump"))
        {
            _machine.SetInfo("JumpInfo", new Vector2(target.velocity.x, target.JumpSpeed));
            _machine.Transition<Jump>();
        }
    }
// 处理各种碰撞 射线，Area，Rigidbody，Kinematic
#region HandleCollision
    
#endregion

}
