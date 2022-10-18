using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    [Min(0)] public float 子弹飞行速度 = 15f;
    [Min(1)] public int 子弹伤害;
    [Min(0)] public float 子弹销毁时间;

    private float 子弹生成时的游戏时间;

    public GameObject hitEffect;

    private Rigidbody2D 子弹刚体;
    private bool 是否触发 = false;

    private void Awake()
    {
        子弹刚体 = GetComponent<Rigidbody2D>();
        var scale = transform.localScale;
        transform.localScale = new Vector3(PlayerController.instance.flag * scale.x, scale.y, scale.z);

        子弹生成时的游戏时间 = Time.time;
        子弹刚体.velocity = PlayerController.instance.flag * transform.right * 子弹飞行速度;

        Physics2D.IgnoreLayerCollision(8, 9);
    }

    private void Update()
    {
        if (Time.time - 子弹生成时的游戏时间 > 子弹销毁时间)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var colname = collision.name;

        if (!colname.Equals("bullet(Clone)") && !colname.Equals("Player"))
        {

            if (!是否触发)
            {
                是否触发 = true;
                //Physics2D.IgnoreCollision(collision, GetComponent<Collider2D>());
                if (collision.GetComponent<Enemy>())
                {
                    if (collision.GetComponent<Enemy>().是否正在死亡)
                    {
                        是否触发 = false;
                        return;
                    }
                    collision.GetComponent<Enemy>().TakeDamage(子弹伤害);
                }

                子弹刚体.velocity = new Vector2();
                Instantiate(hitEffect, collision.ClosestPoint(transform.position), Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}