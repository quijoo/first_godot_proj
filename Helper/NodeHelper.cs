using Godot;
public static class ExtendNode2D{
	/// <summary>
	/// 转换为当前节点同级 Local 坐标，然后计算
	/// </summary>
	/// <param name="node"></param>
	/// <param name="other"></param>
	/// <returns></returns>
	static public Vector2 VectorTo(this Node2D node, Node2D other)
	{
		if(other == null)
		{
			GD.Print("get null point");
			return Vector2.Zero;
		}
		return node.GetParent<Node2D>().ToLocal(other.GlobalPosition) - node.Position;
	}
    static public void ChangeProcess(this Node node, bool flag, uint mask = 0b0)
    {
        void dfs(Node root, bool flag, uint _mask)
        {
            // 只暂停相关层
            if(root == null) return;
            if(root is PhysicsBody2D && ((root as PhysicsBody2D).CollisionLayer & _mask) != 0b0)
            {
                root.SetPhysicsProcess(flag);
                root.SetProcess(flag);
                root.SetProcessInput(flag);
                root.SetProcessInternal(flag);
                root.SetPhysicsProcessInternal(flag);
                root.SetProcessUnhandledInput(flag);
                root.SetProcessUnhandledKeyInput(flag);
            }
            foreach(Node child in root.GetChildren())
            {
                dfs(child, flag, _mask);
            }
        }
        dfs(node, flag, mask);
    }
}
