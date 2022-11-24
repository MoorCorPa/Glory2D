using TMPro;
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
                foreach (var i in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    if (i.GetComponent<Enemy>().当前血量 > 0)
                    {
                        提示文字.gameObject.SetActive(true);
                        Invoke("关闭提示", 3);
                        return;
                    }
                }
                提示文字.gameObject.SetActive(false);
                存档管理器.保存存档();
            }
            else
            {
                提示文字.gameObject.SetActive(false);
                存档管理器.保存存档();
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