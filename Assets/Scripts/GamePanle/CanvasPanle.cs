using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasPanle : MonoBehaviour
{
    public GameObject 设置面板;
    public GameObject 死亡页;
    
    public TextMeshProUGUI 水晶显示数量;
    
    private int 玩家血量 => PlayerController.instance.health;
    private int 水晶数量 => PlayerController.instance.水晶数量;

    public void 操作设置面板(bool 开关)
    {
        Time.timeScale = 开关 ? 0 : 1;
        设置面板.SetActive(开关);
    }
    
    public void 回到主菜单()
    {
        SceneManager.LoadScene(0);
    }
    
    public void 重开()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void 刷新水晶数量()
    {
        水晶显示数量.text = 水晶数量.ToString();
    }
    
    private void Update()
    {
        if (玩家血量<=0)
        {
            Time.timeScale = 0;
            死亡页.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !死亡页.activeSelf)
        {
            操作设置面板(!设置面板.activeInHierarchy);
        }

        刷新水晶数量();
    }
}
