using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class 抛物线子弹 : MonoBehaviour
{
    [Min(0)] public float 子弹飞行速度;
    [Min(1)] public int 子弹伤害;
    [Min(0)] public float 子弹销毁时间;

    public Rigidbody2D 子弹刚体;
    public bool 是否触发;

    public Vector3 玩家位置 => PlayerController.instance.transform.position;
    public Vector3 速度;

    public Vector3 当前位置
    {
        get => transform.position;
        set => transform.position = value;
    }

/*    public 抛物线子弹(Vector3 向量)
    {
        速度 = 向量;
    }*/

    public void Start()
    {
        Destroy(gameObject, 子弹销毁时间);
        子弹刚体 = GetComponent<Rigidbody2D>();
        transform.right = (玩家位置 - 当前位置).normalized;
        //子弹刚体.velocity = transform.right * 子弹飞行速度;
//子弹刚体.velocity = 速度;
        //Physics2D.IgnoreLayerCollision(8, 9);
        是否触发 = false;
        
    }

    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // if (!collision.CompareTag("子弹") && !collision.CompareTag("Enemy"))
        // {
        if (!是否触发)
        {
            是否触发 = true;
            if (collision.GetComponent<PlayerController>())
            {
                collision.GetComponent<PlayerController>().TakeDamage(子弹伤害);
            }
            
            子弹刚体.velocity = new Vector2();
            子弹刚体.gravityScale = 0;
            transform.position = collision.ClosestPoint(transform.position);
            GetComponent<Animator>().Play("射击反馈");
        }
        // }
    }

    public void 子弹销毁()
    {
        Destroy(gameObject);
    }
}