using Godot;
using Godot.Collections;
public interface Archivable
{
    // 需要继承的
    string Name { get; set;}
    string Filename {get;}
    Node GetParent();
    NodePath GetPath();
    void Save(in Dictionary data);
    void Load(in Dictionary data);
}

public interface ArchiveExecutor
{
    void SaveExecute(in Dictionary data, Node node);
    void LoadExecute(in Dictionary data);
}