public interface IHitable
{
    void OnHit(IAttacker attacker, string info);
}
public interface IAttacker
{
    float Damage { get; }
}