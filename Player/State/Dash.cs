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
    [Export(PropertyHint.Layers2dPhysics)] public uint DashLayer;
    [Export(PropertyHint.Layers2dPhysics)] public uint DashMask;

    private uint temp_layer;
    private uint temp_mask;
 
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
        // 关闭物理碰撞(如果这样关闭，会掉出地图边界，应该修改mask)
        // target.collision_shape.SetDeferred("disabled", true);
        temp_layer = target.CollisionLayer;
        target.CollisionLayer = DashLayer;

        temp_mask = target.CollisionMask;
        target.CollisionMask = DashMask;

        // SoundManager.dash_sound.Play();
    }
    public override void Exit()
    {
        // target.collision_shape.SetDeferred("disabled", false);
        target.CollisionLayer = temp_layer;
        target.CollisionMask = temp_mask;

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
