using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasPanle : MonoBehaviour
{
    public GameObject 设置面板;
    public GameObject 强化面板;
    public GameObject 死亡页;
    public GameObject 强化树挡板;

    public AudioMixer 音乐;
    public AudioMixer 音效;
    public GameObject 音乐条;
    public GameObject 音效条;

    public TextMeshProUGUI 强化说明;
    public TextMeshProUGUI 需要水晶;
    public Button 强化按钮;
    public TextMeshProUGUI 水晶显示数量;

    public GameObject 强化提示;
    private int 玩家血量 => PlayerController.instance.health;
    private int 水晶数量 => PlayerController.instance.水晶数量;
    private PlayerController 玩家 => PlayerController.instance;

    private Color32 初始颜色;

    private void Awake()
    {
        存档管理器.强化树挡板 = 强化树挡板;
    }

    private void Start()
    {
        初始颜色 = 需要水晶.color;
        存档管理器.读取音量(音乐, 音效);
        读档();
    }

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

    public void 开面板(GameObject 面板)
    {
        Time.timeScale = 0;
        面板.SetActive(true);
    }

    public void 关面板(GameObject 面板)
    {
        if (面板.name.Equals("强化树面板"))
        {
            Time.timeScale = 1;
        }
        面板.SetActive(false);
    }

    public void 开技能面板(武器强化 _武器强化)
    {
        强化说明.text = _武器强化.强化说明;

        if (_武器强化.是否解锁)
        {
            需要水晶.color = Color.grey;
        }
        else if (水晶数量 - _武器强化.需要水晶 < 0)
        {
            需要水晶.color = Color.red;
        }
        else
        {
            需要水晶.color = Color.green;
        }

        需要水晶.text = _武器强化.需要水晶.ToString();
        强化面板.SetActive(true);

        强化按钮.onClick.AddListener(delegate { 强化(_武器强化); });
    }

    public void 强化(武器强化 _武器强化)
    {
        if (_武器强化.是否解锁)
        {
            执行强化(_武器强化.序号);
            return;
        }
        if (_武器强化.是否可强化)
        {
            if (玩家.水晶消耗(_武器强化.需要水晶))
            {
                执行强化(_武器强化.序号);
                _武器强化.是否解锁 = true;
                强化提示.GetComponent<强化提示>().提示文字内容("强化成功 !");
            }
            else
            {
                强化提示.GetComponent<强化提示>().提示文字内容("水晶不足");
            }
        }
        else
        {
            强化提示.GetComponent<强化提示>().提示文字内容(_武器强化.是否解锁 ? "当前强化已解锁" : "需要解锁上一强化");
        }
    }

    public void 执行强化(int 序号)
    {
        switch (序号)
        {
            case 1:
                Gun.instance.最大子弹数量 = 24;
                Gun.instance.换弹时间 = 0.8f;
                break;
            case 2:
                Gun.instance.timeToColldown = 0.1f;
                break;
            case 3:
                Gun.instance.散射数量 = 3;
                break;
            case 4:
                PlayerController.instance.开启激光 = true;
                break;
            case 5:
                PlayerController.instance.开启攻击回血 = true;
                break;
            case 6:
                Gun.instance.最大子弹数量 = 36;
                Gun.instance.换弹时间 = 0.5f;
                break;
            case 7:
                Gun.instance.timeToColldown = 0.08f;
                break;
            case 8:
                Gun.instance.散射数量 = 5;
                break;
            case 9:
                激光控制.instance.攻击冷却 -= 0.05f;
                break;
            case 10:
                PlayerController.instance.攻击回血次数 /= 2;
                PlayerController.instance.开启攻击回血 = true;
                break;
            default:
                强化提示.GetComponent<强化提示>().提示文字内容("技能id错误");
                break;
        }
    }

    public void 关技能面板()
    {
        强化面板.SetActive(false);
    }

    private void Update()
    {
        if (玩家血量 <= 0)
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

    public void 读档()
    {
        if (File.Exists(存档管理器.存档路径))
        {
            BinaryFormatter 二进制格式器 = new BinaryFormatter();
            FileStream 文件流 = File.Open(存档管理器.存档路径, FileMode.Open);

            存档 save = 二进制格式器.Deserialize(文件流) as 存档;
            文件流.Close();

            var list = 强化树挡板.GetComponentsInChildren<武器强化>();
            for (int i = 0; i < list.Length; i++)
            {
                foreach (var j in save.强化列表)
                {
                    if (list[i].序号 == j.序号)
                    {
                        list[i].序号 = j.序号;
                        list[i].是否解锁 = j.是否解锁;
                        if (list[i].是否解锁)
                        {
                            强化(list[i]);
                        }
                        break;
                    }
                }
            }
        }

        if (File.Exists(存档管理器.音量路径))
        {
            音频 val = 存档管理器.读取音量(音乐, 音效);
            音乐条.GetComponent<Slider>().value = val.volume;
            音效条.GetComponent<Slider>().value = val.sound;
        }
    }
}