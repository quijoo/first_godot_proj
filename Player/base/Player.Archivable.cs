using Godot;
using System;
using Godot.Collections;
using System.Diagnostics;
public partial class Player
{
    // 固定写的两个函数
    public void Save(in Dictionary data)
    {      
        data["PosX"] = Position.x; 
        data["PosY"] = Position.y; 
    }
    public void Load(in Dictionary data)
    {
        Position = new Vector2((float)data["PosX"], (float)data["PosY"]);
        GetNode<StateMachine>("StateMachine").Transition<Idle>();
    }
}
