using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCanvas : MonoBehaviour
{
    public TextMeshProUGUI 提示文字;
    public string 文件名;

    private Data data = new Data();
    private int 步骤;
    private string 当前步;
    private string 文件路径;

    private void Start()
    {
        文件路径 = Application.streamingAssetsPath + "/" + 文件名 + ".json";
        步骤 = 0;

        if (File.Exists(文件路径))
        {
            var json = File.ReadAllText(文件路径); //读取文件
            Data jsonData = JsonUtility.FromJson<Data>(json); //将读取到的文件创建对象
            步骤 = jsonData.guide;
        }

        if (步骤 != -1)
        {
            提示文字.gameObject.SetActive(true);
        }else
        {
            提示文字.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (步骤 != -1)
        {
            引导();
        }
    }

    private void 引导()
    {
        switch (步骤)
        {
            case 0:
                提示文字.text = "使用A键和D键控制移动";
                if (Keyboard.current.dKey.wasPressedThisFrame)
                {
                    if (当前步 == "D")
                    {
                        步骤++;
                    }

                    当前步 = "A";
                }

                if (Keyboard.current.dKey.wasPressedThisFrame)
                {
                    if (当前步 == "A")
                    {
                        步骤++;
                    }

                    当前步 = "D";
                }

                break;
            case 1:
                提示文字.text = "使用空格键跳跃";

                if (Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    步骤++;
                }

                break;
            case 2:
                提示文字.text = "鼠标左键单击可发射子弹";
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    步骤++;
                }

                break;
            case 3:
                提示文字.text = "使用R键补充子弹";
                if (Keyboard.current.rKey.wasPressedThisFrame)
                {
                    步骤 = data.guide = -1;
                    if (!Directory.Exists(Application.streamingAssetsPath))
                    {
                        Directory.CreateDirectory(Application.streamingAssetsPath);
                    }

                    var jsonInfo = JsonUtility.ToJson(data);
                    File.WriteAllText(文件路径, jsonInfo);
                    提示文字.gameObject.SetActive(false);
                }

                break;
        }
    }

    [System.Serializable]
    public class Data
    {
        public int guide;
    }
}