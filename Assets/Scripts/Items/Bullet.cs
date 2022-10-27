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
    
    [SerializeField] private GameObject 子弹反馈粒子;

    private float 子弹生成时的游戏时间;
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
        if (!collision.CompareTag("子弹") && !collision.CompareTag("Player") && !collision.CompareTag("攻击范围"))
        {

            if (!是否触发)
            {
                是否触发 = true;
                //Physics2D.IgnoreCollision(collision, GetComponent<Collider2D>());
                if (collision.GetComponent<Enemy>())
                {
                    collision.GetComponent<Enemy>().掉血(子弹伤害);
                }
                

                子弹刚体.velocity = new Vector2();
                //transform.position = collision.ClosestPoint(transform.position - transform.right * 0.07f);
                transform.position = collision.ClosestPoint(transform.position);
                //transform.localScale *= 0.6f;
                // Instantiate(子弹反馈, collision.ClosestPoint(transform.position), Quaternion.identity);
                GetComponent<Animator>().SetTrigger("击中");
                // Destroy(gameObject);
            }
        }
    }

    public void 子弹销毁()
    {
        Destroy(gameObject);
    }
}