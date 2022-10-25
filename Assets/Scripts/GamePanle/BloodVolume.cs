using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BloodVolume : MonoBehaviour
{
    float 最大血量;
    float currentHealth;
    Image Image;
    public float 像素长度;
    public float 像素宽度;
    public GameObject 血量头;
    public GameObject 血量尾;
    void Start()
    {
        最大血量 = (float)PlayerController.instance.health;
        //GetComponent<Image>().preferredWidth(maxHealth * 5);
        Image = this.transform.GetComponent<Image>();
        this.transform.GetComponent<RectTransform>().sizeDelta = new Vector2((最大血量-2) * 像素长度, 像素宽度);
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
            this.transform.GetComponent<RectTransform>().sizeDelta = new Vector2((currentHealth-1) * 像素长度, 像素宽度);
        }
    }

}
