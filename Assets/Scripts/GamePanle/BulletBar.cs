using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletBar : MonoBehaviour
{
    
    public float 最大子弹条宽度;
    void Start()
    {
        最大子弹条宽度 = GetComponent<Image>().fillAmount;
    }

    void Update()
    {
        GetComponent<Image>().fillAmount = 最大子弹条宽度 * ((float)Gun.instance.当前子弹数量/(float)Gun.instance.最大子弹数量);
    }
}
