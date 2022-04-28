using Godot;
using Godot.Collections;
// using System.Collections.Generic;
public struct DialogJson
{
    public string avator;
    public string background;
    public Array dialog;
}
public class DialogBox : CanvasLayer
{
    System.Collections.Generic.List<DialogJson> json = new System.Collections.Generic.List<DialogJson>();

    static public Label text;
    static public TextureRect avator;
    static Array CachedText;
    static int CachedIndex = 0;
    [Export] float interval = 0.1f;
    static Sprite indicator;
    static Tween tween;
    static bool Interactive = false;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        text = GetNode<Label>(nameof(Label));
        avator = text.GetNode<TextureRect>(nameof(TextureRect));
        indicator = text.GetNode<Sprite>(nameof(Sprite));
        tween = text.GetNode<Tween>(nameof(Tween));
        // 这里默认设置第一个模板
        text.Hide();
    }
    /// <summary>
    ///  index 也可以不用 int ，用string指定
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="index"></param>
    static public void ShowDialog(string json_file)
    {
        var file = new File();
        file.Open(json_file, File.ModeFlags.Read);
        Dictionary d = JSON.Parse(file.GetAsText()).Result as Dictionary;
        avator.Texture = ResourceLoader.Load<Texture>((string)d["avator"]);
        CachedText = (Array)d["dialog"]; 
        Interactive = true; 
        CachedIndex = 0;
        text.Show();
    }
    public void ProcessDialog()
    {
        if(CachedIndex >= CachedText.Count)
        {
            Interactive = false;
            return;
        }
        indicator.Hide();
        text.Text = (string)CachedText[CachedIndex++];
        tween.InterpolateProperty(
            text, "percent_visible", 0, 1,
            interval * text.Text.Length, Tween.TransitionType.Linear
        );
        tween.Start();
    }
    
    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
        if(!Interactive)
        {
            text.Hide(); return;
        }
        if(@event.IsActionPressed("ui_accept"))
        {
            if(tween.IsActive())
            {
                tween.StopAll();
                indicator.Show();
            }
            ProcessDialog();
        }
        GetTree().SetInputAsHandled();
    }
    private void _on_Label_visibility_changed()
    {
        GetTree().Paused = text.Visible;
    }
    private void _on_Tween_tween_all_completed()
    {
        indicator.Show();
    }

}
