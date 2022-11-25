using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class panle : MonoBehaviour
{
    public GameObject settingPanle;
    public GameObject 按键设置面板;

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void OnStartHandler()
    {
        SceneManager.LoadScene(1);
    }
    public void OnOpenSettingPanleHandler()
    {
        settingPanle.SetActive(true);
    }

    public void 打开按键设置()
    {
        按键设置面板.SetActive(true);
    }

    public void 关闭按键设置()
    {
        按键设置面板.SetActive(false);
    }

    public void 退出游戏()
    {
        Application.Quit();
    }
}
