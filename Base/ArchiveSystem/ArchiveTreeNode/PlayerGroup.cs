using Godot;
using System;
using Godot.Collections;
public sealed class PlayerGroup : ArchiveTreeNode
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    


    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    public override void SaveExecute(in Dictionary data, Node node)
    {
        data[NodePathKey] = node.GetPath();
        (node as Archivable).Save(data);
    }
    public override void LoadExecute(in Dictionary data)
    {
        SceneManager.MoveToScene(BasicFollowCamera.Target, SceneManager.GetScene(SceneManager.SceneName));
        CallDeferred("CallNodeLoad", data[NodePathKey].ToString(), data);
    }
    public void CallNodeLoad(string path, Dictionary data)
    {
        GetNode<Node>(path).CallDeferred("Load", data);
    } 
}
