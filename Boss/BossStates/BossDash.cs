using Godot;
using System.Collections.Generic;
public class BossDash : StateNode<Boss_1>
{
    public enum AnimationState
    {
        Error = -1,
        Running = 0,
        Stop,
    }
    [Export] private float speed = 150f;
    Position2D StartPoint;
    Position2D EndPoint;
    /// <summary>
    /// 反应距离（全局坐标系下）
    /// </summary>
    [Export] float ReactDistance;
    // 反应时间
    [Export] float ReactTime;
    // sub state
    bool isDash = false;
    bool isStop = false;
    RandomNumberGenerator random = new RandomNumberGenerator();

    public override void Enter()
    {
        // 1. 选择冲刺点（开始结束），开始点应该与玩家保持一定距离，开始点和结束点之间应该覆盖玩家的x坐标(蓄水池抽样)
        if(target.Target == null)
        {
            _machine.Transition<BossIdle>();
            return;
        }
        Node group1 = target.ScenePoints.GetNode<Node>("LandPoint/Group1");
        Node group2 = target.ScenePoints.GetNode<Node>("LandPoint/Group2");
        float d1 = group1.GetNode<Position2D>("Start").GlobalPosition.DistanceTo(target.Target.GlobalPosition);
        float d2 = group2.GetNode<Position2D>("Start").GlobalPosition.DistanceTo(target.Target.GlobalPosition);
        if(d1 > d2)
        {
            StartPoint = group1.GetNode<Position2D>("Start");
            EndPoint = group1.GetNode<Position2D>("End");
        }
        else
        {
            StartPoint = group2.GetNode<Position2D>("Start");
            EndPoint = group2.GetNode<Position2D>("End");
        }
        
        
        GetNode<Timer>("ReactTimer").WaitTime = ReactTime;
        GetNode<Timer>("ReactTimer").Start();
        // 2. 移动到起始点
        target.GlobalPosition = StartPoint.GlobalPosition;
        isDash = false; isStop = false;
    }
    public override void Exit()
    {

    }
    public override void _PhysicsUpdate(float delta)
    {
        // 4. 从起点冲刺到终点
        if(isDash && !isStop)
        {
            target.MoveTo(EndPoint, speed);
            if(target.VectorTo(EndPoint).Length() < 1f)
            {
                GetNode<Timer>("ReactTimer").WaitTime = 0.7f;
                GetNode<Timer>("ReactTimer").Start();
                isStop = true;
                return;
            }
        }
        

        // 5. 调整面向
        target.UpdateDirection();
        // target.weapon.Fire(target.Target, 10f);
    }
    public void _on_ReactTime_timeout()
    {
        if(!isDash)
        {
            target.animation_state.Travel("Run");
            // SoundManager.PlaySound("Dash");     
            isDash = true;
        }
        else
        {
            _machine.Transition<BossIdle>();
        }
    }
}