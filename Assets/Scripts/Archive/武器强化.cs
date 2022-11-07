using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class 武器强化 : MonoBehaviour
{
    public int 序号;
    public bool 是否需要解锁上一强化;
    public bool 是否解锁;
    public bool 是否可强化;
    public int 需要水晶;
    public string 强化说明 = "子弹上限+10";
    public GameObject 解锁技能;
    private SpriteRenderer 纹理;

    private void Start()
    {
        //纹理 = GetComponent<SpriteRenderer>();
        //根据存档id查找解锁状态

        if (!是否解锁)
        {
            if (解锁技能 != null)
            {
                if (!解锁技能.GetComponent<武器强化>().是否解锁)
                {
                    是否可强化 = false;
                    return;
                }
            }

            是否可强化 = true;
            return;
        }

        //纹理.color = Color.white / 2;
        是否可强化 = false;
    }
}