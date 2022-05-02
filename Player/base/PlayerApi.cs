using Godot;
using System.Diagnostics;
using Events;
public partial class Player
{
	public float GetHorizontalInput() 
	{
		return Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
	}
	public bool HasDashes()
	{
		return num_dash > 0;
	}
	public void ResetDashCounter(int value)
	{
		num_dash = value;
	}
	public void RestartLevel()
	{
		Reborn();
		GetNode<StateMachine>("StateMachine").Transition<Idle>();
	}
	public override void Reborn()
	{
		// EventManager.Send(this, new RebornEvent(this, ((Position2D)GetParent()).Position, "机制"));
        GD.Print("call Reborn function");
		Archive.ArchiveManager.LoadGame(0, "Player");
	}
	// Attack area related.
	public void EnableHorizontalArea()
	{
		attack.horizontal_area.GetNode<CollisionPolygon2D>(nameof(CollisionPolygon2D)).SetDeferred("disabled", false);
	}
	public void DisableHorizontalArea()
	{
		attack.horizontal_area.GetNode<CollisionPolygon2D>(nameof(CollisionPolygon2D)).SetDeferred("disabled", true);
	}
	public void EnableDownArea()
	{
        GD.Print("EnableDownArea");

		attack.down_area.GetNode<CollisionPolygon2D>(nameof(CollisionPolygon2D)).SetDeferred("disabled", false);
	}
	public void DisableDownArea()
	{
        GD.Print("DisableDownArea");
		attack.down_area.GetNode<CollisionPolygon2D>(nameof(CollisionPolygon2D)).SetDeferred("disabled", true);
	}
	public override void UpdateDirection()
	{
		if(direction == Direction.RIGHT)
		{
			sprite.FlipH = false;
			attack.horizontal_area.RotationDegrees = 0f;
			attack.horizontal_area.Position = Vector2.Right * 4;
		}
		if(direction == Direction.LEFT)
		{
			sprite.FlipH = true;
			attack.horizontal_area.RotationDegrees = 180f;
			attack.horizontal_area.Position = Vector2.Left * 4;
		}
	}
	public void OnHit(IAttacker attacker, string info)
	{
		// Health -= attacker.Damage;
		// if(Health <= 0) Dead("unhealth");
		Health -= 1;
		EventManager.Send(this, new PlayerHurtedEvent(attacker, this, 1));
        GetNode<StateMachine>("StateMachine").Transition<Hit>();
        if(Health <= 0)
		{
			Dead("unhealth");
		}
	}
	public override void Dead(string reason)
	{
		GetNode<StateMachine>("StateMachine").Transition<Death>();
		
	}
    public void Pick(Item<Node> item)
    {
        // 往背包里添加 item
        bag.Add(item);
    }
}
