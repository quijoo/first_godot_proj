using Godot;
using System;

/// 场景组织根节点下
/// 1. 静态节点
/// 2. 游戏场景
/// 所有的 UI 将挂载到 UIManager 节点下， 并且在场景切换时不会改变
/// UIManager 将持有所有的 UI 引用

/// UI 的组织如下
/// 1. UIManager
/// 2. 各种 Viewport （绑定脚本，并且持有各个子模块的引用）
/// 3. 各种子模块，例如在 HUD 下 有HealthHUD 子模块， 子模块提供各种功能

/// 添加新 UI 需要：
/// 1. 需要在对应的 Viewport 中添加子模块引用
/// 2. 需要在 UIManager 中添加 Viewport 的 PackedScene
/// 3. 需要在 UIManager 中添加 AddChild

/// *. 重要的事件是 UIManager 不应该通过管理器访问 UI 而是应该通过监听事件来处理
public class UIManager : Node
{
    // 1. HUD
    [Export] public PackedScene HUDPackedScene;
    static public HUD hud;
    // 2. UI 背包， 设置， ...
    [Export] public PackedScene UIPackedScene;
    public override void _Ready()
    {
        base._Ready();
        // 等待根 ViewPort 加载完毕
        // 1. Init HUD
        hud = HUDPackedScene.Instance() as HUD;
        AddChild(hud);
    }
}
