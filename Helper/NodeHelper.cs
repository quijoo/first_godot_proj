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
}
