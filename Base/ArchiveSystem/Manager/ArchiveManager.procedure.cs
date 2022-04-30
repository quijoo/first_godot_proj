using System.Collections;
using Godot;
using Godot.Collections;
namespace Archive
{
    // 加载/保存某个存档组节点的具体方法，每次修改最小单位为 组，即某个场景下某个组的全部节点
    public partial class ArchiveManager
    {
        private string SceneNamePrefix = "scene_{0}_{1}_data";
        private void Save(GameSave saver, string name)
        {
            void leaf_process(Node node)
            {
                ArchiveTreeNode root = GetNode<ArchiveTreeNode>("Root");
                ArchiveTreeNode.black_board[ArchiveTreeNode.current_group_name_key] = node.Name;
                root.SaveHandler(root.GetPathTo(node));
                saver.SceneMap[string.Format(SceneNamePrefix, SceneManager.SceneName, node.Name)] = ArchiveTreeNode.black_board[ArchiveTreeNode.current_data_key] as Array;
            }
            dfs(NodeMap[name], (Node node)=>{}, leaf_process, (Node node)=>{});       
        }
        private void Load(GameSave saver, string name)
        {
            void leaf_process(Node node)
            {
                ArchiveTreeNode root = GetNode<ArchiveTreeNode>("Root");
                ArchiveTreeNode.black_board[ArchiveTreeNode.current_group_name_key] = node.Name;
                ArchiveTreeNode.black_board[ArchiveTreeNode.current_data_key] = saver.SceneMap[string.Format(SceneNamePrefix, SceneManager.SceneName, node.Name)];

                root.LoadHandler(root.GetPathTo(node));
            }
            if(!NodeMap.ContainsKey(name))
            {
                GetTree().Root.GetNode<CanvasLayer>("Console").Call("write_line", $"[color=<red>]error[/color] archive node name {name}");
                return;
            }
            dfs(NodeMap[name], (Node node)=>{}, leaf_process, (Node node)=>{});           
        }
    }
}