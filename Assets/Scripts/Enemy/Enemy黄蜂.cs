using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy黄蜂 : Enemy
{
    [Min(0f)] public float 冲击力;
    [Min(0f)] public float 攻击前摇;
    [Min(0f)] public float 攻击半径;
    [Min(0f)] public float 索敌半径;
    [Min(0f)] public float 攻击间隔;
    [Min(0f)] public float 受伤变色时间;
    [Min(0f)] public float 最大升力;
    [Min(0f)] public float 最小升力;
    [Min(0f)] public float 死亡消失速度;
    [Min(0f)] public float 死亡消失透明度;
    [Min(0f)] public float 攻击间隔计时间隔;
    [Min(0f)] public float 墙体碰撞检测;
    [Min(0f)] public int 最小掉落物数量;
    [Min(0f)] public int 最大掉落物数量;

    public bool 是否远程;

    [Min(0f)] public float 可移动x轴;
    [Min(0f)] public float 可移动y轴;
    public float 可移动x轴偏移;
    public float 可移动y轴偏移;

    public GameObject 子弹;
    public GameObject 尾刺;

    private float 攻击间隔计时;
    private float 攻击间隔计时间隔计时;
    private float 攻击前摇计时;
    private float 颜色透明度;

    private Rigidbody2D 刚体;
    private Animator 动画;
    private SpriteRenderer 纹理;
    private Color32 初始颜色;

    private Vector3 随机位置;
    private Vector3 初始位置;
    private Vector3 初始缩放;
    private Vector3 缓存位置;

    private bool 是否启动;

    private Vector3 当前位置
    {
        get => transform.position;
        set => transform.position = value;
    }

    private Vector3 玩家位置 => PlayerController.instance.transform.position;

    public void Start()
    {
        刚体 = GetComponent<Rigidbody2D>();
        动画 = GetComponent<Animator>();
        纹理 = GetComponent<SpriteRenderer>();
        是否启动 = true;
        初始颜色 = 纹理.color;
        颜色透明度 = 初始颜色.r;
        初始位置 = transform.position;
        初始缩放 = transform.localScale;
        当前血量 = 最大血量;
        攻击间隔计时 = 攻击前摇计时 = 0;
        随机位置 = 获取可移动范围内随机坐标();
        缓存位置 = 当前位置;
    }

    public void Update()
    {
        if (当前血量 > 0)
        {
            刚体.AddForce(transform.up * Random.Range(最小升力, 最大升力), ForceMode2D.Force);
            var 射线 = Physics2D.Raycast(当前位置, 玩家位置 - 当前位置, Vector2.Distance(当前位置, 玩家位置), ~LayerMask.GetMask("Enemy"));
            Debug.DrawLine(当前位置, 射线.point, Color.red);

            var 随机位置检测射线 = Physics2D.Raycast(当前位置, 随机位置 - 当前位置, Mathf.Sqrt((随机位置 - 当前位置).sqrMagnitude),
                LayerMask.GetMask("Ground"));
            Debug.DrawLine(当前位置, 随机位置, Color.green);

            攻击间隔计时间隔计时 += Time.deltaTime;
            if (攻击间隔计时间隔 < 攻击间隔计时间隔计时)
            {
                攻击间隔计时 += Time.deltaTime;
                if (尾刺.GetComponent<SpriteRenderer>().color.a >= 0.5)
                {
                    尾刺.GetComponent<SpriteRenderer>().color = Color.white;
                }
                else
                {
                    尾刺.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Mathf.Clamp(攻击间隔计时 / 攻击间隔, 0, 1));
                }
            }

            var 与玩家的距离 = Vector2.Distance(当前位置, 玩家位置);
            if (与玩家的距离 < 攻击半径 && !射线.collider.CompareTag("地图碰撞区域"))
            {
                if (攻击间隔 < 攻击间隔计时)
                {
                    攻击前摇计时 += Time.deltaTime;
                    if (攻击前摇 < 攻击前摇计时)
                    {
                        if (是否远程)
                        {
                            Instantiate(子弹, 尾刺.transform.position, transform.rotation);
                            尾刺.GetComponent<SpriteRenderer>().color = Color.clear;
                        }
                        else
                        {
                            //摁创
                            if (!射线.collider.CompareTag("地图碰撞区域"))
                            {
                                刚体.AddForce((玩家位置 - 当前位置).normalized * 冲击力, ForceMode2D.Impulse);
                            }
                        }

                        随机位置 = 获取可移动范围内随机坐标();
                        攻击前摇计时 = 攻击间隔计时 = 攻击间隔计时间隔计时 = 0;
                    }
                }
                else
                {
                    随机移动();
                }
            }
            else if (与玩家的距离 < 索敌半径 && !射线.collider.CompareTag("地图碰撞区域") && 攻击间隔 < 攻击间隔计时)
            {
                当前位置 = Vector2.MoveTowards(当前位置, (玩家位置 - 当前位置) * (攻击半径 / 与玩家的距离) + 当前位置, 移动速度 * Time.deltaTime);
            }
            else
            {
                随机移动();
            }

            if (随机位置检测射线)
            {
                随机位置 = 获取可移动范围内随机坐标();
            }
        }
        else
        {
            刚体.velocity = new Vector2(0, 刚体.velocity.y);
            颜色透明度 -= Time.deltaTime * 死亡消失速度;
            纹理.color = new Color32(初始颜色.a, 初始颜色.b, 初始颜色.g, (byte) 颜色透明度);
            if (颜色透明度 < 死亡消失透明度)
            {
                Destroy(gameObject);
                Instantiate(掉落物, transform.position, transform.rotation);
            }
        }

        转向();
    }

    public void FixedUpdate()
    {
    }

    public void 随机移动()
    {
        当前位置 = Vector2.MoveTowards(当前位置, 随机位置, 移动速度 * Time.deltaTime);
        if ((Vector2.Distance(当前位置, 随机位置) < 墙体碰撞检测))
        {
            随机位置 = 获取可移动范围内随机坐标();
        }
    }

    public void 转向()
    {
        if (缓存位置.x - 当前位置.x != 0)
        {
            transform.localScale = new Vector3(缓存位置.x > 当前位置.x ? 初始缩放.x : -初始缩放.x, 初始缩放.y, 初始缩放.z);
        }

        缓存位置 = 当前位置;
    }

    public Vector2 获取可移动范围内随机坐标()
    {
        var 随机坐标 = new Vector2(Random.Range(初始位置.x - 可移动x轴, 初始位置.x + 可移动x轴),
            Random.Range(初始位置.y - 可移动y轴, 初始位置.y + 可移动y轴));
        随机坐标 += new Vector2(可移动x轴偏移, 可移动y轴偏移);
        return 随机坐标;
    }

    public override void 掉血(int 伤害)
    {
        当前血量 -= 伤害;
        if (当前血量 > 0)
        {
            if (当前血量 % 5 == 0)
            {
                Instantiate(掉落物, 当前位置, transform.rotation);
            }

            纹理.color = new Color(0.99f, 0.3f, 0.3f, 1f);
            动画.SetTrigger("掉血");
            Invoke("恢复颜色", 受伤变色时间);
        }
        else
        {
            for (int i = 0; i < Random.Range(最小掉落物数量, 最大掉落物数量); i++)
            {
                Instantiate(掉落物, 当前位置 + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0),
                    transform.rotation);
            }
            刚体.gravityScale = 1.5f;
            动画.SetTrigger("死亡");
        }
    }

    public void 恢复颜色()
    {
        纹理.color = 初始颜色;
    }


