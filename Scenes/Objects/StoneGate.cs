using Godot;
using System;
using System.Collections;
public class StoneGate : StaticBody2D, IGate
{
    AnimatedSprite anim;
    CollisionShape2D collision;
    bool is_closed = true;
    bool is_anim = false;
    public override void _Ready()
    {
        anim = GetNode<AnimatedSprite>("AnimatedSprite");
        collision = GetNode<CollisionShape2D>("CollisionShape2D");
    }
    public async void Switch()
    {
        if(is_anim) return;

        // anim lock
        is_anim = true;

        // open the gate
        anim.Play(is_closed ? "Open" : "Close");
        await ToSignal(anim, "animation_finished");
        // recover idle state
        if(!is_closed) anim.Play("Idle");
        // anim unlock
        is_anim = false;
        collision.SetDeferred("disabled", is_closed);
        is_closed = !is_closed;        
    }
}
