using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class 强化提示 : MonoBehaviour
{
    public TextMeshProUGUI 提示文字;
    public float 消失速度;
    private Color32 初始颜色;
    private Color32 文字初始颜色;
    private Image 图片;
    private float 透明度;
    private float 文字透明度;

    private void Start()
    {
        图片 = GetComponent<Image>();
        初始颜色 = 图片.color;
        文字初始颜色 = 提示文字.color;
        透明度 = 初始颜色.a;
        文字透明度 = 初始颜色.a;
    }

    private void Update()
    {
        提示消失();
    }

    public void 提示文字内容(string 文字)
    {
        提示文字.text = 文字;
        透明度 = 初始颜色.a;
        文字透明度 = 初始颜色.a;
        gameObject.SetActive(true);
    }

    public void 提示消失()
    {
        透明度 -= 消失速度 * 0.003f;
        文字透明度 -= 消失速度 * 0.003f;
        if (透明度 > 0)
        {
            图片.color = new Color32(初始颜色.r, 初始颜色.g, 初始颜色.b, (byte) 透明度);
            提示文字.color = new Color32(文字初始颜色.r, 文字初始颜色.g, 文字初始颜色.b, (byte) 文字透明度);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}