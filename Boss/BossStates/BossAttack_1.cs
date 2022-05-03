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
    Position2D AttachPoint;
    [Export] float ReactTime;
    bool isAttacked = false;
    
	public override void Enter()
	{
		
        // 1. 选择一个 WallPoint
        Node WallPoint = target.ScenePoints.GetNode<Node>("WallPoint");
        int rand_index = target._random.RandiRange(0, WallPoint.GetChildCount() - 1);
        AttachPoint = WallPoint.GetChild<Position2D>(rand_index);
        target.GlobalPosition = AttachPoint.GlobalPosition;
        GetNode<Timer>("Timer").WaitTime = ReactTime;
        GetNode<Timer>("Timer").Start();
        isAttacked = false;
	}
	public override void _PhysicsUpdate(float delta)
	{

        if(target.GetAnimationState("Attack") == (int)AnimationState.Stop && isAttacked)
		{
			target.Idle();
		}
		
		target.SnapControlHandler();
        target.DirectionToTarget();
	}

    public void SpawnFireball()
    {
        if(target.Target == null)
        {
            GD.Print("No target");
            return;
        }
        target.weapon.Fire(target.Target, 0.0f);
    }

    public void _on_Timer_timeout()
    {
        target.animation_state.Travel("Attack");
        target.SetAnimationState("Attack", 0); 
        isAttacked = true;
    }
#region HandleCollision
	
#endregion
}
