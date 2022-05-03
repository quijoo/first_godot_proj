using Godot;
namespace Archive
{
    public partial class ArchiveManager
    {
        // static string SAVE_FOLDER = "res://save/";
        static public string SAVE_FOLDER = OS.GetUserDataDir() + ProjectSettings.GetSetting("application/config/custom_user_dir_name");


        // 存档名称模板
        static string SAVE_NAME_TEMPLATE = "save_{0}.tres";
    }
}