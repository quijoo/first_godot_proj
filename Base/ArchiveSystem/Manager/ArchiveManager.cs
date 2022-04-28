using System;
using Godot;
using System.Collections.Generic;
/*----------------------------------------------------------------
// Copyright (C) 2022 Xuaii
// 版权所有。
//
// 文件名：Mytest.cs
// 功能描述：存档管理器
// 组名采用 Archive_{Type}_{GroupName} 例如: Archive_Level1_Manager, ArchiveGroup_Global_Player 等
// 所有被管理的组都应在管理器中注册，用组的 Type 字段可以区分不同场景和全局对象，每次读档和存档时，管理器都将按组在管理器中的注册顺序依次执行
//----------------------------------------------------------------*/

namespace Archive
{
    public partial class ArchiveManager : Node
    {   
        // 使用 Resource 的方式存档
        [Export] private Script GameSave;
        // public string SAVE_FOLDER = OS.GetUserDataDir() + ProjectSettings.GetSetting("application/config/custom_user_dir_name");
        static private ArchiveManager instance = null;
        static private Dictionary<string, ArchiveTreeNode> NodeMap = new Dictionary<string, ArchiveTreeNode>();
        public delegate void NodeProcess(Node node);
        public override async void _Ready()
        {
            await ToSignal(GetTree().CurrentScene, "ready");
            GD.Print("[ArchiveManager] scene load ok");
            instance = this;
            void process(Node node)
            {
                if(node is ArchiveTreeNode) 
                {
                    NodeMap[node.Name] = node as ArchiveTreeNode;
                }
            }
            dfs(GetNode<ArchiveTreeNode>("Root"), process, (Node node) => {}, (Node node) => {});
        }
        private void dfs(Node node, NodeProcess pre_process, NodeProcess leaf_process, NodeProcess post_process)
        {
            pre_process(node);
            if(node.GetChildCount() == 0)
            {
                leaf_process(node);
                return;
            }
            foreach(Node child in node.GetChildren())
            {
                dfs(child, pre_process, leaf_process, post_process);
            }
            post_process(node);
        }
        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);
            // Just for test
            if(Input.IsActionJustPressed("test"))
            {
                LoadGame(0, "Root");
            }
        }
    }
}

