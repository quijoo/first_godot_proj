using Godot;
using Godot.Collections;
public class FallingSpikes : KinematicBody2D, Archivable, IAttacker

{
    enum State : int
    {
        stuck,
        falling,
        ground,
    }
    [Export] public int gravity = 1000;
    public float Damage { get => 4f;}
    private RayCast2D ray; 
    private Vector2 velocity;
    private bool _EnablePhysics = true;
    private State state;
    public override void _Ready()
    {
        ray = GetNode<RayCast2D>(nameof(RayCast2D));
        state = State.stuck;
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        if(state == State.falling)
        {

            ApplyGravity(delta);
            velocity = MoveAndSlideWithSnap(velocity, Vector2.Up);
            velocity.x = 0;
            for(int i = 0; i < GetSlideCount(); i++)
            {
                var collider = GetSlideCollision(i).Collider;
                if(collider is TileMap)
                {
                    state = State.ground;
                    GetNode<CollisionShape2D>("Area2D/CollisionShape2D").SetDeferred("disabled", true);
                }
            }
        }
        else if(state == State.stuck)
        {
            if(ray.IsColliding() && ray.CollideWithBodies && ray.GetCollider() is Player)
            {
                state = State.falling;
            }
        }
        else if(state == State.ground)
        {
        }
    }
    public void ApplyGravity(float delta)
    {
        velocity.y += gravity * delta;
    }

    public void Load(in Dictionary data)
    {
        Position = new Vector2((float)data["PosX"], (float)data["PosY"]);
        state = (State)data["state"];
        velocity.y = (float) data["V.y"];
        GD.Print("Load spike");
        if(state != State.ground)   GetNode<CollisionShape2D>("Area2D/CollisionShape2D").SetDeferred("disabled", false);
    }
    public void Save(in Dictionary data)
    {
        GD.Print("Save spike");

        data["PosX"] = Position.x;
        data["PosY"] = Position.y;
        data["state"] = (int)state;
        data["V.y"] = velocity.y;
    }
    public void _on_Area2D_body_entered(Node body)
    {
        if(body is Player)
            (body as IHitable).OnHit(this, "spike");
    		// Archive.ArchiveManager.LoadGame(0, "Root");

    }
}
