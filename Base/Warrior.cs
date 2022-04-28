using Godot;
using System;
using Events;
public abstract class Warrior : Controller
{
    // 基本属性
    // 健康值
    public float Health;

    // 所有的 Warrior 都应该监听事件？
    // 这里只是写一个 Demo， 实际上 Warrior 可以不监听事件
    public abstract void RigisterEvents();
    public abstract void UnrigisterEvents(); 
    public override void _EnterTree()
    {
        base._EnterTree();
        RigisterEvents();
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        UnrigisterEvents();
    }   

}
