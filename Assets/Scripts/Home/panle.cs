using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class panle : MonoBehaviour
{
    public GameObject settingPanle;

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
    public void 退出游戏()
    {
        Application.Quit();
    }
}
