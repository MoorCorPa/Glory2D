using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletBar : MonoBehaviour
{
    private float 最大子弹条宽度;
    private int 动画精灵序号;
    private float 动画切换计时;
    
    public List<Sprite> 动画精灵列表;
    public float 动画切换间隔;
    
    void Start()
    {
        最大子弹条宽度 = GetComponent<Image>().fillAmount;
        动画切换计时 = 0;
        动画精灵序号 = 0;
    }

    void Update()
    {
        动画切换计时 += Time.deltaTime;
        if (动画切换计时 > 动画切换间隔)
        {
            动画精灵序号++;
            if (动画精灵序号 >= 动画精灵列表.Count)
            {
                动画精灵序号 = 0;
            }
            GetComponent<Image>().sprite = 动画精灵列表[动画精灵序号];
            动画切换计时 = 0;
        }
        GetComponent<Image>().fillAmount = 最大子弹条宽度 * ((float)Gun.instance.当前子弹数量/(float)Gun.instance.最大子弹数量);
    }
}
