using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BloodBar : MonoBehaviour
{
    float maxHealth;
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
        //GetComponent<Image>().fillAmount = (float)PlayerController.instance.health / maxHealth;
    }

}
