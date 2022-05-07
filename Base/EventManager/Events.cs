using Godot;
using System;
/// <summary>
/// 事件命名空间（事件统一以 Event 作为结尾）
/// </summary>
namespace Events
{
	/// <summary>
	/// Event 基类， 改写 EventId
	/// </summary>
	// public abstract struct Event
	// {
	// 	public virtual int Id { get; }
	// }
    public interface IEvent
    {
        int Id { get; }
    }
	/// <summary>
	/// 击杀事件
	/// </summary>
	public struct KillEvent : IEvent
	{
		public static readonly int EventId = typeof(KillEvent).GetHashCode();
		public int Id { get => EventId; }
        public Warrior attacter;
        public Warrior victim;
        public string reason;
        public KillEvent(Warrior _attacter, Warrior _victim, string _reason)
        {
            attacter = _attacter; victim = _victim; reason = _reason;
        }
	}


    /// <summary>
	/// 击杀事件
	/// </summary>
	public struct LoadEvent : IEvent
	{
		public static readonly int EventId = typeof(LoadEvent).GetHashCode();
		public int Id { get => EventId; }
        public Player player;
        public LoadEvent(Player _player)
        {
            player = _player;
        }
	}
	/// <summary>
	/// 重生事件
	/// </summary>
	public struct RebornEvent : IEvent
    {
        public static readonly int EventId = typeof(RebornEvent).GetHashCode();
		public int Id { get => EventId; }
        public Node node;
        public Vector2 position;
        public string reason;
		public RebornEvent(Node _node, Vector2 _position, string _reason)
		{
            node = _node; position = _position; reason = _reason;
		}
    }
	/// <summary>
	/// 触墙事件
	/// </summary>
	public struct TouchWallEvent : IEvent
	{
		public static readonly int EventId = typeof(TouchWallEvent).GetHashCode();
		public int Id { get => EventId; }
		public Direction direction;
		public TouchWallEvent(Direction _direction) 
		{
			direction = _direction;    
		}
	}
    /// <summary>
	/// 攻击事件
	/// </summary>
    public struct AttackEvent : IEvent
    {
        public static readonly int EventId = typeof(AttackEvent).GetHashCode();
		public int Id { get => EventId; }
        public Warrior attacter;
        public Warrior victim;
        public float harm_value;
		public AttackEvent(Warrior _attacter, Warrior _victim, float _harm_value)
		{
            attacter = _attacter; victim = _victim; harm_value = _harm_value;
		}
    }
    /// <summary>
	/// 受伤事件
	/// </summary>
    public struct HurtEvent : IEvent
    {
        public static readonly int EventId = typeof(HurtEvent).GetHashCode();
		public int Id { get => EventId; }
        public Warrior attacter;
        public Warrior victim;
        public float harm_value;
		public HurtEvent(Warrior _attacter, Warrior _victim, float _harm_value)
		{
            attacter = _attacter; victim = _victim; harm_value = _harm_value;
		}
    }
    /// <summary>
	/// 受伤事件
	/// </summary>
    public struct PlayerHurtedEvent : IEvent
    {
        public static readonly int EventId = typeof(PlayerHurtedEvent).GetHashCode();
		public int Id { get => EventId; }
        public IAttacker attacter;
        public IHitable victim;
        public float harm_value;
		public PlayerHurtedEvent(IAttacker _attacter, IHitable _victim, float _harm_value)
		{
            attacter = _attacter; victim = _victim; harm_value = _harm_value;
		}
    }
    /// <summary>
	/// 出生事件
	/// </summary>
    public struct RespawnEvent : IEvent
    {
        public static readonly int EventId = typeof(RespawnEvent).GetHashCode();
		public int Id { get => EventId; }
        public Node node;
        public Vector2 position;
        public string reason;
		public RespawnEvent(Node _node, Vector2 _position, string _reason)
		{
            node = _node; position = _position; reason = _reason;
		}
    }
}
