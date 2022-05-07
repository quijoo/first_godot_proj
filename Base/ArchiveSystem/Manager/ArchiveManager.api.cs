using System;
using Godot;

namespace Archive
{
    // 存档树不提供场景的加载和保存，仅仅提供场景树节点的加载和保存
    // 存档树的加载和保存都以子树根节点为最小单位
    // 保存节点时也将不再覆盖存档，而是修改存档
    public partial class ArchiveManager
    {
        private void _LoadGame(int id, string node_name)
        {
            if(instance == null) return;
            var save_file_path = SAVE_FOLDER.PlusFile(string.Format(SAVE_NAME_TEMPLATE, id));

            File file = new File();
            if(!file.FileExists(save_file_path))
            {
                GD.Print(string.Format("Save file {0} doesn't exsit", save_file_path));
                return;
            }
            GameSave save_game = ResourceLoader.Load<GameSave>(save_file_path);
            
            // 加载存档记录的场景
            SceneManager.LoadScene(save_game.CurrentSceneName);
            SceneManager.ChangeScene(save_game.CurrentSceneName);
            // core func
            instance.CallDeferred("Load", save_game, node_name);
            // Load(save_game, node_name);
            // GetTree().SetDeferred("paused", false);

        }
        private void _SaveGame(int id, string node_name)
        {
            if(instance == null) return;
            var save_file_path = SAVE_FOLDER.PlusFile(string.Format(SAVE_NAME_TEMPLATE, id));

            File file = new File();
            GameSave save_game;
            if(!file.FileExists(save_file_path))
            {
                save_game = CSharpScript<GameSave>.New();
                GD.Print(string.Format("create save file {0}", save_file_path));

            }
            else
            {
                save_game = ResourceLoader.Load<GameSave>(save_file_path);
            }
            save_game.Version = (string)ProjectSettings.GetSetting("application/config/version");
            save_game.CurrentSceneName = SceneManager.SceneName;
            // core func
            Save(save_game, node_name);
            
            Directory directory = new Directory();
            if(!directory.DirExists(SAVE_FOLDER))
            {
                directory.MakeDirRecursive(SAVE_FOLDER);
            }
            string save_path = SAVE_FOLDER.PlusFile(String.Format(SAVE_NAME_TEMPLATE, id));
            Error error = ResourceSaver.Save(save_path, save_game);
            if(error != Error.Ok)
            {
                GD.Print(string.Format("There was an issue writting the save {0} to {1}", id, save_path));
            }   
        }
        static public void SaveGame(int id, string name)
        {
            if(instance == null) return;
            instance._SaveGame(id, name);
        }
        static public void LoadGame(int id, string name)
        {
            if(instance == null) return;
            instance._LoadGame(id, name);
        }
    }
}