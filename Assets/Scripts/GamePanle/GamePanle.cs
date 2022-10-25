using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePanle : MonoBehaviour
{
    public TextMeshProUGUI 血量;
    public TextMeshProUGUI 子弹;
    // Start is called before the first frame update
    void Start()
    {
        //Text = transform.GetComponent<TextMeshProUGUI>();
        血量.text = PlayerController.instance.health.ToString();
        子弹.text = "x" + Gun.instance.当前子弹数量 + "/" + Gun.instance.最大子弹数量;
    }

    // Update is called once per frame
    void Update()
    {
        血量.text = PlayerController.instance.health.ToString();
        子弹.text = "x" + Gun.instance.当前子弹数量 + "/" + Gun.instance.最大子弹数量;
    }
}
