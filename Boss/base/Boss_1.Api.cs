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
    public void MoveTo(Vector2 target, float speed)
    {
        Vector2 move_direction = target - Position;
        if(move_direction.x > 0) direction = Direction.RIGHT;
        if(move_direction.x < 0) direction = Direction.LEFT;
        UpdateDirection();
        velocity = speed * move_direction.Normalized();
        velocity = MoveAndSlide(velocity);
    }
}