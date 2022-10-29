using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleMap : MonoBehaviour
{
    public string sceneName;
    public TextMeshProUGUI 提示文字;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            {
                提示文字.gameObject.SetActive(true);
                Invoke("关闭提示", 3);
            }
            else
            {
                提示文字.gameObject.SetActive(false);
                下一关();
            }
        }
    }
    
    public void 下一关()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void 关闭提示()
    {
        提示文字.gameObject.SetActive(false);
    }
}
