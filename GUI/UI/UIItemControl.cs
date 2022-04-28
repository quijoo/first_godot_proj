using Godot;
using System;

public class UIItemControl : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public ColorRect rect;
    InventoryBase bag;
    bool Selected = false;
    bool ready = false;
    public bool hasItem = false;
    public Vector2 MeshPosition = Vector2.Zero;
    public InventoryItemBase CurrentInventoryItem = null;
    public override async void _Ready()
    {
        rect = GetNode<ColorRect>(nameof(ColorRect));
        await ToSignal(GetParent().GetParent(), "ready");
        bag = GetParent().GetParent() as InventoryBase;
        ready = true;
    }
    public override void _Draw()
    {
        if(!ready) return;
        if (bag.CurrentItem == this)
        {
            rect.Color = Color.ColorN("red");
            // 选中特效
        }
        else
        {
            rect.Color = Color.ColorN("green");
            // 未选中特效
        }
    }
    public override void _Notification(int what)
    {
        switch (what)
        {
            case NotificationFocusEnter:
                // Control gained focus.
                // 只在未选择的时候锁定焦点
                if(!bag.IsSelected) bag.CurrentItem = this;
                break;

            case NotificationFocusExit:
                // Control lost focus.
                break;
        }
    }
    public override void _GuiInput(InputEvent @event)
    {
        // 仅在鼠标悬浮或者焦点时触发
        if (@event is InputEventJoypadButton mbe && mbe.ButtonIndex == (int)JoystickList.Button0 && mbe.Pressed)
        {
            bag.SelectorAttach();
        }
    }
    public void Store<T>(Item<T> node) where T : Node
    {
        CallDeferred("add_child", node);
        hasItem = true;
        CurrentInventoryItem = node;
    }
    public void Unstore<T>(Item<T> node)  where T : Node
    {
        if(node.GetParent() == this)
        {
            GD.Print("trigger remove");
            CallDeferred("remove_child", node);
            hasItem = false;
            CurrentInventoryItem = null;
        }
    }
}
