/// <summary>
/// 事件管理器类
/// 方法 Send, Subscribe, Unsubscribe
/// 引擎自带事件: 使用原本的 [Signal]
/// 自定义事件: 使用新事件 
/// 1. 在 Events 中编写事件参数类（注意修改EventId）
/// 2. 在事件发生处添加 EventManager.Send(Node sender, new EventName(..., ..., ...))
/// 3. 在订阅事件的对象处 _EnterTree() / _ExitTree() 中编写 Subscribe/Unsubscribe 方法
/// 4. 回调函数格式 void CallbackFuncName(Node sender, Event e) 这里需要在函数体中对 e 做类型转换
/// tips: 在出现消息相关的空引用时，首先检查是否在对象的生命周期结束前未调用 Unsubscribe 方法
/// </summary>
using System.Collections.Generic;
using Godot;
using Events;
public delegate void EventCallback(Node sender, IEvent e);
public class EventManager : Node
{
    static private Dictionary<int, EventCallback> CallDic = new Dictionary<int, EventCallback>();
    static public EventManager instance;
    public override void _Ready()
    {
        base._Ready();
        instance = this;
    }
    /// <summary>
    /// 发送事件消息
    /// </summary>
    /// <param name="sender">发送者对象（is a Node）</param>
    /// <param name="e">事件参数（T 的实例）</param>
    /// <typeparam name="T">泛型事件参数（继承自 Event）</typeparam>
    static public void Send<T>(Node sender, T e) where T : IEvent
    {
        if (CallDic.ContainsKey(e.Id))
        {
            (CallDic[e.Id])?.Invoke(sender, e);
        }
    }
    /// <summary>
    /// 订阅事件消息
    /// </summary>
    /// <param name="node">订阅者对象（is a Node）</param>
    /// <param name="callback">回调函数</param>
    /// <typeparam name="T">泛型事件参数</typeparam>
    static public void Subscribe<T>(Node node, EventCallback callback) where T : IEvent
    {
        int key = typeof(T).GetHashCode();
        if (CallDic.ContainsKey(key))
        {
            CallDic[key] += callback;
        }
        else
        {
            CallDic[key] = callback;
        }
    }
    /// <summary>
    /// 取消订阅事件消息（在对象 queue_free() 之前需要取消订阅，否则会出现空引用）
    /// </summary>
    /// <param name="node">订阅者对象（is a Node）</param>
    /// <param name="callback">回调函数</param>
    /// <typeparam name="T">泛型事件参数</typeparam>
    static public void Unsubscribe<T>(Node node, EventCallback callback) where T : IEvent
    {
        int key = typeof(T).GetHashCode();
        if (CallDic.ContainsKey(key))
        {
            EventCallback res = CallDic[key] - callback;
            if (res == null)
            {
                CallDic.Remove(key);
            }
            else
            {
                CallDic[key] = res;
            }
            
        }
    }
}