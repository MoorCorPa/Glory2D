using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasPanle : MonoBehaviour
{
    public GameObject 设置面板;

    public void 操作设置面板(bool 开关)
    {
        Time.timeScale = 开关 ? 0 : 1;
        设置面板.SetActive(开关);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            操作设置面板(!设置面板.activeInHierarchy);
        }
    }
}
