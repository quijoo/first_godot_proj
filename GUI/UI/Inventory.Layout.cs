using Godot;
using System;
using System.Collections.Generic;
public abstract partial class Inventory<T> 
{

    [Export] int ContainerSize = 18;
    public bool Selected = false;
    private int index = 0;
    public override void _Ready()
    {
        base._Ready();
        store = new Item<T>[Width * Height];
        for(int i = 0; i < Width * Height; i++)
        {
            UIItemControl obj = UIItemControlPrefab.Instance<UIItemControl>();
            obj.MeshPosition = new Vector2(i % Width, i / Width);
            obj.SetPosition(new Vector2((i % Width) * ContainerSize, (i / Width) * ContainerSize));
            obj.FocusMode = Control.FocusModeEnum.All;
            obj.Name = "Item_" + i.ToString();
            Items.Add(obj);
            Grid.AddChild(obj);
        }
        for(int i = 0; i < Width * Height; i++)
        {
            int x = i % Width, y = i / Width;
            Items[i].FocusNeighbourLeft = new NodePath("../" + (x == 0 ? Items[i].Name : Items[i - 1].Name));
            Items[i].FocusNeighbourTop = new NodePath("../" + (y == 0 ? Items[i].Name : Items[i - Width].Name));
            Items[i].FocusNeighbourRight = new NodePath("../" + (x == Width-1 ? Items[i].Name : Items[i + 1].Name));
            Items[i].FocusNeighbourBottom = new NodePath("../" + (y == Height-1 ? Items[i].Name : Items[i + Width].Name));
            // Items[i].FocusNext = new NodePath("./" + (x ==Width * Height - 1 ? Items[i].Name : Items[i + 1].Name));
            Items[i].FocusPrevious = new NodePath("../" + (x > 0 ? Items[i - 1].Name : Items[i].Name)); 
        }
        // 默认关闭 UI
        Close();
        
    }
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(Input.IsActionJustPressed("bag"))
        {
            if(IsOpen)  Close();
            else Open();
        }
    }
    
    private void _on_UseButton_button_down()
    {
        // 使用按钮按下， 使用当前 Item
        GD.Print("使用 Item");
        Item<T> item = CurrentItem.CurrentInventoryItem as Item<T>;
        // 本来应该是 Player， 暂时写死
        Remove(item);
        SelectorAttach();
        item.OnUse(InventoryOwner);
    }
    private void _on_DropButton_button_down()
    {
        // 丢弃按钮按下， 丢弃当前 Item
        // 为了防止循环引用，调用Item的使用方法通过事件来处理
        // 事件 XXXItemUse
        GD.Print("丢弃 Item");
        Item<T> item = CurrentItem.CurrentInventoryItem as Item<T>;

        Remove(item);
        SelectorAttach();
        item.OnDrop();
    }
    private void _on_UseButton_gui_input(InputEvent @event)
    {
        if (@event is InputEventJoypadButton mba && mba.ButtonIndex == (int)JoystickList.Button1 && mba.Pressed)
        {
            //取消
            SelectorAttach();
        }
    }
    private void _on_DropButton_gui_input(InputEvent @event)
    {
        if (@event is InputEventJoypadButton mba && mba.ButtonIndex == (int)JoystickList.Button1 && mba.Pressed)
        {
            //取消
            SelectorAttach();
        }
    }
    public void SetItem(Item<T> node)
    {
        if(0 <= node.Index && node.Index < Items.Count)
        {
            Items[node.Index].Store(node);
        }

    }
    public void RemoveItem(Item<T> node)
    { 
        if(0 <= node.Index && node.Index < Items.Count)
        {
            Items[node.Index].Unstore(node);
        }   
    }
    public virtual void Open() 
    {
        Grid.Visible = true;
        container.Visible = false;
        Background.Visible = true;
        IsOpen = true;
        Items[0].GrabFocus();
    }
    public virtual void Close() 
    {
        Grid.Visible = false;
        container.Visible = false;
        Background.Visible = false;
        IsOpen = false;
    }
}
