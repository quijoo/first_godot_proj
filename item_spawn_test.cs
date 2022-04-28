using Godot;
using System;

public class item_spawn_test : Position2D
{
	[Export] PackedScene scene;
	public override async void _PhysicsProcess(float delta)
	{
        if(Input.IsActionJustPressed("test"))
        {
            Item item = scene.Instance<Item>();
            CallDeferred("add_child", item);
            await ToSignal(item, "ready");
            item.Init(this);
            item.ConvertToSceneItem();
        }
	}
}
