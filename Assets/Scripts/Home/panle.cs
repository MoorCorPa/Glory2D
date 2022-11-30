using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class panle : MonoBehaviour
{
    public GameObject settingPanle;
    public GameObject 继续游戏面板;
    public GameObject 按键设置面板;

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void OnStartHandler()
    {
        if (File.Exists(存档管理器.存档路径))
        {
            继续游戏面板.SetActive(true);
            return;
        }

        SceneManager.LoadScene("01-1");
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

    public void 关闭窗口(GameObject panel)
    {
        panel.SetActive(false);
    }
    public void 打开窗口(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void 继续游戏()
    {
        if (File.Exists(存档管理器.存档路径))
        {
            BinaryFormatter 二进制格式器 = new BinaryFormatter();
            FileStream 文件流 = File.Open(存档管理器.存档路径, FileMode.Open);

            存档 save = 二进制格式器.Deserialize(文件流) as 存档;
            文件流.Close();

            SceneManager.LoadScene(save.关卡名字);
        }
    }

    public void 重新开始()
    {
        SceneManager.LoadScene("01-1");
        if (File.Exists(存档管理器.存档路径))
        {
            File.Delete(存档管理器.存档路径);

        }
    }

    public void 退出游戏()
    {
        Application.Quit();
    }
}
