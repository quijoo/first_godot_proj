using Godot;

using Godot.Collections;
public sealed class ManagerGroup : ArchiveTreeNode
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
        Archivable node = GetNode<Archivable>(data[NodePathKey].ToString());
        node.Load(data);
    }
    // public override void LoadeHandler(Array nodes, in Array<Dictionary> array)
    // {
    //     foreach(var data in array)
    //     {
    //         // 默认延迟调用，如需直接调用，请重写该方法
    //         LoadExecute((Dictionary) data);
    //     }
    // }
}
