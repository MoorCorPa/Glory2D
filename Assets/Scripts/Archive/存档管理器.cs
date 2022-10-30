using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEditor;

public class 存档管理器
{
    //保存在assets目录下
    public static string 存档路径 = Application.dataPath+"/Archive.data";

    public static 存档 创建存档()
    {
        存档 save = new 存档();
        save.血量 = PlayerController.instance.health;
        save.关卡名字 = SceneManager.GetActiveScene().name;
        save.存档时间 = System.DateTime.Now.ToString();
        return save;
    }

    [MenuItem("档案管理器/存档")]
    public static void 保存存档()
    {
        存档 save = 创建存档();
        BinaryFormatter 二进制格式器 = new BinaryFormatter();
        FileStream 文件流 = File.Create(存档路径);

        二进制格式器.Serialize(文件流, save);
        文件流.Close();
    }

    [MenuItem("档案管理器/读档")]
    public static void 读取存档()
    {
        if (File.Exists(存档路径))
        {
            BinaryFormatter 二进制格式器 = new BinaryFormatter();
            FileStream 文件流 = File.Open(存档路径, FileMode.Open);

            存档 save = 二进制格式器.Deserialize(文件流) as 存档;
            文件流.Close();

            SceneManager.LoadScene(save.关卡名字);
            PlayerController.instance.health = save.血量;
        }
        else
        {
            Debug.Log("妹有存档捏");
        }
    }
}
