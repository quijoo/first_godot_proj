using Godot;
using System;
using Events;
public partial class Enemy
{
	// 将时间的监听写在该文件
	public override void RigisterEvents()
	{
		EventManager.Subscribe<HurtEvent>(this, HurtEventHandler);
	}
	public override void UnrigisterEvents()
	{
		EventManager.Unsubscribe<HurtEvent>(this, HurtEventHandler);
	}
	public void HurtEventHandler(Node node, IEvent e)
	{
		HurtEvent ee  = (HurtEvent)e;
		GD.Print("[HurtEvent]", ee.attacter, " ", ee.victim);
	}
}
