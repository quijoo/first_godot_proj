using Godot;
using System;
using System.Collections.Generic;


public interface IInventory<T> where T : Node
{
   
    /// <summary>
    /// 对 Item 排序
    /// </summary>
    void Sort();
    I GetItem<I>(int index) where I : Item<T>;
    I GetItem<I>(string name) where I : Item<T>;
    List<I> GetItem<I>() where I : Item<T>;
    I GetItem<I>(Vector2 location) where I : Item<T>;
    void Save();
    void Load();
    void Open();
    void Close();
}
public abstract class InventoryBase : CanvasLayer
{
    public Button UseButton;
    public Button DropButton;
    [Export]  public PackedScene UIItemControlPrefab;
    public List<UIItemControl> Items;
    public string InventoryName;
    public GridContainer Grid;
    public UIItemControl CurrentItem;
    public VBoxContainer container;
    public Control Background;
    public bool IsOpen = false;
    public bool IsSelected = false;
    
    public int Width = 6;
    public int Height = 6;
    public override void _Ready()
    {
        base._Ready();
        Grid = GetNode<GridContainer>(nameof(GridContainer));
        container = GetNode<VBoxContainer>(nameof(VBoxContainer));
        UseButton = container.GetNode<Button>("UseButton");
        DropButton = container.GetNode<Button>("DropButton");
        Background = GetNode<Control>(nameof(Control));
        Items = new List<UIItemControl>(Height * Width);
    }
    public void SelectorAttach()
    {
        if(!IsSelected && CurrentItem.hasItem)
        {
            container.RectPosition = new Vector2(103, 62) + CurrentItem.RectPosition;
            container.Visible = true;
            IsSelected = true;
            UseButton.GrabFocus();
        }
        else
        {
            container.Visible = false;
            IsSelected = false;
            CurrentItem.GrabFocus();
        }  
    }
}
public abstract partial class Inventory<T> : InventoryBase, IInventory<T> where T : Node
{
    private Item<T>[] store;
    public T InventoryOwner;
    public void Init(T owner) { InventoryOwner = owner; }
    public virtual void Remove(Item<T> item)
    {
        for(int i = 0; i < store.Length; i++)
        {
            if(store[i] == item) 
            {
                store[i] = null;
                break;
            }
        }
        foreach(var _item in Items)
        {
            _item.Unstore(item);
        }
        item.ConvertToSceneItem();   
    }
    public virtual void Add(Item<T> item)
    {
        for(item.Index = 0; item.Index < store.Length; item.Index++)
        {
            if(store[item.Index] == null)
            {
                store[item.Index] = item;
                item.ConvertToUIItem();
                item.Position = Vector2.Zero;
                if(item.GetParent() != null) item.GetParent().CallDeferred("remove_child", item);
                this.SetItem(item);
                break;
            }
        }  
        GD.Print(store);
    }
    public virtual void Sort()
    {

    }
    public I GetItem<I>(int index) where I : Item<T>
    {
        if(index >= 0 && index <= Width * Height)
        {
            return store[index] as I;
        }
        return null;
    }
    public I GetItem<I>(string name) where I : Item<T>
    {
        foreach(var it in store)
        {
            if(it.ItemName == name)
            {
                return it as I;
            }
        }
        return null;
    }
    public List<I> GetItem<I>() where I : Item<T>
    {
        List<I> list = new List<I>();
        foreach(var it in store)
        {
            if(it is I)
            {
                list.Add(it as I);
            }
        }
        return list;
    }
    public I GetItem<I>(Vector2 location) where I : Item<T>
    {
        return store[(int)(location.y * Width + location.x)] as I;
    }
    public virtual void Save() { }
    public virtual void Load() { }
}