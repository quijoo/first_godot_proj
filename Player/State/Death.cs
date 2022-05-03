using Godot;
using System;

public class Death : StateNode<Player>
{

    private AnimationPlayer anima;
    public enum AnimationState
    {
        Error = -1,
        Running = 0,
        Stop = 1,
        Unstart = 2
    }
    public override void Enter()
    {
        // disable collisions
        // TODO：需求： 死亡时角色播放死亡动画， 其余物体停止动画
        // 分析： 死亡时的暂停不能直接修改 TimeScale，会导致 Player 的死亡动画不能播放，导致无法调用复活方法（死锁）
        // 解决方案：为每一类物体设置一个管理器类，每类管理器可以控制当前类别的对象的一般行为（Spawn，death，move，suspend/play）
        // Player死亡时，停止某些物件的动画，停止的时候可以使用 Mask 来进行类别筛选
        
        target.animation_state.Travel("Death");
        target.SetAnimationState("Death", (int)AnimationState.Running);
        // SoundManager.dash_sound.Play();

        // target.GetNode<CollisionShape2D>(nameof(CollisionShape2D)).SetDeferred("disabled", true);
        // target.QueueFree();
    }
    public override void Exit()
    {

    }
    public override void _PhysicsUpdate(float delta)
    {
        // 重置
        target.DisableDownArea();
        target.DisableHorizontalArea();

        if(target.GetAnimationState("Death") == (int)AnimationState.Stop)
        {
            target.RestartLevel();
            return;
        }

    }
}
