using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEditor;

public class �浵������
{
    //������assetsĿ¼��
    public static string �浵·�� = Application.dataPath+"/Archive.data";

    public static �浵 �����浵()
    {
        �浵 save = new �浵();
        save.Ѫ�� = PlayerController.instance.health;
        save.�ؿ����� = SceneManager.GetActiveScene().name;
        save.�浵ʱ�� = System.DateTime.Now.ToString();
        return save;
    }

    [MenuItem("����������/�浵")]
    public static void ����浵()
    {
        �浵 save = �����浵();
        BinaryFormatter �����Ƹ�ʽ�� = new BinaryFormatter();
        FileStream �ļ��� = File.Create(�浵·��);

        �����Ƹ�ʽ��.Serialize(�ļ���, save);
        �ļ���.Close();
    }

    [MenuItem("����������/����")]
    public static void ��ȡ�浵()
    {
        if (File.Exists(�浵·��))
        {
            BinaryFormatter �����Ƹ�ʽ�� = new BinaryFormatter();
            FileStream �ļ��� = File.Open(�浵·��, FileMode.Open);

            �浵 save = �����Ƹ�ʽ��.Deserialize(�ļ���) as �浵;
            �ļ���.Close();

            SceneManager.LoadScene(save.�ؿ�����);
            PlayerController.instance.health = save.Ѫ��;
        }
        else
        {
            Debug.Log("���д浵��");
        }
    }
}
