using Godot;
using System;

public class HUD : CanvasLayer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public HealthHUD Health;
    public override void _Ready()
    {
        Health = GetNode<HealthHUD>(nameof(HealthHUD));
    }

}
