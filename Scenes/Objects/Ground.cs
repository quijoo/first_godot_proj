using Godot;
using System;

//  两个状态就不用 StateMachine 了
public class Ground : StaticBody2D
{
    // Cached reference.
    private AnimatedSprite anim;
    private CollisionPolygon2D unpressed_collision;
    private CollisionPolygon2D pressed_collision;
    bool is_button_pressed = false;

    // Callback function when button is pressed.

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        anim = GetNode<AnimatedSprite>("AnimatedSprite");
        unpressed_collision = GetNode<CollisionPolygon2D>("UnpressedCollision");
        pressed_collision = GetNode<CollisionPolygon2D>("PressedCollision");
        if(anim == null) Logger.Error("Search AnimatedSprite fail.");
        if(unpressed_collision == null) Logger.Error("Search UnpressedCollision fail.");
        if(pressed_collision == null) Logger.Error("Search PressedCollision fail.");

    }

    public override void _Process(float delta)
    {
        
    }

    private void PressButton()
    {
        is_button_pressed = true;
        unpressed_collision.SetDeferred("disable", false);
        pressed_collision.SetDeferred("disable", true);
        anim.Play("Pressed");
    }
    private void UnpressButton()
    {
        // 这里的范围是有问题的，离开碰撞体但是没有离开 ButtonTexture 的范围
        is_button_pressed = false;
        unpressed_collision.SetDeferred("disable", true);
        pressed_collision.SetDeferred("disable", false);
        anim.Play("Unpressed");
    }
    public void _on_PressDetector_body_entered(Node body)
    {
        // 使用 位掩码来判断， 或者使用 Layer 判断，用 is 需要反射太消耗性能
        if(body is RigidBox || body is Player || body is Enemy)
        {
            PressButton();
        }
    }
    public void _on_PressDetector_body_exited(Node body)
    {
        // 这里不能直接判断， 应该检查上方没有其他任何物体的时候才 Unpress
        if(body is RigidBox || body is Player || body is Enemy)
        {
            if(is_button_pressed)
            UnpressButton();
        }
    }
     
}
