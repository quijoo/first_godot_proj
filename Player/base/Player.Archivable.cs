using Godot;
using System;
using Godot.Collections;
using System.Diagnostics;
using Events;
public partial class Player
{
    // 固定写的两个函数
    public void Save(in Dictionary data)
    {      
        data["PosX"] = Position.x; 
        data["PosY"] = Position.y; 
        data["Health"] = Health;
    }
    public void Load(in Dictionary data)
    {
        Position = new Vector2((float)data["PosX"], (float)data["PosY"]);
        Health = (float)data["Health"];
        GetNode<StateMachine>("StateMachine").Transition<Idle>();
		EventManager.Send(this, new LoadEvent(this));

    }
}