#if UNITY_EDITOR
    //绘制
    private void OnDrawGizmosSelected()
    {
        var 绿色 = new Color(0, 1.0f, 0, 0.4f);
        var 红色 = new Color(1.0f, 0, 0, 0.1f);
        var 蓝色 = new Color(0.0f, 0.0f, 1f, 0.1f);
        var 黄色 = new Color(1f, 0.9215686f, 0.01568628f, 0.1f);
        // 攻击范围
        Handles.color = 红色;
        Handles.DrawSolidDisc(当前位置, Vector3.back, 攻击半径);
        // 索敌范围
        Handles.color = 黄色;
        Handles.DrawSolidDisc(当前位置, Vector3.back, 索敌半径);
        // 移动范围
        Handles.DrawSolidRectangleWithOutline(
            是否启动
                ? new Rect(初始位置.x - 可移动x轴 + 可移动x轴偏移, 初始位置.y - 可移动y轴 + 可移动y轴偏移, 可移动x轴 * 2, 可移动y轴 * 2)
                : new Rect(当前位置.x - 可移动x轴 + 可移动x轴偏移, 当前位置.y - 可移动y轴 + 可移动y轴偏移, 可移动x轴 * 2, 可移动y轴 * 2), 绿色, 蓝色);
        // 墙体检测范围
        Handles.color = 蓝色;
        Handles.DrawSolidDisc(当前位置, Vector3.back, 墙体碰撞检测);
    }
#endif
}