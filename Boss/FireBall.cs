using Godot;
using System;

public class FireBall : KinematicBody2D, IElastic, IHitable
{
    private Area2D hurt_area;
    private AnimatedSprite sprite;
    // movement
    private Vector2 velocity = Vector2.Zero;
    public float Speed { get; set;} = 100f;
    public Node2D Target { set; get; } = null;
    public Node2D Weapon = null;
    // IElastic
    [Export] Vector2 _ReboundSpeed = Vector2.Up * 200;
    public Vector2 ReboundSpeed { get=>_ReboundSpeed; }
    public Vector2 Rebound(Vector2 p1, Vector2 p2)
    {
        return _ReboundSpeed;
    }
    // 击中特效
    public int HitCounter;
    public override void _Ready()
    {
        hurt_area = GetNode<Area2D>(nameof(Area2D));
        sprite = GetNode<AnimatedSprite>(nameof(AnimatedSprite));
        HitCounter = 0;
    }
    
    public void Init(Node2D target, Node2D weapon, float speed)
    {
        Target = target;
        Weapon = weapon;
        Speed = speed;
        Position = GetTree().Root.GetNode<Node2D>("MainMap/BulletPool").ToLocal(Weapon.GetParent<Node2D>().ToGlobal(Weapon.Position));
    }

    public override void _PhysicsProcess(float delta)
    {
        foreach(var collision in this.Extend_GetCollison())
        {
            if(collision.Collider is Player)
            {
                // process player collision
            }
            else if(collision.Collider is TileMap)
            {
                // 1. 播放爆炸动画
                // 2. 播放碰撞特效
                // 3. 等待动画结束 await
                // 4. 删除对象queue_free
                QueueFree();
            }
            // GD.Print(collision.Collider);
        }
        // 跟随玩家
        // 1. 获取目标的 Position
        // 2. 坐标转换
        // 2. 计算速度方向
        // - PlayerPosition
        //      - Player
        // - Enemy
        //      - FireballWeapon(this)
        //          - Bullet_1
        //          - Bullet_2
        //          - Bullet_3
        if(Target == null || Weapon == null) return;
        var end_posi = GetTree().Root.GetNode<Node2D>("MainMap/BulletPool").ToLocal(Target.GetParent<Node2D>().ToGlobal(Target.Position));
        // dot(a, b) = |a|x|b| cos(a,b)
        velocity = (end_posi - Position).Normalized() * Speed;
        LookAt(end_posi);
        velocity = MoveAndSlide(velocity);
    }

    public void OnHit(IAttacker attacker, string reason)
    {
        // 改变颜色， 改变移动方向，清除目标锁定
    }
}
