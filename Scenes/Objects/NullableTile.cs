using Godot;
using System;
using Godot.Collections;
public class NullableTile: Node2D, Archivable
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    public void Save(in Dictionary data)
    {      
        data["PosX"] = Position.x; 
        data["PosY"] = Position.y; 
    }
    public void Load(in Dictionary data)
    {
        Position = new Vector2((float)data["PosX"], (float)data["PosY"]);
    }
}
