using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BloodVolume : MonoBehaviour
{
    float maxHealth;
    float currentHealth;
    Image Image;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = (float)PlayerController.instance.health;
        //GetComponent<Image>().preferredWidth(maxHealth * 5);
        Image = this.transform.GetComponent<Image>();
        this.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(maxHealth * 5, Image.sprite.texture.height);
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = (float)PlayerController.instance.health;
        this.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(currentHealth * 5, Image.sprite.texture.height);
    }

}
