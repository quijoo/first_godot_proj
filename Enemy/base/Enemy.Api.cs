using Godot;
using System;

public partial class Enemy
{
    /// 一些可被调用的API函数
    public void OnHit(IAttacker attacker, string info)
    {
        // Health -= attacker.Damage;
        // if(Health <= 0) Dead("unhealth");
        Dead("attacked by hero!@!!");
    }
    public override void Dead(string reason)
    {
        GetNode<StateMachine>("StateMachine").Transition<EnemyDeath>();
    }
    public Vector2 Rebound(Vector2 p1, Vector2 p2)
    {
        return _ReboundSpeed;
    }
    public void HorizontalControlHandler(float delta)
    {
        velocity.x = walk_speed * (direction.Match(Direction.RIGHT) ? 1 : -1);
    }
    public override void UpdateDirection()
    {
        if(direction == Direction.RIGHT)
        {
            sprite.FlipH = false;
        }
        if(direction == Direction.LEFT)
        {
            sprite.FlipH = true;
        }
    }
}
