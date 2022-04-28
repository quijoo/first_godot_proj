using Godot;
using System;
using System.Collections.Generic;

// TODO： 
// 1. 音效管理器应该是一个全局单例
// 2. 应该在游戏加载时候自动载入（或者异步载入）
// 3. 应该设置统一访问名
public class SoundManager : Node2D
{
    static Node2D music_list;
    static Node2D sound_list;
    static Dictionary<string,AudioStreamPlayer> MusicDictionary = new Dictionary<string,AudioStreamPlayer>();
    static Dictionary<string,AudioStreamPlayer> SoundDictionary = new Dictionary<string,AudioStreamPlayer>();

    public override void _Ready()
    {
        music_list = GetNode<Node2D>("Music");
        sound_list = GetNode<Node2D>("SoundEffect");
        for(int i = 0; i < music_list.GetChildCount(); i++)
        {
            var child = music_list.GetChild<AudioStreamPlayer>(i);
            MusicDictionary.Add(child.Name, child);
        }
        for(int i = 0; i < sound_list.GetChildCount(); i++)
        {
            var child = sound_list.GetChild<AudioStreamPlayer>(i);
            SoundDictionary.Add(child.Name, child);
        }
    }

    public void UpdateSoundVolume(int value, int volrange)
    {

    }
    public void UpdateMusicVolume(int value, int volrange)
    {
        
    }
    public void StopAllMusic()
    {

    }
    static public void PlayMusic(string name)
    {
        if(MusicDictionary.ContainsKey(name))
            MusicDictionary[name].Play();
    }
    static public void PlaySound(string name)
    {
        if(SoundDictionary.ContainsKey(name))
            SoundDictionary[name].Play();
    }
}
