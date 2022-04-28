using Godot;
using System;
using System.Collections.Generic;
// 库存系统
// 处理库存，物品 之间的关系，不考虑场景物品，UI背包物品

/// <summary>
/// 背包物品
/// </summary>

public interface IInventoryItem<T> where T : Node
{
    void OnDrop();
    void OnAdd();
    IComparable GetKey();
    void ShowInfo();
    void OnUse(T user);
    void OnLoad();
    void OnSave();

}
public abstract class InventoryItemBase : Node2D
{
    public string ItemName;
    public int Index = -1;
    public Vector2 Location;
    public Node2D uiItem;
    public SceneItem sceneItem;
    public override void _Ready()
    {
        base._Ready();
        uiItem = GetNode<Node2D>("UIItem");
        sceneItem = GetNode<SceneItem>("SceneItem");
    }
    public void ConvertToUIItem()
    {
        sceneItem.Disable();
        uiItem.Visible = true;
    }
    public void ConvertToSceneItem()
    {
        sceneItem.Enable();
        uiItem.Visible = false;
    }
}
public class Item<T> : InventoryItemBase, IInventoryItem<T> where T : Node
{
    
    public T ItemOwner;
    public virtual void Init(T owner) { ItemOwner = owner; }
    public override void _Ready()
    {
        base._Ready();
    }
    public void OnDrop()
    {
        ConvertToSceneItem();
        sceneItem.velocity =Vector2.Up * 10;
        Position = Vector2.Zero;
        ItemOwner.CallDeferred("add_child", this);
    }
    public void OnAdd()
    {

    }
    public IComparable GetKey()
    {
        return 1;
    }
    public void ShowInfo()
    {

    }
    public void OnUse(T user)
    {
        GD.Print("Call OnUse");
        // QueueFree()
    }
    public void OnLoad()
    {

    }
    public void OnSave()
    {

    }
}