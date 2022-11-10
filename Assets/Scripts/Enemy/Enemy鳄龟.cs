using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy鳄龟 : Enemy
{
    [Min(0f)] public float 攻击前摇;
    [Min(0f)] public float 攻击间隔;
    [Min(0f)] public float 攻击僵值;
    [Min(0f)] public float 背刺攻击间隔;
    [Min(0f)] public float 受伤变色时间;

    public bool 玩家是否在范围内;

    public GameObject[] 背刺;
    public GameObject[] 显示的背刺;

    public GameObject 攻击特效;
    
    [Header("扫描区域设置")] [Tooltip("扫描攻击区域")] [Range(0.0f, 360.0f)]
    public float 视角方向;

    [Range(0.0f, 360.0f)] public float 视角FOV;

    [Min(0f)] public float 视野距离;

    private float 攻击间隔计时;
    private float 攻击前摇计时;
    private float 攻击僵直计时;
    private float 背刺攻击间隔计时;

    private Rigidbody2D 刚体;
    private Animator 动画;
    private SpriteRenderer 纹理;
    private Color32 初始颜色;

    private Vector3 随机位置;
    private Vector3 初始缩放;

    private bool 开始向玩家移动;

    private Vector3 当前位置
    {
        get => transform.position;
        set => transform.position = value;
    }

    private Vector3 玩家位置 => PlayerController.instance.transform.position;

    private float 与玩家距离 => Vector2.Distance(当前位置, 玩家位置);

    public void Start()
    {
        初始缩放 = transform.localScale;
        刚体 = GetComponent<Rigidbody2D>();
        动画 = GetComponent<Animator>();
        纹理 = GetComponent<SpriteRenderer>();
        初始颜色 = 纹理.color;
        当前血量 = 最大血量;
        攻击间隔计时 = 0;
        开始向玩家移动 = false;
    }

    public void Update()
    {
        if (当前血量 <= 0) return;
        // var 玩家检测射线 = Physics2D.Raycast(当前位置, 玩家位置 - 当前位置, Vector2.Distance(当前位置, 玩家位置), ~LayerMask.GetMask("Enemy") | ~LayerMask.GetMask("Player"));
        // Debug.DrawLine(当前位置, 玩家检测射线.point, Color.red);
        攻击间隔计时 += Time.deltaTime;
        动画.SetBool("开始向玩家移动", 开始向玩家移动);
        攻击僵直计时 += Time.deltaTime;
        背刺攻击间隔计时 += Time.deltaTime;
        if (背刺攻击间隔 < 背刺攻击间隔计时)
        {
            foreach (var 刺 in 显示的背刺)
            {
                if (刺.GetComponent<SpriteRenderer>().color.a >= 0.5)
                {
                    刺.GetComponent<SpriteRenderer>().color = Color.white;
                }
                else
                {
                    刺.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Mathf.Clamp(攻击间隔计时 / 攻击间隔, 0, 1));
                }
            }
        }

        if (攻击前摇计时 > 0 | (玩家在扇形范围() & Mathf.Abs(玩家位置.x - 当前位置.x) < 视野距离 - 0.5f))
        {
            开始向玩家移动 = false;
            动画.SetInteger("状态", 0);

            if (攻击间隔 < 攻击间隔计时)
            {
                攻击前摇计时 += Time.deltaTime;
                if (攻击前摇 < 攻击前摇计时)
                {
                    攻击间隔计时 = 攻击前摇计时 = 攻击僵直计时 = 0;
                    动画.SetTrigger("攻击");
                }
                else
                {
                    攻击特效.GetComponent<Animator>().Play("攻击特效");
                }
            }
        }
        else if (攻击间隔 < 攻击间隔计时 && 背刺攻击间隔 < 背刺攻击间隔计时)
        {
            if (Random.Range(0, 6) != 0) return;
            for (var i = 0; i < 背刺.Length; i++)
            {
                显示的背刺[i].GetComponent<SpriteRenderer>().color = Color.clear;
                Instantiate(背刺[i], 显示的背刺[i].transform.position, transform.rotation);
            }
            攻击间隔计时 = 背刺攻击间隔计时= 0;
        }
        else if (攻击僵直计时 > 攻击僵值)
        {
            transform.localScale = new Vector3(当前位置.x - 玩家位置.x > 0 ? 初始缩放.x : -初始缩放.x, 初始缩放.y, 初始缩放.y);
            开始向玩家移动 = true;
        }
        else
        {
            开始向玩家移动 = false;
        }
    }


    private void FixedUpdate()
    {
        if (开始向玩家移动)
        {
            当前位置 = Vector2.MoveTowards(当前位置, new Vector3(玩家位置.x, 当前位置.y, 当前位置.z), 移动速度);
        }
    }

    public bool 玩家在扇形范围()
    {
        Vector2 正前方向量 = transform.rotation * (transform.localScale.x > 0 ? Vector2.left : Vector2.right);
        Vector3 v = Quaternion.Euler(0, 0, transform.localScale.x > 0 ? 视角方向 : -视角方向) * 正前方向量;
        Vector2 到玩家的向量 = 玩家位置 - 当前位置;
        // 两个向量的夹角
        float 夹角 = Mathf.Acos(Vector2.Dot(v.normalized, 到玩家的向量.normalized)) * Mathf.Rad2Deg;

        if (与玩家距离 < 视野距离)
        {
            if (夹角 <= 视角FOV * 0.5f)
            {
                return true;
            }
        }

        return false;
    }


    public void 判断是否击中玩家()
    {
        if (玩家是否在范围内)
        {
            PlayerController.instance.TakeDamage(攻击力);
        }
    }

    public override void 掉血(int 伤害)
    {
        当前血量 -= 伤害;
        if (当前血量 > 0)
        {
            纹理.color = new Color(0.99f, 0.3f, 0.3f, 1f);
            Invoke("恢复颜色", 受伤变色时间);
        }
        else
        {
            刚体.velocity = new Vector2(0, 刚体.velocity.y);
            //动画.Play("鳄龟死亡");
        }
    }

    public void 恢复颜色()
    {
        纹理.color = 初始颜色;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            玩家是否在范围内 = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            玩家是否在范围内 = false;
        }
    }

    //绘制
    private void OnDrawGizmosSelected()
    {
        var 红色 = new Color(1.0f, 0, 0, 0.1f);

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