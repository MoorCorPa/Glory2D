using System;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy螃蟹 : Enemy
{
    public int 怪物ID;
    [Min(0f)] public float 攻击僵直;
    [Min(0f)] public float 攻击前摇;
    [Min(0f)] public float 攻击间隔;
    [Min(0f)] public float 受伤变色时间;


    [Min(0f)] public float 向左巡逻检测的x轴;
    [Min(0f)] public float 向右巡逻检测的x轴;
    [Min(0f)] public float 向左索敌的x轴;
    [Min(0f)] public float 向右索敌的x轴;
    [Min(0f)] public float 最小移动距离;

    [Min(0f)] public float 空路射线长度;

    public bool 是否远程;

    public bool 玩家是否在范围内;

    public GameObject 子弹;

    [Header("扫描区域设置")]
    [Tooltip("扫描攻击区域")]
    [Range(0.0f, 360.0f)]
    public float 视角方向;

    [Range(0.0f, 360.0f)] public float 视角FOV;

    [Min(0f)] public float 视野距离;
    [Min(0f)] public float 索敌半径;

    private float 攻击间隔计时;
    private float 攻击前摇计时;
    private float 攻击僵直计时;
    private float 颜色透明度;

    private Rigidbody2D 刚体;
    private Animator 动画;
    private SpriteRenderer 纹理;
    private Color32 初始颜色;

    private Vector3 随机位置;
    private Vector3 初始位置;
    private Vector3 初始缩放;
    private Vector3 缓存位置;

    private Vector3 当前位置
    {
        get => transform.position;
        set => transform.position = value;
    }

    private Vector3 玩家位置 => PlayerController.instance.transform.position;

    private float 与玩家距离;

    public void Start()
    {
        初始位置 = transform.position;
        初始缩放 = transform.localScale;
        刚体 = GetComponent<Rigidbody2D>();
        动画 = GetComponent<Animator>();
        纹理 = GetComponent<SpriteRenderer>();
        初始颜色 = 纹理.color;
        颜色透明度 = 初始颜色.r;
        动画.SetInteger("怪物ID", 怪物ID);
        当前血量 = 最大血量;
        攻击间隔计时 = 0;
        缓存位置 = 当前位置;
    }

    public void Update()
    {
        if (当前血量 > 0)
        {
            var 索敌范围射线 = Physics2D.Raycast(new Vector2(初始位置.x - 向左索敌的x轴, 当前位置.y), transform.right,
                Vector2.Distance(new Vector2(初始位置.x - 向左索敌的x轴, 当前位置.y), new Vector2(初始位置.x + 向右索敌的x轴, 当前位置.y)),
                LayerMask.GetMask("Player"));
            Debug.DrawLine(new Vector2(初始位置.x - 向左索敌的x轴, 当前位置.y), new Vector2(初始位置.x + 向右索敌的x轴, 当前位置.y),
                Color.blue);

            // var 检测范围射线 = Physics2D.Raycast(new Vector2(初始位置.x - 向左巡逻检测的x轴, 当前位置.y), transform.right,
            //     Vector2.Distance(new Vector2(初始位置.x - 向左巡逻检测的x轴, 当前位置.y), new Vector2(初始位置.x + 向右巡逻检测的x轴, 当前位置.y)),
            //     LayerMask.GetMask("Player"));
            Debug.DrawLine(new Vector2(初始位置.x - 向左巡逻检测的x轴, 当前位置.y), new Vector2(初始位置.x + 向右巡逻检测的x轴, 当前位置.y),
                Color.yellow);
            Debug.DrawLine(当前位置, 随机位置, Color.green);
            与玩家距离 = Vector2.Distance(当前位置, 玩家位置);
            var 射线 = Physics2D.Raycast(当前位置, 玩家位置 - 当前位置, Vector2.Distance(当前位置, 玩家位置),
                ~LayerMask.GetMask("Enemy") | ~LayerMask.GetMask("Player"));
            Debug.DrawLine(当前位置, 射线.point, Color.red);
            攻击间隔计时 += Time.deltaTime;
            攻击僵直计时 += Time.deltaTime;


            if (玩家在扇形范围() && ((射线 ? !射线.collider.CompareTag("地图碰撞区域") : true) || !是否远程))
            {
                if (攻击间隔 < 攻击间隔计时)
                {
                    攻击前摇计时 += Time.deltaTime;
                    if (攻击前摇 < 攻击前摇计时)
                    {
                        动画.SetBool("Run", false);
                        if (是否远程)
                        {
                            Instantiate(子弹, 当前位置, transform.rotation);
                        }
                        else
                        {
                            攻击僵直计时 = 0;
                            switch (Random.Range(0, 3))
                            {
                                case 0:
                                    动画.SetTrigger("Attack");
                                    break;
                                case 1:
                                    动画.SetTrigger("Attack 2");
                                    break;
                                case 2:
                                    动画.SetTrigger("Attack 3");
                                    break;
                                default:
                                    break;
                            }
                        }

                        攻击间隔计时 = 攻击前摇计时 = 0;
                        获取新位置();
                    }
                }
                else
                {
                    if (攻击僵直计时 > 攻击僵直)
                    {
                        随机移动();
                    }
                }
            }
            else if (索敌范围射线 && (与玩家距离 < 索敌半径) && 攻击间隔 < 攻击间隔计时 && Mathf.Abs(玩家位置.x - 当前位置.x) > 0.3f)
            {
                transform.localScale = new Vector3(当前位置.x - 玩家位置.x > 0 ? 初始缩放.x : -初始缩放.x, 初始缩放.y, 初始缩放.y);
                当前位置 = Vector2.MoveTowards(当前位置, new Vector3(玩家位置.x, 当前位置.y, 当前位置.z), 移动速度 * Time.deltaTime);
                动画.SetBool("Run", true);
            }
            else
            {
                if (攻击僵直计时 > 攻击僵直)
                {
                    随机移动();
                }
            }
        }
        else
        {
            刚体.velocity = new Vector2(0, 刚体.velocity.y);
            颜色透明度 -= Time.deltaTime * 100;
            纹理.color = new Color32(初始颜色.a, 初始颜色.b, 初始颜色.g, (byte)颜色透明度);
            if (颜色透明度 < 0)
            {
                销毁();
            }
        }
    }

    public virtual void 随机移动()
    {
        动画.SetBool("Run", true);
        if ((Vector2.Distance(当前位置, 随机位置) < 0.3f) || 前方路段为空())
        {
            获取新位置();
        }

        if (随机位置 == Vector3.zero)
        {
            获取新位置();
        }

        if (Math.Abs(当前位置.y - 随机位置.y) < 0.1f)
        {
            transform.localScale = new Vector3(当前位置.x - 随机位置.x > 0 ? 初始缩放.x : -初始缩放.x, 初始缩放.y, 初始缩放.y);
            当前位置 = Vector2.MoveTowards(当前位置, 随机位置, 移动速度 * Time.deltaTime);
        }
        else
        {
            获取新位置();
        }
    }

    public void 获取新位置()
    {
        var 向左移动距离 = Random.Range(-向左巡逻检测的x轴, -最小移动距离);
        var 向右移动距离 = Random.Range(最小移动距离, 向右巡逻检测的x轴);
        随机位置 = new Vector2(初始位置.x, 当前位置.y) + new Vector2(Random.Range(0, 2) > 0 ? 向左移动距离 : 向右移动距离, 0);
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
                Debug.Log("玩家出现在扇形范围内！");
                return true;
            }
        }

        return false;
    }

    public bool 前方路段为空()
    {
        Vector3 方向 = new Vector3(-transform.localScale.x, -1, 0);
        Debug.DrawLine(transform.position, transform.position + 方向.normalized * 空路射线长度, Color.yellow);

        if (Physics2D.Raycast(transform.position, 方向, 空路射线长度, LayerMask.GetMask("Ground", "GroundPlatform")))
        {
            return false;
        }

        return true;
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
            动画.SetTrigger("掉血");
            Invoke("恢复颜色", 受伤变色时间);
        }
        else
        {
            动画.SetTrigger("死亡");
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
            // Debug.Log("检测到玩家");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            玩家是否在范围内 = false;
            Debug.Log("检测到玩家");
        }
    }

    //绘制
    private void OnDrawGizmosSelected()
    {
        var 绿色 = new Color(0, 1.0f, 0, 0.4f);
        var 红色 = new Color(1.0f, 0, 0, 0.1f);
        var 蓝色 = new Color(0.0f, 0.0f, 1f, 0.1f);
        var 黄色 = new Color(1f, 0.9215686f, 0.01568628f, 0.1f);

        // 攻击范围
        Vector3 forward = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        forward = Quaternion.Euler(0, 0, transform.localScale.x > 0 ? 视角方向 : -视角方向) * forward;
        if (GetComponent<SpriteRenderer>().flipX) forward.x = -forward.x;

        Vector3 endpoint = transform.position + (Quaternion.Euler(0, 0, 视角FOV * 0.5f) * forward);

        Handles.color = 红色;
        Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, 视角FOV,
            视野距离);
        // 索敌范围
        Handles.color = 黄色;
        Handles.DrawSolidDisc(transform.position, Vector3.back, 索敌半径);
        // // 移动范围
        // Handles.DrawSolidRectangleWithOutline(
        //     new Rect(初始位置.x - 可移动x轴 + 可移动x轴偏移 * 2, 初始位置.y - 可移动y轴 + 可移动y轴偏移 * 2, 可移动x轴 * 2, 可移动y轴 * 2), 绿色, 蓝色);
    }
}