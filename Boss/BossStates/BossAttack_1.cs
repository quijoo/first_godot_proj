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
        Node b1 = fire_ball.Instance();
        Node b2 = fire_ball.Instance();
        Node b3 = fire_ball.Instance();
        
        Vector2 p1 = target.GetNode<Position2D>("FireballPosition_1").Position;
        Vector2 p2 = target.GetNode<Position2D>("FireballPosition_2").Position;
        Vector2 p3 = target.GetNode<Position2D>("FireballPosition_3").Position;

        target.GetParent().AddChild(b1);
        target.GetParent().AddChild(b2);
        target.GetParent().AddChild(b3);

        ((FireBall)b1).Position = ((Node2D)(target.GetParent())).ToLocal(target.ToGlobal(p1));
        ((FireBall)b2).Position = ((Node2D)(target.GetParent())).ToLocal(target.ToGlobal(p2));
        ((FireBall)b3).Position = ((Node2D)(target.GetParent())).ToLocal(target.ToGlobal(p3));
    }
#region HandleCollision
	
#endregion
}
