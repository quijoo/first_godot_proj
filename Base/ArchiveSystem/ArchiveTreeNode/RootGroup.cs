using Godot;
using Godot.Collections;
// Root 节点不要随意修改
public class Pair<T1, T2> 
where T1 : class 
where T2 : class
{
    public T1 node;
    public T2 data;
    public Pair(T1 t1, T2 t2)
    {
        node = t1;
        data = t2;
    }
}
public sealed class RootGroup : ArchiveTreeNode
{
    
    public override void _Ready()
    {
        
    }
    public override void SaveHandler(string path)
    {
        // 获取待存档节点组
        string groupname = black_board[current_group_name_key] as string;
        Array nodes = GetTree().GetNodesInGroup(groupname);
        Array savenodes = new Array();
        foreach(Node node in nodes)
        {
            if(SceneManager.GetScene(SceneManager.SceneName).IsAParentOf(node))
            {
                savenodes.Add(node);
            }
        }
        black_board[current_group_node_key] = savenodes;
        // 构造待存档数据（空）
        Array<Dictionary> datas = new Array<Dictionary>();
        black_board[current_data_key] = datas;

        foreach(Node node in savenodes)
        {
            datas.Add(new Dictionary());
        }
        base.SaveHandler(path);

    }
    public override void LoadHandler(string path)
    {
        // 获取待读档节点组（这里有个问题：当前的场景存在的节点可能和存档中的节点完全不一样）
        string groupname = black_board[current_group_name_key] as string;
        Array nodes_temp = GetTree().GetNodesInGroup(groupname);
        Array nodes = GetTree().GetNodesInGroup(groupname);

        var data = black_board[current_data_key] as Array;
        // 节点配对算法
        foreach(var info in data)
        {
            string node_path = (info as Dictionary)[NodePathKey].ToString();
            Node node = GetNodeOrNull(node_path);
            if(node != null) nodes_temp.Remove(node);
        }
        foreach(Node node in nodes_temp)
        {
            if(!SceneManager.GetScene(SceneManager.SceneName).IsAParentOf(node))
            {
                continue;
            }
            node.Free();
        }
        base.LoadHandler(path);
    }
    public override void SaveExecute(in Dictionary data, Node node)
    {

    }
    public override void LoadExecute(in Dictionary data)
    {
    }
}
