using Godot;
using System.Collections.Generic;
public class BossDash : StateNode<Boss_1>
{
    public enum AnimationState
    {
        Error = -1,
        Running = 0,
        Stop,
    }
    private List<Vector2> positions = new List<Vector2>();
    private int index = 0;
    private float speed = 150f;
    public override void Enter()
    {
        // 开启动画
        target.animation_state.Travel("Run");    
        // 计算冲刺点
        positions.Clear();
        index = 0;
        foreach(var ray in target.NavigationRay)
        {
            if(ray.IsColliding())
            {
                positions.Add(target.GetParent<Node2D>().ToLocal(ray.GetCollisionPoint()));
            }
        }
        positions.Add(target.Position);
        
    }
    public override void Exit()
    {

    }
    private float time = 3f;
    public override void _PhysicsUpdate(float delta)
    {
        if(time > 0f)
        {
            time -= delta;
            return;
        }
        target.MoveTo(positions[index], 5000f);
        if(positions[index].DistanceTo(target.Position) < 30f)
        {
            if(index + 1 < positions.Count)
            {
                index += 1;
                // 开启定时器
                time = 1f;
                target.UpdateDirection();
                target.weapon.Fire(target.Target, 10f);
            }
            else
            {
                // target.velocity
                _machine.Transition<BossIdle>();
            }
        }
    }
#region HandleCollision
    
#endregion
}