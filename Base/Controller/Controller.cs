using Godot;
using System;
using System.Collections.Generic;
 
public abstract class Controller : KinematicBody2D
{
	[Export] public float walk_speed    = 75;
    // Jump
    [Export] public float MaxJumpHeight = 24.0f;
    [Export] public float MaxJumpTime   = 0.4f;
    [Export] public float LowJumpMultiplier = 2f;
    public float FixedGravity { get; private set;}
    public float JumpSpeed { get; private set;}
    [Export] public float MaxFallSpeed = 200;
    //wall slide
    [Export] public float WallSlideGravity = 100f;
    // Wall Jump
    [Export] public float WallJumpLockTime = 0.1f;
    [Export] public Vector2 WallJumpSpeed = new Vector2(-100, -100);
    [Export] public Vector2 WallClimbSpeed = new Vector2(-200, -200);
    [Export] public Vector2 SuperWallJumpSpeed = new Vector2(-200, -200);



    // Grace Time
    [Export] private float DefaultGraceTimer = 0.1f;
    public float GraceTimer { get; private set; }

    // jump buffer
    [Export] private float DefaultJumpBufferTimer = 0.1f;
    public float JumpBufferTimer;
	// position state parameters
	public Vector2 velocity;
	public Direction direction = Direction.RIGHT;
	// cache
	public AnimationNodeStateMachinePlayback animation_state;
    public AnimationPlayer anim;
	public CollisionShape2D collision_shape;

	// raycasts 每条边最多 32 条射线, mask检测
	[Export] private PackedScene RayCastScene;
    [Export] public float ProbeLength = 0.2f;
    private float delta = 2f;
	List<UserDefineRayCast2D> RayList = new List<UserDefineRayCast2D>();
	[Export] public int HorizontalRayNum = 2;
	[Export] public int VerticalRayNum = 3;
    [Export] private bool ShowBox = false;
    [Export] private Color BoxColor = Color.ColorN("White");
    private Rect2 collider_rect;
    private WarroirCollision _collision;
    public WarroirCollision collision { get => _collision; }

	// Handle slopes
	[Export] public int snap_length = 2;
	[Export] public Vector2 snap_direction = Vector2.Down;
	[Export] public Vector2 snap_vector;
	[Export] public float floor_max_angle = Mathf.Deg2Rad(70f);
	
	// AnimationTree relate.
	private Dictionary<string, int> AnimationStateDict = new Dictionary<string, int>();
	public enum AnimationState : int
	{
		Error = -1,
		Running = 0,
		Stop,

	}
#region Api_func
	/// <summary>
	/// if left input > right input, return a positve float.
	/// </summary>
	/// <returns></returns>
	public virtual void Dead(string reason)
	{
		GD.PrintErr("Unimplement function Dead.");
	} 
    public virtual void Reborn()
    {
        
    }
	/// <summary>
	/// Only called by _PhysicsProcess / _PhysicsUpdate
	/// </summary>
	/// <returns></returns>
	
#endregion
	
#region Engine life cycle function

