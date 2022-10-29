using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class 读取场景 : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //电脑和iphong上的路径是不一样的，这里用标签判断一下。
        #if UNITY_EDITOR
            string filepath = Application.dataPath + "/StreamingAssets" + "/my.xml";
        #elif UNITY_IPHONE
            string filepath = Application.dataPath +"/Raw"+"/my.xml";
        #endif

        if (File.Exists(filepath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filepath);
            XmlNodeList nodeList = xmlDoc.SelectSingleNode("gameObjects").ChildNodes;
            foreach (XmlElement scene in nodeList)
            {
                //因为我的XML是把所有游戏对象全部导出， 所以这里判断一下只解析需要的场景中的游戏对象
                //JSON和它的原理类似
                if (!scene.GetAttribute("name").Equals("Assets/StarTrooper.unity"))
                {
                    continue;
                }

                foreach (XmlElement gameObjects in scene.ChildNodes)
                {

                    string asset = "Prefab/" + gameObjects.GetAttribute("name");
                    Vector3 pos = Vector3.zero;
                    Vector3 rot = Vector3.zero;
                    Vector3 sca = Vector3.zero;
                    foreach (XmlElement transform in gameObjects.ChildNodes)
                    {
                        foreach (XmlElement prs in transform.ChildNodes)
                        {
                            if (prs.Name == "position")
                            {
                                foreach (XmlElement position in prs.ChildNodes)
                                {
                                    switch (position.Name)
                                    {
                                        case "x":
                                            pos.x = float.Parse(position.InnerText);
                                            break;
                                        case "y":
                                            pos.y = float.Parse(position.InnerText);
                                            break;
                                        case "z":
                                            pos.z = float.Parse(position.InnerText);
                                            break;
                                    }
                                }
                            }
                            else if (prs.Name == "rotation")
                            {
                                foreach (XmlElement rotation in prs.ChildNodes)
                                {
                                    switch (rotation.Name)
                                    {
                                        case "x":
                                            rot.x = float.Parse(rotation.InnerText);
                                            break;
                                        case "y":
                                            rot.y = float.Parse(rotation.InnerText);
                                            break;
                                        case "z":
                                            rot.z = float.Parse(rotation.InnerText);
                                            break;
                                    }
                                }
                            }
                            else if (prs.Name == "scale")
                            {
                                foreach (XmlElement scale in prs.ChildNodes)
                                {
                                    switch (scale.Name)
                                    {
                                        case "x":
                                            sca.x = float.Parse(scale.InnerText);
                                            break;
                                        case "y":
                                            sca.y = float.Parse(scale.InnerText);
                                            break;
                                        case "z":
                                            sca.z = float.Parse(scale.InnerText);
                                            break;
                                    }
                                }
                            }
                        }

                        //拿到 旋转 缩放 平移 以后克隆新游戏对象
                        GameObject ob = (GameObject)Instantiate(Resources.Load(asset), pos, Quaternion.Euler(rot));
                        ob.transform.localScale = sca;

                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
