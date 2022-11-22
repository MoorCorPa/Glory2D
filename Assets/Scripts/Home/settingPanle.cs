using UnityEngine;
using UnityEngine.Audio;

public class settingPanle : MonoBehaviour
{
    public AudioMixer 音乐;
    public AudioMixer 音效;

    public void OnCloseHandler()
    {
        gameObject.SetActive(false);
    }

    public void 音乐控制(float i)
    {
        音乐.SetFloat("BGMAudioMixer", i > -30 ? i : -80);
    }

    public void 音效控制(float i)
    {
        音效.SetFloat("BGMAudioMixer", i > -30 ? i : -80);
    }
}