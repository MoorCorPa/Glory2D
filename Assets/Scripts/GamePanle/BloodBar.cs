using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BloodBar : MonoBehaviour
{
    float 最大血量;
    float currentHealth;
    public float 血量中1像素长度;
    public float 血量中1像素宽度;
    public float 血量中2像素长度;
    public float 血量中2像素宽度;
    public GameObject 血量头;
    public GameObject 血量中1;
    public GameObject 血量中2;
    public GameObject 血量尾;
    void Start()
    {
        //GetComponent<Image>().preferredWidth(maxHealth * 5);
        最大血量 = (float)PlayerController.instance.health;
        血量中1.GetComponent<RectTransform>().sizeDelta = new Vector2((最大血量-2) * 血量中1像素长度, 血量中1像素宽度);
        血量中2.GetComponent<RectTransform>().sizeDelta = new Vector2((最大血量-2) * 血量中2像素长度, 血量中2像素宽度);
        GetComponent<RectTransform>().sizeDelta = new Vector2(
            GetComponent<RectTransform>().rect.width + (最大血量 - 5) * 8,
            GetComponent<RectTransform>().rect.height);
    
    }

    void Update()
    {
        
        currentHealth = (float)PlayerController.instance.health;
        if (currentHealth < 最大血量)
        {
            血量尾.SetActive(false);
        }
        if (currentHealth <= 0)
        {
            血量头.SetActive(false);
        }

        if (currentHealth < 最大血量 - 1)
        {
            血量中1.GetComponent<RectTransform>().sizeDelta = new Vector2((currentHealth-1) * 血量中1像素长度, 血量中1像素宽度);
            血量中2.GetComponent<RectTransform>().sizeDelta = new Vector2((currentHealth-1) * 血量中2像素长度, 血量中2像素宽度);
        }
    }

}