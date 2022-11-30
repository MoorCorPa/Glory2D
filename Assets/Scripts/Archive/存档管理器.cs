using System.Collections;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Audio;

public class 存档管理器
{

    //保存在assets目录下
    public static string 存档路径 = Application.streamingAssetsPath + "/Archive.data";
    public static string 音量路径 = Application.streamingAssetsPath + "/Volume.json";

    public static GameObject 强化树挡板;

    public static 存档 创建存档()
    {
        存档 save = new();

        save.血量 = PlayerController.instance.health;
        save.关卡名字 = SceneManager.GetActiveScene().name;
        save.存档时间 = System.DateTime.Now.ToString();
        save.水晶数量 = PlayerController.instance.水晶数量;
        
        foreach (var i in 强化树挡板.GetComponentsInChildren<武器强化>())
        {
            强化存档 s = new();
            s.序号 = i.序号;
            s.是否解锁 = i.是否解锁;
            save.强化列表.Add(s);
        }
        return save;
    }

   // [MenuItem("存档管理器/存档")]
    public static void 保存存档()
    {
        存档 save = 创建存档();
        BinaryFormatter 二进制格式器 = new BinaryFormatter();
        FileStream 文件流 = File.Create(存档路径);
        Debug.Log(save.强化列表[0].是否解锁);
        二进制格式器.Serialize(文件流, save);
        文件流.Close();
    }

    IEnumerator 载入场景(存档 save)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(save.关卡名字, LoadSceneMode.Additive);
        yield return null;

        if (operation.isDone)
        {
            PlayerController.instance.health = save.血量;
        }
    }

    //[MenuItem("存档管理器/读档")]
    public static void 读取存档()
    {
        if (File.Exists(存档路径))
        {
            BinaryFormatter 二进制格式器 = new BinaryFormatter();
            FileStream 文件流 = File.Open(存档路径, FileMode.Open);

            存档 save = 二进制格式器.Deserialize(文件流) as 存档;
            文件流.Close();

            SceneManager.LoadScene(save.关卡名字);
            //instance.StartCoroutine("载入场景", save);
        }
        else
        {
            Debug.Log("妹有存档捏");
        }
    }

    public static 音频 读取音量(AudioMixer 音量, AudioMixer 音效)
    {
        音频 data;
        if (File.Exists(音量路径))
        {
            string str = File.ReadAllText(音量路径);
            data = JsonUtility.FromJson<音频>(str);
            音量.SetFloat("BGMAudioMixer", data.volume);
            音效.SetFloat("BGMAudioMixer", data.sound);
        }
        else
        {
            data = new(1, 1);
            File.Create(音量路径).Dispose();
            File.WriteAllText(音量路径, JsonUtility.ToJson(data, true));
        }

        return data;
    }

    public static void 保存音量(float v, float s)
    {
        if (File.Exists(音量路径))
        {
            string str = File.ReadAllText(音量路径);
            音频 data = JsonUtility.FromJson<音频>(str);
            data.volume = v!= -100 ? v : data.volume;
            data.sound = s!= -100 ? s : data.sound;
            File.WriteAllText(音量路径, JsonUtility.ToJson(data, true));
        }
        else
        {
            音频 data = new(v!= -100 ? v : 1, s!= -100 ? v : 1);
            File.Create(音量路径).Dispose();
            File.WriteAllText(音量路径, JsonUtility.ToJson(data, true));
        }
    }

}
