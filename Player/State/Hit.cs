using Godot;
using System;
using System.Collections.Generic;
public class Hit : StateNode<Player>
{
    public enum AnimationState:int
    {
        Error = -1,
        Running = 0,
        Stop = 1,
        Unstart = 2
    }
    public override void Enter()
    {
        // 效果：
        // 1. 闪烁 2. 击退 3. 停止动画
        target.animation_state.Travel("Hit");
        target.SetAnimationState("Hit", (int)AnimationState.Running);
        SoundManager.PlaySound("Hit");
    }
    public override void Exit()
    {
    }
    public override void _PhysicsUpdate(float delta)
    {
        if(target.GetAnimationState("Hit") == (int)AnimationState.Stop)
        {
            _machine.Transition<Fall>();
            // TODO：修改好之后取消注释
            // Archive.ArchiveManager.LoadGame(0);
            target.HorizontalControllHandler();
            target.GravityControllHandler(10, delta);
            target.SnapControlHandler();

            return;
        }
    }
#region HandleCollision
    
#endregion
}
