using System;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BloodBar : MonoBehaviour
{
    private float 最大血量;
    private float 当前血量 => PlayerController.instance.health;
    public float 血量中1像素长度;
    public float 血量中1像素宽度;
    public float 血量中2像素长度;
    public float 血量中2像素宽度;
    public GameObject 血量头;
    public GameObject 血量中1;
    public GameObject 血量中2;
    public GameObject 血量尾;

    private void Start()
    {
        //GetComponent<Image>().preferredWidth(maxHealth * 5);
        最大血量 = (float) PlayerController.instance.最大血量;
        血量中1.GetComponent<RectTransform>().sizeDelta = new Vector2((最大血量 - 2) * 血量中1像素长度, 血量中1像素宽度);
        血量中2.GetComponent<RectTransform>().sizeDelta = new Vector2((最大血量 - 2) * 血量中2像素长度, 血量中2像素宽度);
        GetComponent<RectTransform>().sizeDelta = new Vector2(
            GetComponent<RectTransform>().rect.width + (最大血量 - 5) * 8,
            GetComponent<RectTransform>().rect.height);
    }

    private void Update()
    {
        if (当前血量 < 最大血量)
        {
            血量尾.SetActive(false);
        }

        if (当前血量 <= 0)
        {
            血量头.SetActive(false);
        }

        if (Mathf.Abs(当前血量 - 最大血量) < Mathf.Epsilon)
        {
            血量尾.SetActive(true);
        }
        else
        {
            血量中1.GetComponent<RectTransform>().sizeDelta = new Vector2((当前血量 - 1) * 血量中1像素长度, 血量中1像素宽度);
            血量中2.GetComponent<RectTransform>().sizeDelta = new Vector2((当前血量 - 1) * 血量中2像素长度, 血量中2像素宽度);
            
        }

    }
}