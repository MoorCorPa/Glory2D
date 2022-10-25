using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Random = UnityEngine.Random;

public abstract class EnemyFly : MonoBehaviour
{
    public int 怪物ID;
    [Min(0f)] public int 最大血量;
    [Min(0f)] public int 当前血量;
    [Min(0f)] public int 攻击力;
    [Min(0f)] public float 移动速度;
    [Min(0f)] public float 攻击半径;
    [Min(0f)] public float 索敌半径;
    [Min(0f)] public float 攻击间隔;
    [Min(0f)] public float 受伤变色时间;

    public bool 是否远程;

    [Min(0f)] public float 可移动x轴;
    [Min(0f)] public float 可移动y轴;
    public float 可移动x轴偏移;
    public float 可移动y轴偏移;

    public GameObject 子弹;

    private float 攻击间隔计时;
    private float 颜色透明度;

    private Rigidbody2D 刚体;
    private Animator 动画;
    private SpriteRenderer 纹理;
    private Color32 初始颜色;

    private Vector3 随机位置;
    private Vector3 初始位置;

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
        初始颜色 = 纹理.color;
        颜色透明度 = 初始颜色.r;
        动画.SetInteger("怪物ID", 怪物ID);
        初始位置 = transform.position;
        当前血量 = 最大血量;
        攻击间隔计时 = 0;
        随机位置 = 获取可移动范围内随机坐标();
    }

    public void Update()
    {
        if (当前血量 > 0)
        {
            var 射线 = Physics2D.Raycast(当前位置, 玩家位置 - 当前位置);
            Debug.DrawLine(当前位置, 射线.point, Color.red);

            var 随机位置检测射线 = Physics2D.Raycast(当前位置, 随机位置 - 当前位置, Mathf.Sqrt((随机位置 - 当前位置).sqrMagnitude),
                LayerMask.GetMask("Ground"));
            Debug.DrawLine(当前位置, 随机位置, Color.green);

            var 与玩家的距离 = Vector2.Distance(当前位置, 玩家位置);
            if (与玩家的距离 < 攻击半径)
            {
                if (是否远程)
                {
                    攻击间隔计时 += Time.deltaTime;
                    if (攻击间隔 < 攻击间隔计时)
                    {
                        Instantiate(子弹, 当前位置, transform.rotation);
                        攻击间隔计时 = 0;
                    }
                }
                else
                {
                    //摁创
                }
            }
            else if (与玩家的距离 < 索敌半径 && !射线.collider.CompareTag("地图碰撞区域"))
            {
                当前位置 = Vector2.MoveTowards(当前位置, 玩家位置 - 当前位置 * (攻击半径 / 与玩家的距离) + 当前位置, 移动速度 * Time.deltaTime);
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
            颜色透明度 -= Time.deltaTime * 100;
            纹理.color = new Color32(初始颜色.a, 初始颜色.b, 初始颜色.g, (byte) 颜色透明度);
            if (颜色透明度 < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void FixedUpdate()
    {
    }

    public void 随机移动()
    {
        当前位置 = Vector2.MoveTowards(当前位置, 随机位置, 移动速度 * Time.deltaTime);
        if ((Vector2.Distance(当前位置, 随机位置) < Mathf.Epsilon))
        {
            随机位置 = 获取可移动范围内随机坐标();
        }
    }

    public Vector2 获取可移动范围内随机坐标()
    {
        var 随机坐标 = new Vector2(Random.Range(-可移动x轴, 可移动x轴), Random.Range(-可移动y轴, 可移动y轴));
        随机坐标 -= new Vector2(可移动x轴偏移, 可移动y轴偏移);
        return 随机坐标;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        // if (other.gameObject.CompareTag("地图碰撞区域"))
        // {
        //     随机位置 = 获取可移动范围内随机坐标();
        // }
    }

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
            new Rect(初始位置.x - 可移动x轴 + 可移动x轴偏移 * 2, 初始位置.y - 可移动y轴 + 可移动y轴偏移 * 2, 可移动x轴 * 2, 可移动y轴 * 2), 绿色, 蓝色);
    }
}