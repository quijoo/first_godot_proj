using Godot;
using System;

public partial class Boss_1
{
public void Idle()
	{
		_machine.Transition<BossIdle>();
	}
	public void Run()
	{
		_machine.Transition<BossRun>();
	}
	public void TurnLeft()
	{
		direction = Direction.LEFT;
		UpdateDirection();
	}
	public void TurnRight()
	{
		direction = Direction.RIGHT;
		UpdateDirection();
	}
	public void Attack(int index)
	{
        if(index == 1)
		    _machine.Transition<BossAttack_1>();
        else if(index == 2)
	    	_machine.Transition<BossAttack_2>();
	}
    public void Dash()
	{
        _machine.Transition<BossDash>();
	}
	public void Jump()
	{
		_machine.Transition<BossJump>();
	}
	public void Death()
	{
		_machine.Transition<BossDeath>();
	}
	public void Walk()
	{
		_machine.Transition<BossWalk>();
	}
    public override void RigisterEvents()
    {
        
    }
    public override void UnrigisterEvents()
    {
        
    }
    public void MoveTo(Node2D _target, float speed)
    {
        if(_target == null)
        {
            GD.Print("target is null");
            return;
        }
        Vector2 move_direction = this.VectorTo(_target);
        if(move_direction.x > 0) direction = Direction.RIGHT;
        if(move_direction.x < 0) direction = Direction.LEFT;
        UpdateDirection();
        velocity = speed * move_direction.Normalized();
        velocity = MoveAndSlide(velocity);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_target">目标位置(局部坐标)</param>
    /// <param name="speed"></param>
    public void MoveTo(Vector2 _target, float speed)
    {
        Vector2 move_direction = _target - Position;
        if(move_direction.x > 0) direction = Direction.RIGHT;
        if(move_direction.x < 0) direction = Direction.LEFT;
        UpdateDirection();
        velocity = speed * move_direction.Normalized();
        velocity = MoveAndSlide(velocity);
    }
}