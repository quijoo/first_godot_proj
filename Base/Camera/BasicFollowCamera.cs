using Godot;
using System;
using Events;
public class BasicFollowCamera : Node2D
{
	[Export] private Vector2 position;
	[Export] private Vector2 size = new Vector2(50, 50);
	[Export] Vector2 offset;
	// cache
	static public Controller Target { get; private set;}
	static RectangleShape2D shape;
    static public BasicFollowCamera instance = null;
    static public Camera2D camera = null;
    public ColorRect rect;


	public override async void _Ready()
	{
		await ToSignal(GetTree().Root, "ready");
        rect = GetNode<ColorRect>("ColorRect");
		rect.SetSize(size);
        instance = this;
        camera = GetNode<Camera2D>("Camera2D");
        GD.Print("Camera ready");
	}
	public override void _Process(float delta)
	{
        if(Target == null) return;
        if(!Target.IsInsideTree()) return;
		float half_width = shape.Extents.x;
		float half_height = shape.Extents.y;
        Vector2 half = new Vector2(shape.Extents.x, shape.Extents.y);
        Vector2 target =  Target.GlobalPosition;
		if(target.x + half_width > position.x + size.x)
		{
			position.x = target.x - size.x + half_width;
		}
		else if(target.x - half_width < position.x)
		{
			position.x = target.x - half_width;
		}

		if(target.y + half_height > position.y + size.y)
		{
			position.y = target.y - size.y + half_height;
		}
		else if(target.y - half_height < position.y)
		{
			position.y = target.y - half_height;
		}
        GlobalPosition = position;
	}
    static public void Follow(Controller worrior)
    {
        Target = worrior as Controller;
        shape = (RectangleShape2D)(Target.collision_shape.Shape);
    }
    static public void UnFollow()
    {
        Target = null;
    }

}
