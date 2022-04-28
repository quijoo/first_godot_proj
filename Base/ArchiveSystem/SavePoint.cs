using Godot;
using System;

public class SavePoint : ColorRect
{
    Color green = Color.ColorN("green");
    Color red = Color.ColorN("red");

    static SavePoint current = null; 
    public override void _Ready()
    {
        red.a = 0.2f;
        green.a = 0.2f;
        Color =  red;
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        Color = current == this ? red : green;
    }
    public void _on_Area2D_body_entered(Node body)
    {
        if(current == this) return;
        if(body is Player)
        {
            Archive.ArchiveManager.SaveGame(0, "Root");
            current = this;
        }
    }
}
