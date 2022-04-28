using Godot;
using System;

public partial class Player
{
    public void _on_TrailTimer_timeout()
	{
		if(Mathf.Abs(velocity.x) < 0.1) return;
        Sprite trail_instance = packed_trail.Instance<Sprite>();
        GetParent().AddChild(trail_instance);
        // 覆盖 trail (之后在场景中添加一个 Effect Node)
        GetParent().MoveChild(trail_instance, GetIndex());
        
        foreach(var name in properties)
        {
            trail_instance.Set(name, sprite.Get(name));
        }
    }
}
