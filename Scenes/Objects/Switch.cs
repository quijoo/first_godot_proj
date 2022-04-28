using Godot;
using System;

public class Switch : Area2D
{
    // signal define
    // [Signal] public delegate void switch_changed(int id);
    [Signal] public delegate void switch_changed(); 
    // animation cache
    AnimatedSprite anim;
    // switch control
    bool is_switch_open = false;

    // ID related.
    // static int global_switch_id = -2;

    // [Export] public readonly int outer_switch_id = -1;
    // private int inner_switch_id;
    // int switch_id 
    // {
    //     get
    //     {
    //         return (outer_switch_id == -1) ? inner_switch_id : outer_switch_id;
    //     }
    // }
    // Switch()
    // {
    //     inner_switch_id = global_switch_id--;
    // }

    // Ready
    public override void _Ready()
    {
        anim = GetNode<AnimatedSprite>("AnimatedSprite");
    }
    public void _on_Switch_area_entered(Area2D area)
    {
        if(area.Owner is Player)
        {
            anim.Play(is_switch_open ? "Close" : "Open");
            is_switch_open = !is_switch_open;
            // 优化为函数调用列表
            // GetNode<StoneGate>("StoneGate").Switch();
            // EmitSignal("switch_changed", switch_id);
            EmitSignal("switch_changed");
        }
    }
}
