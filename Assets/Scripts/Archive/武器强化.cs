using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class 武器强化 : MonoBehaviour
{
    public int 序号;
    public bool 是否需要解锁上一强化;
    public bool 是否解锁;
    public bool 是否可强化;
    public int 需要水晶;
    public string 强化说明 = "子弹上限+10";
    public GameObject 解锁技能;
    [SerializeField]
    private SpriteRenderer 纹理;

    private void OnGUI()
    {
        Debug.Log("yes");
        if (!是否解锁)
        {
            if (解锁技能 != null)
            {
                是否可强化 = 解锁技能.GetComponent<武器强化>().是否解锁 ? true : false;
                return;
            }

            是否可强化 = true;
            return;
        }

        是否可强化 = false;
    }
}