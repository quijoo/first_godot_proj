using Godot;
using System;
using System.Collections.Generic;

public class InteractiveLine : KinematicBody2D
{
    public bool IsLocked;
    static public string IsSlideName = "";
    public SegmentShape2D shape;
    public InteractiveWindow window;
    public Vector2 velocity = Vector2.Zero;
    public int ColllisionCount = 0;
    [Export] public Vector2 DefaultBias;

    public override void _Ready()
    {

        shape = GetNode<CollisionShape2D>(nameof(CollisionShape2D)).Shape as SegmentShape2D;
        window = GetParent().GetParent<InteractiveWindow>();
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        foreach (Node child in GetChildren())
        {
            if(child != null && child is InteractiveAnchor)
            {
                child.QueueFree();
            }
        }
        GetNode<StateMachine>(nameof(StateMachine)).Transition<InteractiveLineFollow>();
    }
    public override void _PhysicsProcess(float delta)
    {
    }
    public void SetCollision(uint layer, uint mask)
    {
        CollisionLayer = layer;
        CollisionMask = mask;
    }
    public void Reset(Vector2 player_position_global)
    {
        Position = GetParent<Node2D>().ToLocal(player_position_global) + DefaultBias;
    }
    public float DistanceToLine(Vector2 P, Vector2 A, Vector2 B)
    {
        float a = (P - B).LengthSquared();
        float b = (P - A).LengthSquared();
        float c = (A - B).LengthSquared();

        return Mathf.Sqrt(a - Mathf.Pow(a-b+c, 2f) / (4*c));
    }
    public Vector2 NormalToLine(Vector2 P, Vector2 A, Vector2 B)
    {
        return  ((P - B) - (P - B).Length() * Mathf.Cos((A - B).AngleTo(P - B)) * (A - B).Normalized());
    }
}
