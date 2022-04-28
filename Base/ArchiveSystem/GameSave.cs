using Godot;
using Godot.Collections;
[Tool, CSharpScript]
public class GameSave : Resource
{
    [Export] public string Version;
    [Export] public string CurrentSceneName;
    [Export] public Dictionary<string, Array> SceneMap = new Dictionary<string, Array>();
}
