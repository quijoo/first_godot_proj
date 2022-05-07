using Godot;
using System;
// StateMachine's state node interface.
// 每一个 Controller 对象都应该拥有一个 StateMachine 对象
public abstract class StateNode<T> : StateBase, IState where T : KinematicBody2D
{
    // State node's _machine is it's parent.
    protected T target;
    public override async void _Ready()
    {
        await ToSignal(Owner, "ready");
        target = Owner as T;
    }
    public void Init(StateMachine machine)
    {
        _machine = machine;
    }
    // Call by it's statemachine object.
    public virtual void _HandleInput(InputEvent _event)
    {

    }
    // call by it's statemachine's _process()
    public virtual void _Update(float delta)
    {
        
    }
    // call by it's statemachine's physics_process()
    public virtual void _PhysicsUpdate(float delta)
    {
        
    }
    public virtual void Enter()
    {
        
    }
    public virtual void Exit()
    {
        
    }
}
