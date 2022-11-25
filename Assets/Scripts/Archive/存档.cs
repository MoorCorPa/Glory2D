using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class 存档
{
    public int 血量;
    public string 关卡名字;
    public string 存档时间;
    public int 水晶数量;
    [SerializeReference]
    public List<强化存档> 强化列表 = new();
}

[Serializable]
public class 音频
{
    public float volume;
    public float sound;

    public 音频(float volume, float sound){
        this.volume = volume;
        this.sound = sound;
    }
}