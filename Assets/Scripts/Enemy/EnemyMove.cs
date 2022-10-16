using Pathfinding;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform 目标;
    public Transform 本体;

    public float 速度 = 200f;
    public float 下一个路径点距离 = 3f;

    Path 路径;
    int 当前路径点 = 0;
    bool 是否到达路径终点 = false;

    Seeker 观察者;
    Rigidbody2D 刚体;

    void Start()
    {
        观察者 = GetComponent<Seeker>();
        刚体 = GetComponent<Rigidbody2D>();

        InvokeRepeating("更新路径", 0f, 0.5f);
    }

    void 更新路径()
    {
        if (观察者.IsDone())
            观察者.StartPath(刚体.position, 目标.position, 当路径完成);
    }

    void 当路径完成(Path 路)
    {
        if (!路.error)
        {
            路径 = 路;
            //吧当前路径点设置为0，从新路径的起点开始移动
            当前路径点 = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (路径 == null)
            return;

        if (当前路径点 >= 路径.vectorPath.Count)
        {
            是否到达路径终点 = true;
            return;
        }
        else
        {
            是否到达路径终点 = false;
        }

        Vector2 朝向 = ((Vector2)路径.vectorPath[当前路径点] - 刚体.position).normalized;
        Vector2 力 = 朝向 * 速度 * Time.deltaTime;

        刚体.AddForce(力);

        float 距离 = Vector2.Distance(刚体.position, 路径.vectorPath[当前路径点]);

        if (距离 < 下一个路径点距离)
            当前路径点++;

        if (力.x >= 0.01f)
        {
            本体.localScale = new Vector3(-1, 1, 1);
        }else if (力.x <= -0.01f)
        {
            本体.localScale = new Vector3(1, 1, 1);
        }
    }
}
