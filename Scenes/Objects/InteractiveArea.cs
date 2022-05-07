using Godot;
using System;

public class InteractiveArea : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Scene = SceneManager.GetScene(SceneManager.SceneName);
    }
    Node Scene;
    public override void _EnterTree()
    {
        base._EnterTree();
    }
    public override void _PhysicsProcess(float delta)
    {
        Scene.ChangeProcess(false, CollisionMask);

    }
    void _on_Area2D_body_entered(Node body)
    {
        GD.Print("entered", body.Name);
        body.ChangeProcess(true, CollisionMask);
    }
    void _on_Area2D_body_exited(Node body)
    {
        GD.Print("exited", body.Name);
        body.ChangeProcess(false, CollisionMask);
    }
}
