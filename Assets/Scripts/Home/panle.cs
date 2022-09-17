using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class panle : MonoBehaviour
{
    public GameObject settingPanle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnStartHandler()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void OnOpenSettingPanleHandler()
    {
        settingPanle.SetActive(true);
    }
}
