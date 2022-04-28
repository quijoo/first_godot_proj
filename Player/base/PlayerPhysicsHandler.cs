using Godot;
using System;

public partial class Player
{
    public void HorizontalControllHandler(bool reset = true)
    {
        if(horizontal_input > 0) direction = Direction.RIGHT;
        if(horizontal_input < 0) direction = Direction.LEFT; 
        
        UpdateDirection();
        if(Input.IsActionPressed("ui_right") || Input.IsActionPressed("ui_left"))
        {
            velocity.x = walk_speed * horizontal_input;
        }
        else if(reset)
        {
            velocity.x = 0;
        }
    }
    public void DashControlHandler()
    {
        if(direction == Direction.RIGHT)
        {
            velocity.x = dash_speed;
        }
        if(direction == Direction.LEFT)
        {
            velocity.x = -dash_speed;
        }
    }
    public void LockVerticalMovement()
    {
        velocity.y = 0;
    }
}
