using Godot;
using System;

public class Dash : StateNode<Player>
{
    public enum AnimationState
    {
        Error   = -1,
        Running    = 0,
        Stop = 1,
    }
    public override void Enter()
    {
        target.num_dash -= 1;
        // 仅在开始时确定方向
        if(target.horizontal_input > 0) target.direction = Direction.RIGHT;
        else if(target.horizontal_input < 0) target.direction = Direction.LEFT;
        else if(!target.collision.Null(target.direction))
        {
            target.direction = target.direction.GetFlip();
        }
        // 1. 动画的执行是异步的，所以在 Enter 中触发的动画，可能在第一个物理帧时并没有开始播放
        // 2. 这会带来一个问题：
        //          默认 Dash == 1 表示动画结束
        //          动画未开始 Dash == 1， 那么无法区分这两个状态
        //          第一个物理帧检测到 Dash == 1， 就直接切换状态 Idle， 事实上是动画还未执行
        target.SetAnimationState("Dash", (int)AnimationState.Running);
        target.animation_state.Travel("Dash");
        // SoundManager.dash_sound.Play();
    }
    public override void Exit()
    {
    }
    public override void _PhysicsUpdate(float delta)
    {
        target.LockVerticalMovement();
        target.DashControlHandler();
        target.SnapControlHandler();
        if(target.GetAnimationState("Dash") == (int)AnimationState.Stop)
        {
            if(target.collision.AnyDected(Direction.DOWN))
            {
                _machine.Transition<Idle>();
                return;
            }
            if(target.collision.Null(Direction.DOWN))
            {
                target.velocity.x = 0f;
                _machine.Transition<Fall>();
                return;
            }
        }
        if(target.collision.All(target.direction) && target.collision.Null(Direction.DOWN))
        {
            _machine.Transition<Attach>();
            return;
        }
    }
}
