using Godot;
using System;
using System.Collections.Generic;
// Define the signal of state changing.

public class StateMachine : Node
{
	[Export] public NodePath initial_state_path;
	public IState state;
	private Dictionary<string, System.Object> state_info = new Dictionary<string, System.Object>();
	[Signal] public delegate void transitioned(string state_name);

	public override async void _Ready()
	{
		state = GetNode<IState>(initial_state_path);
			 
		await ToSignal(Owner, "ready");
		
		foreach(IState child in GetChildren())
		{
			child.Init(this);
		}
		state.Enter();
	}

	public override void _UnhandledInput(InputEvent _event)
	{
		state._HandleInput(_event);
	}

	public override void _Process(float delta)
	{
		state._Update(delta);
	}
	public override void _PhysicsProcess(float delta)
	{
		state._PhysicsUpdate(delta);
	}
	#region Api
	public void Transition<T>() where T : class, IState
	{
		// make sure that "state name = state type"
		GD.Print("[StateMachine] ", typeof(T).ToString());
		string node_path = typeof(T).ToString();
		if(!HasNode(node_path))
		{
			return;
		}

		state.Exit();
		state = GetNode<T>(node_path);
		state.Enter();
		EmitSignal("transitioned", node_path);
	}
    public void Transition(string node_path)
	{
		// make sure that "state name = state type"
		GD.Print("[StateMachine] ", node_path);
		if(!HasNode(node_path))
		{
			return;
		}

		state.Exit();
		state = GetNode<IState>(node_path);
		state.Enter();
		EmitSignal("transitioned", node_path);
	}
	public void SetInfo(string name, System.Object info)
	{   
		state_info[name] = info;
	}
	public System.Object GetInfo(string name)
	{
		return state_info[name];
	}
	public T GetInfo<T>(string name) 
	{
		return (T)state_info[name];
	}
	#endregion
	
}
