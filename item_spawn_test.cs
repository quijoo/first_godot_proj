using Godot;
using System;

public class item_spawn_test : Position2D
{
	[Export] PackedScene scene;
    private Console _wrapper;
    public override void _Ready()
    {
        _wrapper = GetTree().Root.GetNode<Console>("CSharpConsole");        
		_wrapper.AddCommand("item_spawn", this, nameof(ItemSpawn))
				.SetDescription("spawn <iten:%id%> at item spawn position")
				.AddArgument("id", Variant.Type.Int)
				.Register();
    }
    public async void ItemSpawn(int id)
    {
        Item item = scene.Instance<Item>();
        CallDeferred("add_child", item);
        await ToSignal(item, "ready");
        item.Init(this);
        item.ConvertToSceneItem();
    }
}
