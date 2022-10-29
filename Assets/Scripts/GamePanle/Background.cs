using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Vector2 相对视差;
    private Transform 人物;
    private Vector3 人物位置;

    private void Start()
    {
        人物 = GameObject.FindWithTag("Player").transform;
        人物位置 = 人物.position;
    }

    private void LateUpdate()
    {
        Vector3 移动差 =  人物.position - 人物位置;
        transform.position += new Vector3(移动差.x * 相对视差.x, 移动差.y * 相对视差.y, 0);
        人物位置 = 人物.position;
    }
}