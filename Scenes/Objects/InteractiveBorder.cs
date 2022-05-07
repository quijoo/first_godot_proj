using Godot;
using System;

public class InteractiveBorder : Node2D
{
    public Rect2 window;
    [Export] private Rect2 DefalutRect;
    public InteractiveLine left;
    public InteractiveLine right;
    public InteractiveLine top;
    public InteractiveLine down;
    public Vector2 Start
    {
        get => GetViewportTransform() * (GetGlobalTransform() * window.Position);
    }
    public Vector2 End
    {
        get => GetViewportTransform() * (GetGlobalTransform() * (window.Position + window.Size));
    }
    public Rect2 rect
    {
        get => new Rect2(Start, End - Start);
    }
    public override void _Ready()
    {
        left = GetNode<InteractiveLine>("Left");
        right = GetNode<InteractiveLine>("Right");
        top = GetNode<InteractiveLine>("Top");
        down = GetNode<InteractiveLine>("Down");
    }
    public override void _EnterTree()
    {
        base._EnterTree();
    }
    public override void _Draw()
    {
        float margin = 2f;
        DrawRect(new Rect2(window.Position, new Vector2(margin, window.Size.y)), left.IsLocked ? Color.ColorN("red") : Color.ColorN("green"));
        DrawRect(new Rect2(window.Position, new Vector2(window.Size.x, margin)), top.IsLocked ? Color.ColorN("red") : Color.ColorN("green"));
        DrawRect(new Rect2(window.Position + window.Size - new Vector2(window.Size.x, margin), new Vector2(window.Size.x, margin)), down.IsLocked ? Color.ColorN("red") : Color.ColorN("green"));
        DrawRect(new Rect2(window.Position + window.Size - new Vector2(margin, window.Size.y), new Vector2(margin, window.Size.y)), right.IsLocked ? Color.ColorN("red") : Color.ColorN("green"));
    }
    public override void _Process(float delta)
    {
        Update();
    }
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        window.Position = new Vector2(left.Position.x, top.Position.y);
        window.Size = new Vector2(right.Position.x - left.Position.x, down.Position.y - top.Position.y);
    }
    public void Reset(Vector2 global_position)
    {
        window = DefalutRect;
        left.Reset(global_position);
        right.Reset(global_position);     
        top.Reset(global_position);     
        down.Reset(global_position);
    }
}
