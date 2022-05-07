using Godot;
using System.Collections.Generic;
using Events;


// [Tool]
public class InteractiveWindow : Node2D
{
	// window local position
	[Export] PackedScene bullet;
    [Export(PropertyHint.Layers2dPhysics)] public uint MoveMask;
    [Export(PropertyHint.Layers2dPhysics)] public uint MoveLayer;
    [Export(PropertyHint.Layers2dPhysics)] public uint FollowMask;
    [Export(PropertyHint.Layers2dPhysics)] public uint FollowLayer;
    [Export(PropertyHint.Layers2dPhysics)] public uint LockMask;
    [Export(PropertyHint.Layers2dPhysics)] public uint LockLayer;

	ShaderMaterial shader;
	public InteractiveBorder border;
    Main screen;
    // canvas + window related
    WindowDialog dialog;
    CanvasLayer canvas;
    // screen effects
    Tween tween;
    RandomNumberGenerator _random = new RandomNumberGenerator();
    Queue<InteractiveAnchor> queue = new Queue<InteractiveAnchor>();
    // mouse related
    public bool IsMouseIn = false;
    private bool NeedToFindPlayer = true;
    // physics's disabled area
    Area2D area;
    RectangleShape2D shape;

    [Export] Vector2 SoftMargin = new Vector2(5, 5);
	public override void _Ready()
	{		
		border = GetNode<InteractiveBorder>("Border");
		// 获取边界物理体
        dialog = canvas.GetNode<WindowDialog>("WindowDialog");
        dialog.PopupExclusive = true;
        // 连接相关信号
        dialog.Connect("mouse_entered", this, "OnMouseEntered");
        dialog.Connect("mouse_exited", this, "OnMouseExited");

        dialog.Connect("gui_input", this, "OnWindowGuiInput");
        // screen shake
        tween = GetNode<Tween>("WindowShake");
        area = GetNode<Area2D>("Area2D");
        shape = area.GetNode<CollisionShape2D>("CollisionShape2D").Shape as RectangleShape2D;
	}
    public override void _EnterTree()
    {
        NeedToFindPlayer = true;
        screen = GetTree().Root.GetNode<Main>("Main");
        canvas = GetNode<CanvasLayer>("InteractiveWindowLayer");
        this.CallDeferred("remove_child", canvas);
        screen.CallDeferred("add_child", canvas);
    }
    public override void _ExitTree()
    {

        CallDeferred("_DeferredPrograce");        
        tween.StopAll();
    }
    void _DeferredPrograce()
    {
        screen.RemoveChild(canvas);
        screen.ResetScreen();
        this.AddChild(canvas);
    }
	
	public override void _PhysicsProcess(float delta)
	{
        if(NeedToFindPlayer)
        {
            // 在第一帧锁定 Player位置
            GD.Print("lock player");
            border.ChangeProcess(false);
            border.Reset(BasicFollowCamera.Target.GlobalPosition);
            border.ChangeProcess(true);
            NeedToFindPlayer = false;
        }
		// 1. window 更新 + 可视区剪裁
		screen.SetDisplayRect(border.Start, border.End);
        // 2. windowdialog 对齐
        dialog.Popup_(border.rect.Clip(screen.viewport.GetVisibleRect()));
        // 3. 物理开启区域对齐
        area.Position = border.window.Position - SoftMargin;
        shape.Extents = border.window.Size +  SoftMargin * 2;

	}
    public void OnWindowGuiInput(InputEvent inputEvent)
    {
        if (inputEvent is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{

			switch ((ButtonList)mouseEvent.ButtonIndex)
			{
				case ButtonList.Left:
				{					
					InteractiveAnchor anchor = bullet.Instance<InteractiveAnchor>();
					anchor.GlobalPosition = GetGlobalTransform().AffineInverse() * GetViewportTransform().AffineInverse() * (border.Start + mouseEvent.Position);
					this.CallDeferred("add_child", anchor);
                    // anchor.CallDeferred("Init", border.Start + mouseEvent.Position - BasicFollowCamera.Target.ScreenPosition, (object)EffectCallback);
                    anchor.Init((border.Start + mouseEvent.Position - BasicFollowCamera.Target.ScreenPosition), EffectCallback);
					break;
				}    
				case ButtonList.WheelUp:
					break;
			}
		}
    }
    // mouse event
    public void OnMouseEntered() => IsMouseIn = true;
    public void OnMouseExited() => IsMouseIn = false;
    // delete callback
    public void EffectCallback(InteractiveAnchor anchor)
    {
        // 窗口动画 + 删除节点
        // func _ready():
        //      camera = get_node_or_null(camera_path)

        // func disturb_offset(strength : float):
            // camera.h_offset = rand_range(-strength,strength)
            // camera.v_offset = rand_range(-strength,strength)
        float duration = 3f;
        float strength = 10f;
        tween.InterpolateMethod(this, "disturb_offset", strength, 0, duration, Tween.TransitionType.Sine, Tween.EaseType.Out, 0);
        tween.Start();
        queue.Enqueue(anchor);
    }
    void _on_WindowShake_tween_all_completed()
    {
        GD.Print("all end shake");
    }
    void _on_WindowShake_tween_completed(Object _object, NodePath path)
    {
        queue.Dequeue().QueueFree();
        GD.Print("end shake");
    }
    void disturb_offset(float _strength)
    {
        dialog.RectPosition += new Vector2(_random.RandfRange(-_strength, _strength), _random.RandfRange(-_strength, _strength));
        screen.GetNode<TextureRect>("CanvasLayer/_ScreenTexture").RectPosition += new Vector2(_random.RandfRange(-_strength, _strength), _random.RandfRange(-_strength, _strength));
    }

}
