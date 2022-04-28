using Godot;
using System;
public class Attack : StateNode<Player>
{
    public enum AnimationState
    {
        Error = -1,
        Running = 0,
        Stop = 1,
        Unstart = 2
    }
   public override void Enter()
    {
        // 先设置状态防止异步动画带来的问题（所有动画标志都需要在代码里置为Running，在动画轨道修改值）
        target.SetAnimationState("Attack", (int)AnimationState.Running);
        target.animation_state.Travel("Attack");
    }
    public override void Exit()
    {

    }
    public override void _PhysicsUpdate(float delta)
    {
        if(target.GetAnimationState("Attack") == (int)AnimationState.Stop)
        {
            _machine.Transition<Idle>();
            return;
        }
        target.HorizontalControllHandler();
        target.GravityControllHandler(target.FixedGravity, delta);
        target.SnapControlHandler();

    }
// 处理各种碰撞 射线，Area，Rigidbody，Kinematic
#region HandleCollision
    private void _on_Area2D_body_entered(Node body)
    {
        if(body is IHitable)
        {
            IHitable hitable = body as IHitable;
            hitable.OnHit(target, "attacked by player.");
        }
    }
#endregion
}
