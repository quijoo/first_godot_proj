using Godot;
using System;

public class UserDefineRayCast2D : Godot.RayCast2D
{
    [Export] private Color UnCollisionColor = Color.ColorN("green");
    [Export] private Color CollisionColor = Color.ColorN("red");
    [Export] public bool VisualLine;
    CollisionObject2D parent;
    Vector2 CachePosition;
    private bool _Colliding;
    public override void _Ready()
    {
        parent = GetParent() as CollisionObject2D;
        CollisionMask = parent.CollisionMask;
    }

    public override void _Process(float delta)
    {
        CachePosition = ToLocal(parent.ToGlobal(Position));
        Update();  
    }
    public override void _PhysicsProcess(float delta)
    {
        _Colliding = IsColliding();
    }
    public override void _Draw()
    {
        if(VisualLine)
        {
            if(_Colliding)
            {
                DrawLine(CachePosition, CachePosition + CastTo, CollisionColor, 1, true);
            }
            else
            {
                DrawLine(CachePosition, CachePosition + CastTo, UnCollisionColor, 1, true);
            }
        }
    } 
}
