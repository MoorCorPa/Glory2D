using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class 导出场景到XML : Editor
{
    [MenuItem("GameObject/ExportXML")]
    static void 导出XML()
    {
        string 文件路径 = Application.dataPath + @"/StreamingAssets/测试.xml";
        if (!File.Exists(文件路径))
        {
            File.Delete(文件路径);
        }
        XmlDocument xml文档 = new XmlDocument();
        XmlElement 源 = xml文档.CreateElement("gameObjects");

        var 当前场景 = SceneManager.GetActiveScene();
        XmlElement 场景s = xml文档.CreateElement("scenes");

        场景s.SetAttribute("name", 当前场景.name);
        foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.transform.parent == null)
            {
                XmlElement gameObject = xml文档.CreateElement("gameObjects");
                gameObject.SetAttribute("name", obj.name);

                gameObject.SetAttribute("asset", obj.name + ".prefab");
                XmlElement transform = xml文档.CreateElement("transform");
                XmlElement position = xml文档.CreateElement("position");
                XmlElement position_x = xml文档.CreateElement("x");
                position_x.InnerText = obj.transform.position.x + "";
                XmlElement position_y = xml文档.CreateElement("y");
                position_y.InnerText = obj.transform.position.y + "";
                XmlElement position_z = xml文档.CreateElement("z");
                position_z.InnerText = obj.transform.position.z + "";
                position.AppendChild(position_x);
                position.AppendChild(position_y);
                position.AppendChild(position_z);

                XmlElement rotation = xml文档.CreateElement("rotation");
                XmlElement rotation_x = xml文档.CreateElement("x");
                rotation_x.InnerText = obj.transform.rotation.eulerAngles.x + "";
                XmlElement rotation_y = xml文档.CreateElement("y");
                rotation_y.InnerText = obj.transform.rotation.eulerAngles.y + "";
                XmlElement rotation_z = xml文档.CreateElement("z");
                rotation_z.InnerText = obj.transform.rotation.eulerAngles.z + "";
                rotation.AppendChild(rotation_x);
                rotation.AppendChild(rotation_y);
                rotation.AppendChild(rotation_z);

                XmlElement scale = xml文档.CreateElement("scale");
                XmlElement scale_x = xml文档.CreateElement("x");
                scale_x.InnerText = obj.transform.localScale.x + "";
                XmlElement scale_y = xml文档.CreateElement("y");
                scale_y.InnerText = obj.transform.localScale.y + "";
                XmlElement scale_z = xml文档.CreateElement("z");
                scale_z.InnerText = obj.transform.localScale.z + "";

                scale.AppendChild(scale_x);
                scale.AppendChild(scale_y);
                scale.AppendChild(scale_z);

                transform.AppendChild(position);
                transform.AppendChild(rotation);
                transform.AppendChild(scale);

                gameObject.AppendChild(transform);
                xml文档.AppendChild(gameObject);
                源.AppendChild(场景s);
                xml文档.AppendChild(源);
                xml文档.Save(文件路径);

            }
        }
        AssetDatabase.Refresh();
    }

/*    [MenuItem("GameObject/ExportJSON")]
    static void 导出JSON()
    {
        string filepath = Application.dataPath + @"/StreamingAssets/json.txt";
        FileInfo t = new FileInfo(filepath);
        if (!File.Exists(filepath))
        {
            File.Delete(filepath);
        }
        StreamWriter sw = t.CreateText();

        StringBuilder sb = new StringBuilder();
        JsonWriter writer = new JsonWriter(sb);
        writer.WriteObjectStart();
        writer.WritePropertyName("GameObjects");
        writer.WriteArrayStart();

        foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes)
        {
            if (S.enabled)
            {
                string name = S.path;
                EditorApplication.OpenScene(name);
                writer.WriteObjectStart();
                writer.WritePropertyName("scenes");
                writer.WriteArrayStart();
                writer.WriteObjectStart();
                writer.WritePropertyName("name");
                writer.Write(name);
                writer.WritePropertyName("gameObject");
                writer.WriteArrayStart();

                foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
                {
                    if (obj.transform.parent == null)
                    {
                        writer.WriteObjectStart();
                        writer.WritePropertyName("name");
                        writer.Write(obj.name);

                        writer.WritePropertyName("position");
                        writer.WriteArrayStart();
                        writer.WriteObjectStart();
                        writer.WritePropertyName("x");
                        writer.Write(obj.transform.position.x.ToString("F5"));
                        writer.WritePropertyName("y");
                        writer.Write(obj.transform.position.y.ToString("F5"));
                        writer.WritePropertyName("z");
                        writer.Write(obj.transform.position.z.ToString("F5"));
                        writer.WriteObjectEnd();
                        writer.WriteArrayEnd();

                        writer.WritePropertyName("rotation");
                        writer.WriteArrayStart();
                        writer.WriteObjectStart();
                        writer.WritePropertyName("x");
                        writer.Write(obj.transform.rotation.eulerAngles.x.ToString("F5"));
                        writer.WritePropertyName("y");
                        writer.Write(obj.transform.rotation.eulerAngles.y.ToString("F5"));
                        writer.WritePropertyName("z");
                        writer.Write(obj.transform.rotation.eulerAngles.z.ToString("F5"));
                        writer.WriteObjectEnd();
                        writer.WriteArrayEnd();

                        writer.WritePropertyName("scale");
                        writer.WriteArrayStart();
                        writer.WriteObjectStart();
                        writer.WritePropertyName("x");
                        writer.Write(obj.transform.localScale.x.ToString("F5"));
                        writer.WritePropertyName("y");
                        writer.Write(obj.transform.localScale.y.ToString("F5"));
                        writer.WritePropertyName("z");
                        writer.Write(obj.transform.localScale.z.ToString("F5"));
                        writer.WriteObjectEnd();
                        writer.WriteArrayEnd();

                        writer.WriteObjectEnd();
                    }
                }

                writer.WriteArrayEnd();
                writer.WriteObjectEnd();
                writer.WriteArrayEnd();
                writer.WriteObjectEnd();
            }
        }
        writer.WriteArrayEnd();
        writer.WriteObjectEnd();

        sw.WriteLine(sb.ToString());
        sw.Close();
        sw.Dispose();
        AssetDatabase.Refresh();
    }*/

}