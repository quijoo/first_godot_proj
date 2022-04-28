using Godot;
using System;
using Godot.Collections;
public delegate void VoidFunction();
public class SceneManager : Node
{
	static public SceneTree CurrentTree;
    static public Dictionary<string, PackedScene> PackedSceneDictionary;
    [Export] private Dictionary<string, PackedScene> _PackedSceneDictionary = new Dictionary<string, PackedScene>();
    static private Dictionary<string, Node> LoadedSceneNodeDictionary = new Dictionary<string, Node>();
    // 储存场景的循环队列
    static private int CacheLoadSceneNum = 5; 
    static private CircleQueue<string> LoadedSceneNameQueue;
    public static bool IsReady = false;
    public static string SceneName;
    // 初始化
    [Export] public string FirstSceneName;
    [Export] PackedScene PlayerPacked;
    static private SceneManager instance;
	public override async void _Ready()
	{
		CurrentTree = GetTree();
        LoadedSceneNameQueue = new CircleQueue<string>(CacheLoadSceneNum);
        PackedSceneDictionary = _PackedSceneDictionary;
        
        await ToSignal(GetTree().Root, "ready");
        SceneName = FirstSceneName;
        instance = this;
        IsReady = true;
        Player player = PlayerPacked.Instance<Player>();

        Node scene = LoadScene(FirstSceneName);

        MoveToScene(player, scene);
        ChangeScene(FirstSceneName);
	}
#region Api
    public Array<string> GetSceneList()
    {
        if(!IsReady) return null;
        Array<string> res = new Array<string>();
        foreach(var key in PackedSceneDictionary.Keys)
        {
            res.Add(key);
        }
        return res;
    }
    // 移动物体到场景，根据类型扩充方法列表
	static public void MoveToScene(Node node, Node scene)
	{
        if(CurrentTree == null || !IsReady) return;
			// 添加到场景
        if(node is Player)
        {
            if(node.GetParent() != null)
            {
                node.GetParent().RemoveChild(node);
            }
            ((Player)node).Position = Vector2.Zero;
            instance.CallDeferred("AddPlayer", scene, node);
        }
	}
    private void AddPlayer(Node scene, Node player)
    {
        {scene.GetNode<Position2D>("PlayerPosition").AddChild(player);}
    }
    // 加载场景到缓存
    static public Node LoadScene(string scene_name)
	{
        if(!IsReady) return null;
        // 已经加载的场景直接 Load
        if(LoadedSceneNodeDictionary.ContainsKey(scene_name))
        {
            return LoadedSceneNodeDictionary[scene_name];
        }
        else if(PackedSceneDictionary.ContainsKey(scene_name))
        {
            // 清除最久未使用的场景
            if(LoadedSceneNameQueue.Full)
            {
                string deleteKey = LoadedSceneNameQueue.Dequeue();
                Node node = LoadedSceneNodeDictionary[deleteKey];
                if(!deleteKey.Empty()) LoadedSceneNodeDictionary.Remove(deleteKey);
                // TODO：调用一次存档
                node.QueueFree();
            }
            Node scene = PackedSceneDictionary[scene_name].Instance<Node>();
            LoadedSceneNodeDictionary[scene_name] = scene;
            LoadedSceneNameQueue.Enqueue(scene_name);
            instance.GetTree().Root.GetNode("MainMap").CallDeferred("add_child", scene);
            // scene.Name = scene_name;
            return scene;
        }
        return null;
        
	}
    static public Node GetScene(string scene_name)
	{
		if(PackedSceneDictionary.ContainsKey(scene_name))
        {
            return LoadedSceneNodeDictionary[scene_name];
        }
        return null;
	}
    static public void ChangeScene(string scene_name)
	{
        if(!IsReady) return;
        if(GetScene(scene_name) == null) return;

        (GetScene(SceneName) as Node2D).Visible = false;
        (GetScene(scene_name) as Node2D).Visible = true;
        SceneName = scene_name;
    }
#endregion
}
