using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class Enemy生命体 : Enemy
{
    [Min(0f)] public float 受伤变色时间;

    [Header("扫描区域设置")]
    [Tooltip("扫描攻击区域")]
    [Range(0.0f, 360.0f)]
    public float 视角方向;

    [Range(0.0f, 360.0f)] public float 视角FOV;

    [Min(0f)] public float 视野距离;

    public int 当前阶段;

    [Header("各阶段状态机设置")]
    public AnimatorController[] 动画控制器;

    private Rigidbody2D 刚体;
    private Animator 动画;
    private SpriteRenderer 纹理;
    private Color32 初始颜色;

    private Vector3 随机位置;
    private Vector3 初始缩放;

    private Vector3 当前位置
    {
        get => transform.position;
        set => transform.position = value;
    }

    private Vector3 玩家位置 => PlayerController.instance.transform.position;

    private float 与玩家距离 => Vector2.Distance(当前位置, 玩家位置);

    void Start()
    {
        初始缩放 = transform.localScale;
        刚体 = GetComponent<Rigidbody2D>();
        动画 = GetComponent<Animator>();
        纹理 = GetComponent<SpriteRenderer>();
        初始颜色 = 纹理.color;
        当前血量 = 最大血量;
        当前阶段 = 1;
    }

    void Update()
    {
        if (当前阶段 < 3 && 当前血量 <= 10)
        {
            //触发切换状态动画
            动画.SetTrigger("Ability");
            return;
        }

        if (当前阶段 is 3 && 当前血量 is 0)
        {
            动画.SetTrigger("Death");
            执行死亡();
        }
    }

    void 切换状态()
    {
        动画.runtimeAnimatorController = 动画控制器[当前阶段++];
        当前血量 = 最大血量;
    }

    void 执行死亡()
    {
        动画.runtimeAnimatorController = 动画控制器[0];
        动画.SetTrigger("Death");
    }

    void 销毁()
    {
        Destroy(gameObject);
    }

    public override void 掉血(int 伤害)
    {
        if (当前阶段<3 && 当前血量 <= 10) return;

        当前血量 -= 伤害;
        if (当前血量 > 0)
        {
            if (当前血量 % 5 == 0)
            {
                Instantiate(掉落物, 当前位置, transform.rotation);
            }

            纹理.color = new Color(0.99f, 0.3f, 0.3f, 1f);
            动画.SetTrigger("Hit");
            Invoke("恢复颜色", 受伤变色时间);
        }
        else
        {
            刚体.gravityScale = 1.5f;
            动画.SetTrigger("Death");
            执行死亡();
        }
    }

    private void OnDrawGizmosSelected()
    {
        var 红色 = new Color(1.0f, 0, 0, 0.1f);
        var 蓝色 = new Color(0.1f, 0.2f, 0.9f, 0.1f);

        // 攻击范围
        Vector3 forward = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        forward = Quaternion.Euler(0, 0, transform.localScale.x > 0 ? 视角方向 : -视角方向) * forward;
        if (GetComponent<SpriteRenderer>().flipX) forward.x = -forward.x;

        Vector3 endpoint = transform.position + (Quaternion.Euler(0, 0, 视角FOV * 0.5f) * forward);

        Handles.color = 红色;
        Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, 视角FOV,
            视野距离);
    }
}
