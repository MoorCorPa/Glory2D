using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyRemake : MonoBehaviour
{
    public int 怪物ID;
    [Min(0f)] public int 最大血量;
    [Min(0f)] public int 当前血量;
    [Min(0f)] public int 攻击力;

    [Header("扫描区域设置")]
    [Tooltip("扫描攻击区域")]
    [Range(0.0f, 360.0f)]
    public float 视角方向 = 0.0f;

    [Range(0.0f, 360.0f)] public float 视角FOV;
    [Min(0f)] public float 视野距离;

    [Min(0f)] public float 移动速度;
    [Min(0f)] public float 攻击半径;
    [Min(0f)] public float 索敌半径;
    [Min(0f)] public float 攻击间隔;
    [Min(0f)] public float 受伤变色时间;

    public float 空路射线长度;

    public bool 是否远程;

    public GameObject 子弹;

    private float 攻击间隔计时;
    private float 颜色透明度;

    private Rigidbody2D 刚体;
    private Animator 动画;
    private SpriteRenderer 纹理;
    private Color32 初始颜色;

    private Vector3 随机位置;
    private Vector3 初始位置;
    private Vector3 缓存位置;
   
    private Vector3 当前位置
    {
        get => transform.position;
        set => transform.position = value;
    }

    private Vector3 玩家位置 => PlayerController.instance.transform.position;

    private float 与玩家距离;

    // Start is called before the first frame update
    void Start()
    {
        刚体 = GetComponent<Rigidbody2D>();
        动画 = GetComponent<Animator>();
        纹理 = GetComponent<SpriteRenderer>();
        初始颜色 = 纹理.color;
        颜色透明度 = 初始颜色.r;
        动画.SetInteger("怪物ID", 怪物ID);
        初始位置 = transform.position;
        当前血量 = 最大血量;
        攻击间隔计时 = 0;
        缓存位置 = 当前位置;
    }

    // Update is called once per frame
    void Update()
    {
        if (当前血量 > 0)
        {
            与玩家距离 = Vector2.Distance(当前位置, 玩家位置);
            var 射线 = Physics2D.Raycast(当前位置, 玩家位置 - 当前位置, Vector2.Distance(当前位置, 玩家位置), ~LayerMask.GetMask("Enemy"));
            Debug.DrawLine(当前位置, 射线.point, Color.red);
            攻击间隔计时 += Time.deltaTime;

            if (攻击间隔 < 攻击间隔计时)
            {
                if (是否远程)
                {
                    if (与玩家距离 < 攻击半径 && !射线.collider.CompareTag("地图碰撞区域"))
                    {
                        Instantiate(子弹, 当前位置, transform.rotation);
                    }
                    else
                    {
                        随机移动();
                    }
                }
                else
                {
                    if (与玩家距离 < 索敌半径 && !前方路段为空())
                    {
                        if (玩家在扇形范围())
                        {
                            动画.SetTrigger("Attack");
                        }
                        else
                        {
                            当前位置 = Vector2.MoveTowards(当前位置, new Vector3(玩家位置.x, 当前位置.y, 当前位置.z), 移动速度 * Time.deltaTime);
                            Debug.Log("宝贝我来咯");
                        }
                    }
                    else
                    {
                        随机移动();
                    }
                }
            }
            else
            {
                随机移动();
            }
            转向();
        }
        else
        {
            颜色透明度 -= Time.deltaTime * 100;
            纹理.color = new Color32(初始颜色.a, 初始颜色.b, 初始颜色.g, (byte)颜色透明度);
            if (颜色透明度 < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public virtual void 随机移动()
    {
        if ((Vector2.Distance(当前位置, 随机位置) < 0.3f) || 前方路段为空())
        {
            获取新位置();
        }
        当前位置 = Vector2.MoveTowards(当前位置, 随机位置, 移动速度 * Time.deltaTime);
    }

    public void 获取新位置()
    {
        随机位置 = new Vector3(当前位置.x+Random.Range(-3, 3f), 当前位置.y, 当前位置.z);
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
        Debug.Log("戳啦，不在嘛");
        return false;
    }

    public bool 前方路段为空()
    {
        Vector3 方向 = new Vector3(-transform.localScale.x, -1, 0);
        Debug.DrawLine(transform.position, transform.position + 方向.normalized * 空路射线长度, Color.yellow);

        if (Physics2D.Raycast(transform.position, 方向, 空路射线长度, LayerMask.GetMask("Ground")))
        {
            Debug.Log("空路段射线测试");
            return false;
        }
        Debug.Log("戳啦，不空嘛");
        return true;
    }

    public void 转向()
    {
        if (缓存位置.x - 当前位置.x != 0)
        {
            transform.localScale = new Vector3(缓存位置.x > 当前位置.x ? 1 : -1, 1, 1);
        }
        缓存位置 = 当前位置;
    }

    public void 掉血(int 伤害)
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
            刚体.gravityScale = 1;
            动画.SetTrigger("死亡");
        }
    }

    public void 恢复颜色()
    {
        纹理.color = 初始颜色;
    }

    //绘制
    private void OnDrawGizmosSelected()
    {
        // 攻击范围
        Vector3 forward = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        forward = Quaternion.Euler(0, 0, transform.localScale.x > 0 ? 视角方向 : -视角方向) * forward;
        if (GetComponent<SpriteRenderer>().flipX) forward.x = -forward.x;

        Vector3 endpoint = transform.position + (Quaternion.Euler(0, 0, 视角FOV * 0.5f) * forward);

        Handles.color = new Color(0, 1.0f, 0, 0.2f);
        Handles.DrawSolidArc(transform.position, -Vector3.forward, (endpoint - transform.position).normalized, 视角FOV,
            视野距离);

        // 索敌范围
        Handles.color = new Color(1.0f, 0, 0, 0.1f);
        Handles.DrawSolidDisc(transform.position, Vector3.back, Mathf.Sqrt(索敌半径));
    }
}
