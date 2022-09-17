using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class settingPanle : MonoBehaviour
{
    public AudioMixer audioMixer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCloseHandler()
    {
        gameObject.SetActive(false);
    }
    public void SetVolumeHandler(float i)
    {
        if (i > -30)
        {
            audioMixer.SetFloat("BGMAudioMixer", i);
        }
        else
        {
            audioMixer.SetFloat("BGMAudioMixer", -80);
        }
    }
}
