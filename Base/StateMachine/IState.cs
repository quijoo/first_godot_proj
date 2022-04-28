using Godot;
using System;

public interface IState
{
    void _HandleInput(InputEvent _event);
    // call by it's statemachine's _process()
    string Name {get;}
    void _Update(float delta);
    void _PhysicsUpdate(float delta);
    void Enter();
    void Exit();
    void Init(StateMachine machine);
}
