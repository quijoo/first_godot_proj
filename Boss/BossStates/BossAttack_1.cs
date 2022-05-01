using Godot;
using System;
using System.Collections.Generic;
public class BossAttack_1 : StateNode<Boss_1>
{
    [Export] private PackedScene fire_ball; 
    private List<Node> fire_ball_list = new List<Node>();
    public enum AnimationState
    {
        Error = -1,
        Running = 0,
        Stop,
    }
	public override void Enter()
	{
		target.animation_state.Travel("Attack");
        target.SetAnimationState("Attack", 0);
        // TODO: 添加攻击特效
        // 1. 生成法术球（特效，碰撞，反馈）
        // 由动画的方法调用轨道调用
        // 2. 等待动画完成转移到 Idle 状态
        // 3. 攻击不可中断 
        // 4. 法术球可下劈跳（与关卡结合到达本来到不了的地方）      
	}
	public override void _PhysicsUpdate(float delta)
	{
        if(target.GetAnimationState("Attack") == (int)AnimationState.Stop)
		{
			target.Idle();
		}
		target.GravityControllHandler(10, delta);
		target.SnapControlHandler();
	}

    public void SpawnFireball()
    {
        if(target.Target == null)
        {
            GD.Print("No target");
            return;
        }
        target.weapon.Fire(target.Target, 0.4f);
    }
#region HandleCollision
	
#endregion
}
