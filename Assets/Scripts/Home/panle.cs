using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class panle : MonoBehaviour
{
    public GameObject settingPanle;
    void Start()
    {
        
    }

    void Update()
    {

    }
    public void OnStartHandler()
    {
        SceneManager.LoadScene("00-InitialLevel");
    }
    public void OnOpenSettingPanleHandler()
    {
        settingPanle.SetActive(true);
    }
}
