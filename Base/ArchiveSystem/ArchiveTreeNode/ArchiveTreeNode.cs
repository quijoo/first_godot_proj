using Godot;
using Godot.Collections;
public abstract class ArchiveTreeNode : Node, ArchiveExecutor
{
    // 用于存档节点间的信息交流
    static public Dictionary black_board = new Dictionary();
    // 处理过程中不写入data，全部在黑板中操作，整个树完成后才写入到data中,每处理一个组清空黑板
    static public string current_group_name_key = "current_group_name_key";
    static public string current_group_node_key = "current_group_node_key";
    static public string current_data_key = "current_data_key";

    static public string  ParentPathKey = "_sys_node_parent_path";
    static public string  NodeTypeKey = "_sys_node_type";
    static public string  NodePathKey = "_sys_node_self_path";

    public abstract void SaveExecute(in Dictionary data, Node node);
    public abstract void LoadExecute(in Dictionary data);
    protected virtual Array GetCurrentNodes()
    {
        return black_board[current_group_node_key] as Array;
    }
    protected virtual Array GetCurrentData()
    {
        return black_board[current_data_key] as Array;
    }
    // 在存档树上递归调用
    public virtual void SaveHandler(string path)
    {
        // 分发当前组的 SaveExecute 任务（Root需要单独重写，即任务的配对）
        Array nodes = (black_board[current_group_node_key]) as Array;
        Array datas = (black_board[current_data_key]) as Array;
        for(int i = 0; i < datas.Count; i++)
        {
            SaveExecute(datas[i] as Dictionary, nodes[i] as Node);
        }

        if(path == "") return; 
        if(path.IndexOf("/") != -1)
        {
            GetNode<ArchiveTreeNode>(path.Substring(0, path.IndexOf("/"))).SaveHandler(path.Substring(path.IndexOf("/") + 1));
        }
        else
        {
            GetNode<ArchiveTreeNode>(path).SaveHandler("");
        }
        
    }
    public virtual void LoadHandler(string path)
    {
        // 分发当前组的 LoadExecute 任务（Root需要单独重写，即任务的配对）
        Array datas = black_board[current_data_key] as Array;
        for(int i = 0; i < datas.Count; i++)
        {
            LoadExecute(datas[i] as Dictionary);
        }
        // 计算以一个处理节点
        if(path == "") return; 
        if(path.IndexOf("/") != -1)
        {
            GetNode<ArchiveTreeNode>(path.Substring(0, path.IndexOf("/"))).LoadHandler(path.Substring(path.IndexOf("/") + 1));
        }
        else
        {
            GetNode<ArchiveTreeNode>(path).LoadHandler("");
        }
    }
}

