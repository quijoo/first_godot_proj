using Godot;
using System;
public class BasicFollowCamera : ColorRect
{
	[Export] private Vector2 position;
	[Export] private Vector2 size = new Vector2(50, 50);
	[Export] Vector2 offset;
	// cache
	static public Controller Target { get; private set;}
	static RectangleShape2D shape;
    static bool HasTarget = false;
	public override async void _Ready()
	{
		await ToSignal(GetTree().Root, "ready");
		SetSize(size);
        GD.Print("Camera ready");
	}
	public override void _Process(float delta)
	{
        if(!HasTarget) return;
        if(!Target.IsInsideTree()) return;
        // bug:指定文件名， 指定框框大小
        MarginLeft = -size.x / 2;
        MarginRight = size.x / 2;
        MarginTop = -size.y / 2;
        MarginBottom = size.y / 2;
        if(!HasTarget) return;
		float half_width = shape.Extents.x;
		float half_height = shape.Extents.y;
        // Vector2 target = GetParent<Node2D>().ToLocal(Target.GetParent<Node2D>().ToGlobal(Target.Position));
        Vector2 target =  Target.GetParent<Node2D>().ToGlobal(Target.Position);

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
		SetPosition(position);
	}
    static public void Follow(Controller worrior)
    {
        HasTarget = true;
        Target = worrior as Controller;
        shape = (RectangleShape2D)(Target.collision_shape.Shape);
    }
    static public void UnFollow()
    {
        HasTarget = false;
    }

}
