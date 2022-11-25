using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class settingPanle : MonoBehaviour
{
    public AudioMixer 音乐;
    public AudioMixer 音效;
    public GameObject 音乐条;
    public GameObject 音效条;

    public void OnCloseHandler()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        音频 val = 存档管理器.读取音量(音乐, 音效);
        音乐条.GetComponent<Slider>().value = val.volume;
        音效条.GetComponent<Slider>().value = val.sound;
    }

    public void 音乐控制(float i)
    {
        音乐.SetFloat("BGMAudioMixer", i > -30 ? i : -80);
        存档管理器.保存音量(i, -100);
    }

    public void 音效控制(float i)
    {
        音效.SetFloat("BGMAudioMixer", i > -30 ? i : -80);
        存档管理器.保存音量(-100, i);
    }
}