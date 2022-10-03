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

    // 攻击时间
    public float attackTime;
    // 攻击间隔
    public float attackColldown;

    //索敌半径
    public float perceptionRadius;

    public Collider2D attackTriggerColl;
    // 随机移动距离
    private float moveX;

    public bool isAttacked = true;

    // Start is called before the first frame update
    public void Start()
    {
        //transform.position = Vector2.MoveTowards(transform.position, -transform.position, moveSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    public void Update()
    {
        // 血量为0消失
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        moveColldown();
    }

    // 随机移动
    public void moveColldown()
    {
        
        if (perceptionRadius > (transform.position - PlayerController.instance.transform.position).sqrMagnitude)
        {

            //Debug.Log("已进入攻击范围");
            transform.localScale = new Vector3(getFaceAt()? 1 : -1, 1, 1);
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(PlayerController.instance.transform.position.x, transform.position.y, transform.position.z), moveSpeed* Time.deltaTime);
        }
        else
        {
            moveTimeCount += Time.deltaTime;
            if (moveTimeCount > moveColldownTime)
            {
                moveX = Random.Range(-3, 3f) + transform.position.x;
                moveTimeCount = 0;
            }

            transform.localScale = new Vector3(moveX < transform.position.x ? 1 : -1, 1, 1);
            transform.position = Vector2.MoveTowards(transform.position, new Vector3(moveX, transform.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }
    }

    public void takeDamage(int damage)
    {
        health -= damage;
    }

    // 获取玩家
    public bool getFaceAt()
    {
        return transform.position.x>PlayerController.instance.transform.position.x ? true:false;
    }
}
