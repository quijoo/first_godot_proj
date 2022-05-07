using Godot;
using System;

public class InteractiveAnchor : RigidBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.\
    public Vector2 velocity;
    public bool IsStop = false;
    public Vector2 StopPosition = Vector2.Zero;
    ColorRect rect;
    RandomNumberGenerator _random = new RandomNumberGenerator();
    public delegate void InteractiveAnchorCallBack(InteractiveAnchor anchor);
    public InteractiveAnchorCallBack callback;
    // cache 一旦锁定目标，不可更改！！
    InteractiveLine AnotherCache = null;
    
    public override void _Ready()
    {
        rect = GetNode<ColorRect>("ColorRect");
    }
    public void Init(Vector2 _velocity, InteractiveAnchorCallBack _callback)
    {
        velocity = _velocity;
        callback = _callback;
    }
    public override void _ExitTree()
    {
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        if(!IsStop) this.AddForce(Vector2.Zero, velocity);
        else if(AnotherCache != null)
        {
            // 计算交点position
            var Line_1 = GetParent<InteractiveLine>();
            var Line_2 = AnotherCache;
            var normal = Line_1.NormalToLine(Line_1.GlobalPosition, Line_2.GlobalPosition + Line_2.shape.A, Line_2.GlobalPosition + Line_2.shape.B);
            var g_position = Line_1.GlobalPosition - normal;
            Position = Line_1.ToLocal(g_position);
        }
        else
        {
            Position = StopPosition;
        }
    }
    // 添加抖动将要删除时 
    public void _on_DeleteEffect_timeout()
    {
        callback.Invoke(this);
    }
    void _on_Area2D_body_entered(Node body)
    {
        if(body is InteractiveLine)
        {
            (body as InteractiveLine).ColllisionCount += 1;
            if(body != null && !(GetParent() is InteractiveLine))
            {
                // change tree
                Vector2 StopGlobalPosition = GlobalPosition;
                this.GetParent().CallDeferred("remove_child", this);
                body.CallDeferred("add_child", this);
                this.CallDeferred("CacheCurrentState", StopGlobalPosition);
                // change state
                this.IsStop = true;
                // cahnge force
                this.AppliedForce = Vector2.Zero;
            }
            else if(body != null && (GetParent() is InteractiveLine) && body != GetParent())
            {
                AnotherCache = body as InteractiveLine;
            }
        }
    }
    void _on_Area2D_body_exited(Node body)
    {
        (body as InteractiveLine).ColllisionCount -= 1;
    }
    void CacheCurrentState(Vector2 origin_global_position)
    {
        // change position
        this.GlobalPosition = origin_global_position;
        StopPosition = this.Position;
    }
}