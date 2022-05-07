using Godot;
using System;

public class Main : Node2D
{
    public Viewport viewport;
    public TextureRect texture;
    ShaderMaterial shader;
    public override void _Ready()
    {
        viewport = FindNode("_ScreenViewport") as Viewport;
        texture = FindNode("_ScreenTexture") as TextureRect;

        if(viewport == null)
        {
            GD.Print("this is no viewport in main");
        }
        texture.Texture = viewport.GetTexture();
        shader = texture.Material as ShaderMaterial;
        shader.SetShaderParam("texture_size", texture.Texture.GetSize());
        ResetScreen();
    }
    public void SetDisplayRect(Vector2 start, Vector2 end)
    {
        texture.RectPosition = start;
        texture.RectSize = end - start;
        shader.SetShaderParam("window_size", end - start);
        shader.SetShaderParam("start", start);
    }
    public void ResetScreen()
    {
        texture.RectPosition = viewport.GetVisibleRect().Position;
        texture.RectSize = viewport.GetVisibleRect().Size;

        shader.SetShaderParam("window_size", texture.RectSize);
        shader.SetShaderParam("start", texture.RectPosition);
    }
}
