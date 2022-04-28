using Godot;
public interface IElastic
{
    Vector2 ReboundSpeed {get;}
    Vector2 Rebound(Vector2 collision, Vector2 collided);
}