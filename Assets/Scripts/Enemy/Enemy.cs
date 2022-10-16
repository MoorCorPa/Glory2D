using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Random = UnityEngine.Random;

public abstract class Enemy : MonoBehaviour
{
    public Animator animator;

    [Header("扫描区域设置")] [Tooltip("扫描攻击区域")] [Range(0.0f, 360.0f)]
    public float viewDirection = 0.0f;

    [Range(0.0f, 360.0f)] public float viewFov;
    [Min(0f)] public float viewDistance;

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

    [Min(0)] public float 受伤变色时间;

    public bool 是否正在死亡 = false;

    public void Start()
    {
        animator = GetComponent<Animator>();
        //transform.position = Vector2.MoveTowards(transform.position, -transform.position, moveSpeed * Time.deltaTime);
    }

    public void Update()
    {
        if (!isAttacked)
        {
            if (attackTime < attackColldown)
            {
                attackTime += Time.deltaTime;
            }
            else
            {
                attackTime = 0;
                isAttacked = true;
            }
        }

        // 与玩家的距离
        float distance = Vector2.Distance(transform.position, PlayerController.instance.transform.position);
        // 正前方的向量
        Vector2 norVec = transform.rotation * (transform.localScale.x > 0 ? Vector2.left : Vector2.right);
        Vector3 v = Quaternion.Euler(0, 0, transform.localScale.x > 0 ? viewDirection : -viewDirection) * norVec;
        // 敌人与的玩家方向向量 
        Vector2 temVec = PlayerController.instance.transform.position - transform.position;
        // 两个向量的夹角
        float jiajiao = Mathf.Acos(Vector2.Dot(v.normalized, temVec.normalized)) * Mathf.Rad2Deg;

        if (distance < viewDistance)
        {
            if (jiajiao <= viewFov * 0.5f)
            {
                Debug.Log("玩家出现在扇形范围内！");

                if (isAttacked)
                {
                    Debug.Log("冷却完毕开始攻击动画");
                    AttackAnim();
                }
            }
        }
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
                        transform.position.z), moveSpeed * 0.02f);
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
                new Vector3(moveX, transform.position.y, transform.position.z), moveSpeed * 0.02f);
        }
    }

    // 怪物掉血
    public void TakeDamage(int damage)
    {
        Debug.Log("怪物掉血!!!");
        health -= damage;
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
            animator.SetTrigger("掉血");
            Invoke("恢复颜色", 受伤变色时间);
        }
    }

    public void 怪物死亡()
    {
        Debug.Log("怪物死亡!!!");
        animator.ResetTrigger("死亡");
        Destroy(gameObject);
    }

    public void 关闭掉血动画()
    {
        animator.ResetTrigger("掉血");
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


    // 激活攻击动画
    public void AttackAnim()
    {
        ActiveAttack();
        transform.localScale = new Vector3(getFaceAt() ? 1 : -1, 1, 1);
        animator.SetTrigger("Attack");
    }

    // 攻击成功,判断掉血
    public void Cause()
    {
        if (isAttackHitRange)
        {
            PlayerController.instance.TakeDamage(damage);
        }
        else
        {
            Debug.Log("攻击未命中!!!");
        }
    }

    public void ActiveAttack()
    {
        isAttacked = false;
    }

    public void resetAnim()
    {
        animator.ResetTrigger("Attack");
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) isAttackHitRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) isAttackHitRange = false;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isNearPlayer = true;
            // Debug.Log(other.rigidbody.velocity * other.rigidbody.mass);
            // GetComponent<Rigidbody2D>().AddForce(other.rigidbody.velocity * other.rigidbody.mass ,ForceMode2D.Force);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player")) isNearPlayer = false;
    }

    //绘制
    private void OnDrawGizmosSelected()
    {
        // 攻击范围
        Vector3 forward = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        forward = Quaternion.Euler(0, 0, transform.localScale.x > 0 ? viewDirection : -viewDirection) * forward;
        if (GetComponent<SpriteRenderer>().flipX) forward.x = -forward.x;

        Vector3 endpoint = transform.position + (Quaternion.Euler(0, 0, viewFov * 0.5f) * forward);

        Handles.color = new Color(0, 1.0f, 0, 0.2f);
        Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, viewFov,
            viewDistance);

        // 索敌范围
        Handles.color = new Color(1.0f, 0, 0, 0.1f);
        Handles.DrawSolidDisc(transform.position, Vector3.back, Mathf.Sqrt(perceptionRadius));
    }
}