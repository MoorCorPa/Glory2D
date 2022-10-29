using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        if (i > -30)
        {
            音乐.SetFloat("BGMAudioMixer", i);
        }
        else
        {
            音效.SetFloat("BGMAudioMixer", -80);
        }
    }
    public void 音效控制(float i)
    {
        if (i > -30)
        {
            音效.SetFloat("BGMAudioMixer", i);
        }
        else
        {
            音效.SetFloat("BGMAudioMixer", -80);
        }
    }
}
