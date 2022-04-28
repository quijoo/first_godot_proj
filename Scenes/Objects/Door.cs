using Godot;
using System;

public class Door : Area2D
{
	// 之后在 scenmemanager 中建立门与场景的名字映射
	[Export] private string next_scene_name;
	
	public override void _Ready()
	{
		
	}

	public void _on_Door_body_exited(Node node)
	{
		if(node is Player)
		{
			Node scene = SceneManager.LoadScene(next_scene_name);
			SceneManager.MoveToScene(node, scene);
			SceneManager.ChangeScene(next_scene_name);
		}
	}
}
