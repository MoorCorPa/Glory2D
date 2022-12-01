using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleMap : MonoBehaviour
{
    public string sceneName;
    public TextMeshProUGUI 提示文字;
    public GameObject 鳄龟;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (鳄龟 != null && 鳄龟.GetComponent<Enemy鳄龟>().当前血量 <= 0)
            {
                if (sceneName.Equals("终章"))
                {
                    提示文字.gameObject.GetComponent<TMP_Text>().text = "游戏结束，感谢游玩；游戏将在3秒后返回";
                    提示文字.gameObject.SetActive(true);
                    Invoke("关闭提示", 3);
                    Invoke("返回主页", 3);
                    return;
                }
            }


            if (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            {
                foreach (var i in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    if (i.GetComponent<Enemy>().当前血量 > 0)
                    {
                        if (sceneName.Equals("终章")|| sceneName.Equals("02-1") || sceneName.Equals("03-1") || sceneName.Equals("04-1"))
                            return;

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

    public void 返回主页()
    {
        SceneManager.LoadScene("GameHome");
    }
}