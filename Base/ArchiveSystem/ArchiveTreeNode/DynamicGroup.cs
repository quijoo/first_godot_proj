using Godot;
using Godot.Collections;
public sealed class DynamicGroup : ArchiveTreeNode
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    static string  NodeNameKey = "_sys_node_self_name";
    static string  SceneFileNameKey= "_sys_node_packedscene_filename";
    public override void _Ready()
    {
        
    }
    public override void SaveExecute(in Dictionary data, Node node)
    {
        data[NodeNameKey] = node.Name;
        data[SceneFileNameKey] = node.Filename;
        data[ParentPathKey] = node.GetParent().GetPath();
        (node as Archivable).Save(data);
    }
    public override void LoadExecute(in Dictionary data)
    {
        PackedScene newObjectScene = (PackedScene)ResourceLoader.Load(data[SceneFileNameKey].ToString());
        Archivable node = newObjectScene.Instance<Archivable>();
        GetNode(data[ParentPathKey].ToString()).AddChild((Node)node);
        node.Name = data[NodeNameKey].ToString();
        (node as Archivable).Load(data);
    }
    // public override void LoadeHandler(Array nodes, in Array<Dictionary> array)
    // {
    //     foreach (Node node in nodes)
    //     {
    //         node.Free();
    //     }
    //     base.LoadeHandler(nodes, array);
    // }
}
