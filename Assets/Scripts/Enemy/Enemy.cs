using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Random = UnityEngine.Random;

public abstract class Enemy : MonoBehaviour
{
    public Animator animator;

    // 血量
    public int health;

    // 攻击力
    public int damage;

    // 移动速度
    public float moveSpeed;

    // 移动计时
    public float moveTimeCount = 0;

    // 移动间隔
    public float moveColldownTime;

    // 攻击计时
    public float attackTime = 0;

    // 攻击间隔
    public float attackColldown;

    //索敌半径
    public float perceptionRadius;

    public Collider2D attackTriggerColl;

    // 随机移动距离
    public float moveX;

    // 是否可攻击
    public bool isAttacked = true;

    // 是否已接近玩家
    public bool isNearPlayer = false;
    
    // 玩家是否在攻击命中范围
    public bool isAttackHitRange = false;

    [Min(0)]
    public float 受伤变色时间;
    
    
    public bool 是否正在死亡 = false;
    public void Start()
    {
        animator = GetComponent<Animator>();
        //transform.position = Vector2.MoveTowards(transform.position, -transform.position, moveSpeed * Time.deltaTime);
    }

    public void Update()
    {

    }

    public void FixedUpdate()
    {
        MoveController();
    }

    // 随机移动 移动控制器
    private void MoveController()
    {
        if (perceptionRadius > (transform.position - PlayerController.instance.transform.position).sqrMagnitude)
        {
            if (!isNearPlayer)
            {   
                // 进入锁定范围
                Debug.Log("进入锁定范围");
                transform.localScale = new Vector3(getFaceAt() ? 1 : -1, 1, 1);
                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector3(PlayerController.instance.transform.position.x, transform.position.y,
                        transform.position.z), moveSpeed*0.02f);
            }
            else
            {
                if (!isAttacked)
                {
                    animator.SetTrigger("Static");
                }
            }
        }
        else
        {
            //随机移动
            moveTimeCount += Time.deltaTime;
            if (moveTimeCount > moveColldownTime)
            {
                moveX = Random.Range(-3, 3f) + transform.position.x;
                moveTimeCount = 0;
            }

            transform.localScale = new Vector3(moveX < transform.position.x ? 1 : -1, 1, 1);
            transform.position = Vector2.MoveTowards(transform.position,
                new Vector3(moveX, transform.position.y, transform.position.z), moveSpeed *0.02f);
        }
    }

    // 怪物掉血
    public void TakeDamage(int damage)
    {
        health -= damage;
        // 血量为0消失
        if (health <= 0)
        {
            if (!是否正在死亡)
            {
                是否正在死亡 = true;
                animator.SetTrigger("死亡");
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(0.99f, 0.3f, 0.3f, 1f);
            Invoke("恢复颜色",受伤变色时间);
        }
        
    }

    public void 怪物死亡()
    {
        animator.ResetTrigger("死亡");
        Destroy(gameObject);
    }

    public void 恢复颜色()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    
    // 获取玩家
    public bool getFaceAt()
    {
        return transform.position.x > PlayerController.instance.transform.position.x ? true : false;
    }
}