using Godot;
using System;
using System.Collections.Generic;
using Events;

public class HealthHUD : GridContainer
{
    public override void _EnterTree()
    {
        base._EnterTree();
        EventManager.Subscribe<PlayerHurtedEvent>(this, OnPlayerHurtedEvent);
        EventManager.Subscribe<RebornEvent>(this, OnRebornEvent);

    }
    public override void _ExitTree()
    {
        base._ExitTree();
        EventManager.Unsubscribe<PlayerHurtedEvent>(this, OnPlayerHurtedEvent);
        EventManager.Unsubscribe<RebornEvent>(this, OnRebornEvent);
    }
    [Export] PackedScene Heart;
    private List<Node> heart_list = new List<Node>();
    public override void _Ready()
    {
        
    }

    public void Increase(int value)
    {
        for(int i = 1; i <= value; i++)
        {
            Node heart = Heart.Instance();
            heart.Name = (heart_list.Count + i).ToString();
            this.AddChild(heart);
            heart_list.Add(heart);
        }
    }
    public void Decrease(int value)
    {
        if(value  > heart_list.Count)
        {
            return;
        }
        for(int i = 1; i <= value; i++)
        {
            Node heart = heart_list[heart_list.Count - 1];
            this.RemoveChild(heart);
            heart_list.Remove(heart);
            heart.QueueFree();
        }
    }
    public void Set(int value)
    {
        Decrease(heart_list.Count);
        Increase(value);
    }
    // Callback
    public void OnPlayerHurtedEvent(Node node, IEvent e)
    {
        PlayerHurtedEvent ee = (PlayerHurtedEvent)e;
        if(ee.victim is Player)
        {
            Decrease((int)ee.harm_value);
        }
    }
    public void OnRebornEvent(Node node, IEvent e)
    {
        RebornEvent ee = (RebornEvent)e;
        if(ee.node is Player)
        {
            Set((int)(ee.node as Player).Health);
        }
    }
}
