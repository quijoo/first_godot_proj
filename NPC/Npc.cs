using Godot;
using System;

public class Npc : Controller
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    AnimatedSprite talk_bubble;
    [Export(PropertyHint.File, "*.json")] private string json_file;

    public override void _Ready()
    {
        base._Ready();
        talk_bubble = GetNode<AnimatedSprite>(nameof(AnimatedSprite)); 
    }
    public override void UpdateDirection()
	{
	}
    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
        if(talk_bubble.Visible && @event.IsActionPressed("interact"))
        {
            DialogBox.ShowDialog(json_file);
        }
    }
    private void _on_Dialog_area_entered(Node body)
    {
        // 显示可交互提示
        talk_bubble.Show();
    }
    private void _on_Dialog_area_exited(Node body)
    {
        // 关闭显示可交互提示
        talk_bubble.Hide();
    }
}