	public override void _Ready()
	{
        // animation
        anim = GetNode<AnimationPlayer>(nameof(AnimationPlayer));
		// get chache.
		animation_state = (AnimationNodeStateMachinePlayback)GetNode<AnimationTree>("AnimationTree").Get("parameters/playback");
		collision_shape = GetNode<CollisionShape2D>(nameof(CollisionShape2D));
		// Init slopes attr
		snap_direction = snap_direction * snap_length;
		// init raycast
        _collision = new WarroirCollision(HorizontalRayNum, VerticalRayNum);
        GenerateRayCast();
        RectangleShape2D shape = collision_shape.Shape as RectangleShape2D;
        collider_rect = new Rect2(collision_shape.Position - shape.Extents, shape.Extents * 2);
        // character controller relate.
        FixedGravity = 2 * MaxJumpHeight / Mathf.Pow(MaxJumpTime, 2);
        JumpSpeed = -2 * MaxJumpHeight / MaxJumpTime;

        //grace time
        GraceTimer = 0;
	}
    public override void _Draw()
    {
        if(ShowBox)
        {
            DrawRect(collider_rect, BoxColor);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        UpdateRayCollisionInfo();
        // jump buffer
        if(Input.IsActionJustPressed("jump")) JumpBufferTimer = DefaultJumpBufferTimer;
        if(JumpBufferTimer > 0) JumpBufferTimer-=delta;
        // grace timer
        if(velocity.y < -0.01f && !collision.Null(Direction.DOWN))
        {
            GraceTimer = 0;
        }
        if(collision.AnyDected(Direction.DOWN))
        {
            GraceTimer = DefaultGraceTimer;
        }
        else
        {
            GraceTimer -= delta;
        }
    }
#endregion

#region private_function
        /// <summary>
    /// Only call in _Ready() func;
    /// </summary>
	private void GenerateRayCast()
	{
        RectangleShape2D shape = collision_shape.Shape as RectangleShape2D;
		float half_width = shape.Extents.x;
		float half_height = shape.Extents.y;

		float horizontal_delta =  2f * (half_width - delta) / (float)(HorizontalRayNum - 1);
		float vertical_delta =  2f * (half_height - delta) / (float)(VerticalRayNum - 1);

		UserDefineRayCast2D ray;

        // set ray.
        for(int index = 0; index < 2 * (VerticalRayNum + HorizontalRayNum); index++)
        {
            ray = RayCastScene.Instance() as UserDefineRayCast2D;
            if(index < VerticalRayNum)
            {
                // left = 0
                ray.Position = new Vector2(-half_width, -half_height + delta + vertical_delta * (float)index);
                ray.CastTo = new Vector2( -ProbeLength, 0f) ;
            }
            else if(index < 2 * VerticalRayNum)
            {
                // right = 1
                ray.Position = new Vector2(half_width, -half_height + delta + vertical_delta * (float)(index - VerticalRayNum));
                ray.CastTo = new Vector2(ProbeLength, 0f) ;
            }
            else if(index < 2 * VerticalRayNum + HorizontalRayNum)
            {
                // down = 2
                ray.Position = new Vector2(-half_width + delta + horizontal_delta * (float)(index - 2 * VerticalRayNum), half_height);
                ray.CastTo = new Vector2(0, ProbeLength);
            }
            else
            {
                // up = 3
                ray.Position = new Vector2(-half_width + delta + horizontal_delta * (float)(index - 2 * VerticalRayNum - HorizontalRayNum), -half_height);
                ray.CastTo = new Vector2(0f, -ProbeLength);
            }
            AddChild(ray);
            RayList.Add(ray);                
        }
	}
    private void UpdateRayCollisionInfo()
    {
        _collision.Clear();
        uint mask= 0b1;
        // left right
        for(int index = 0; index < VerticalRayNum; index++)
        {
            if(RayList[index].IsColliding()) _collision.left |= mask;
            if(RayList[VerticalRayNum + index].IsColliding()) _collision.right |= mask;
            mask <<= 1;
        }
        // down up
        mask = 0b1;
        for(int index = 0; index < HorizontalRayNum; index++)
        {
            if(RayList[2 * VerticalRayNum + index].IsColliding()) _collision.down |= mask;
            if(RayList[2* VerticalRayNum + HorizontalRayNum + index].IsColliding()) _collision.up |= mask;
            mask <<= 1;
        } 
    }
#endregion

#region Api_func
    public UserDefineRayCast2D GetRay(Direction direct, int index)
    {
        // 检查约束
        if(index < 0) return null;
        if(index > VerticalRayNum && (direct.Match(Direction.LEFT) || direct.Match(Direction.RIGHT))) return null;
        if(index > HorizontalRayNum && (direct.Match(Direction.UP) || direct.Match(Direction.DOWN))) return null;

        // 查询
        switch(direct)
        {
            case Direction.LEFT:    return RayList[index];
            case Direction.RIGHT:   return RayList[VerticalRayNum + index]; 
            case Direction.DOWN:    return RayList[2 * VerticalRayNum + index];
            case Direction.UP:      return RayList[2* VerticalRayNum + HorizontalRayNum + index];
            default:
                return null;
        }
    }
    public abstract void UpdateDirection();
#endregion

#region Physics_Handler_func

	/// <summary>
	/// Process gravity, Only call in _PhysicsProcee func
	/// </summary>
	/// <param name="delta">Physics 帧间隔</param>
	public void GravityControllHandler(float _gravity, float delta)
	{
		velocity.y += _gravity * delta;
	}

	/// <summary>
	/// Process collision like slope collision. note this method will produce 5 times collision by engine. Only call in _PhysicsProcee func
	/// </summary>
	public void SnapControlHandler()
	{
		velocity = MoveAndSlideWithSnap(
			velocity,    snap_vector,     Vector2.Up,
			true, 4,            floor_max_angle, false);
	}
	
#endregion

#region Callback_func
	public void SetAnimationState(string AttrName, int Value)
	{
		AnimationStateDict[AttrName] = Value;
	}
	public int GetAnimationState(string AttrName)
	{
		if(AnimationStateDict.ContainsKey(AttrName))
		{
			return AnimationStateDict[AttrName];
		}
		else
		{
			return (int)AnimationState.Error;
		}
	}

#endregion
}
