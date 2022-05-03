using Godot;
using System;
using System.Collections;
public class StoneGate : StaticBody2D, IGate
{
    AnimatedSprite anim;
    CollisionShape2D collision;
    bool is_closed = true;
    bool is_anim = false;
    enum Command
    {
        Open,
        Close,
    }
    System.Collections.Generic.Queue<Command> queue = new System.Collections.Generic.Queue<Command>();
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
    public override async void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(is_anim) return;
        if(queue.Count == 0) return;
        Command command = queue.Dequeue();
        is_anim = true;
        if(command == Command.Close && !is_closed)
        {
            anim.Play("Close");
            await ToSignal(anim, "animation_finished");
            anim.Play("Idle");
            collision.SetDeferred("disabled", false);
            is_closed = true;
        }
        if(command == Command.Open && is_closed)
        {
            anim.Play("Open");
            await ToSignal(anim, "animation_finished");
            collision.SetDeferred("disabled", true);
        }
        is_anim = false;
    }

    public void Lock()
    {
        queue.Enqueue(Command.Close);    
    }
    public void UnLock()
    {
        queue.Enqueue(Command.Open);       
    }
}
