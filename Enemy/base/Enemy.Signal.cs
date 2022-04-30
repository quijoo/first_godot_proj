using Godot;
using System;
using Events;
public partial class Enemy
{

    // 这里检测不到 Player 的 on_Hitbox_area_entered ，因为player的恭喜区域是关闭的（在未按攻击键之前）
    private void _on_Hitbox_body_entered(Node body)
    {
        if(body is Player)
        {
            // EventManager.Send(this, new Events.AttackEvent(this, (Warrior)body, ForceValue));
            EventManager.Send(this, new KillEvent(this, (Warrior)body, "Touch enemy"));
            // 1. 将屏幕特效和声音统一的为一个 Effectback 对象
            // 2. Effectback对象有一个 Play 方法， 当触发事件的时候 调用相应的 Play
            // 所有的 Effectback 由一个 EffectbackManager 对象管理， 其他对象通过名字调用获取到 Effectback，然后调用 Play
            // 这样所有的 Effectback 都能由 Manager 统一加载，它的加载将 统一的 在代码中使用明确的地址加载 [Export] List<NodePath> 或者 [Export] List<PackedScene>
            (body as Player).OnHit(this, "no reason");
        }
    }
}
