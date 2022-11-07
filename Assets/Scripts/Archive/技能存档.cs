using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class 技能存档 : MonoBehaviour
{
    private ArrayList 武器强化列表 = new ArrayList();
    private 武器强化 _武器强化;
    private void Start()
    {
        //是否存在存档
        //读取
        
    }
//存档时机
    public void 存档()
    {
        foreach (var 强化 in  GetComponentsInChildren<武器强化>())
        {
            _武器强化 = new 武器强化();
            _武器强化.序号 = 强化.序号;
            _武器强化.是否解锁 = 强化.是否解锁;
            武器强化列表.Add(_武器强化);
        }
    }
}