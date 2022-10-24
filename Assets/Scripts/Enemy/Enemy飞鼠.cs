using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy飞鼠 : Enemy
{

    public float 随机移动Y;
    public float 冲力 = 5;
    Vector3 方向;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        animator.SetInteger("怪物ID", 1);
    }

    // Update is called once per frame
    void Update() 
    {
        base.Update();
        检测障碍物();
        方向 = new Vector3(
            PlayerController.instance.transform.position.x - transform.position.x,
            PlayerController.instance.transform.position.y - transform.position.y, 0);
    }

    void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void MoveController()
    {
        if (perceptionRadius > (transform.position - PlayerController.instance.transform.position).sqrMagnitude && isAttacked && !检测障碍物())
        {
            // 进入锁定范围
            开始攻击();
        }
        else
        {
            随机移动();
        }
    }

    public virtual void 开始攻击()
    {
        Debug.Log("进入锁定范围");
        transform.localScale = new Vector3(getFaceAt() ? 1 : -1, 1, 1);
        刚体.AddForce(方向*冲力, ForceMode2D.Impulse);
        isAttacked = false;
    }

    public override void 随机移动()
    {
        moveTimeCount += Time.deltaTime;
        if (moveTimeCount > moveColldownTime)
        {
            moveX = Random.Range(-3, 3f) + transform.position.x;
            随机移动Y = Random.Range(-3, 3f) + transform.position.y;
            moveTimeCount = 0;
        }

        transform.localScale = new Vector3(moveX < transform.position.x ? 1 : -1, 1, 1);
        transform.position = Vector2.MoveTowards(transform.position,
            new Vector2(moveX, 随机移动Y), moveSpeed * 0.02f);
    }

    public virtual bool 检测障碍物()
    {
        var 长度 = Vector2.Distance(transform.position, PlayerController.instance.transform.position);
        Debug.DrawLine(transform.position, transform.position + 方向.normalized * 长度, Color.yellow);

        if (Physics2D.Raycast(transform.position, 方向, 长度, LayerMask.GetMask("Ground")))
        {
            return true;
        }
        return false;
    }
}
