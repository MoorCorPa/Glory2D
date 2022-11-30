using UnityEngine;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class Enemy喷子怪 : Enemy
{
    [Min(0f)] public int 最小子弹数量;
    [Min(0f)] public int 最大子弹数量;
    [Min(0f)] public int 最小掉落物数量;
    [Min(0f)] public int 最大掉落物数量;
    [Min(0f)] public float 子弹x轴偏移;
    [Min(0f)] public float 子弹y轴偏移;
    [Min(0f)] public float 攻击僵直;
    [Min(0f)] public float 攻击前摇;
    [Min(0f)] public float 攻击间隔;
    [Min(0f)] public float 受伤变色时间;

    [Min(0f)] public float 墙体检测射线长度;

    public GameObject 子弹;
    public GameObject 血条框;
    public GameObject 血条;

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

    public Transform 炮口;

    private Vector3 当前位置
    {
        get => transform.position;
        set => transform.position = value;
    }

    private Vector3 玩家位置 => PlayerController.instance.transform.position;

    private float 与玩家距离;

    private void Start()
    {
        初始位置 = transform.position;
        初始缩放 = transform.localScale;
        刚体 = GetComponent<Rigidbody2D>();
        动画 = GetComponent<Animator>();
        纹理 = GetComponent<SpriteRenderer>();
        初始颜色 = 纹理.color;
        颜色透明度 = 初始颜色.r;
        当前血量 = 最大血量;
        缓存位置 = 当前位置;
        InvokeRepeating("播放攻击", 0, 攻击间隔);
        攻击僵直计时 = 攻击僵直;
        //InvokeRepeating("发射", 0, 1);
        血条框.SetActive(true);
        血条.GetComponent<Image>().fillAmount = 1;
    }

    private void Update()
    {
        var 墙体检测 = Physics2D.Raycast(当前位置 + Vector3.down * 0.5f, transform.right, 墙体检测射线长度,
            LayerMask.GetMask("Ground"));
        Debug.DrawLine(当前位置 + Vector3.down * 0.5f, 当前位置 + Vector3.down * 0.5f + transform.right * 墙体检测射线长度, Color.red);

        if (墙体检测)
        {
            transform.right = -transform.right;
            移动速度 = -移动速度;
        }

        攻击僵直计时 += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (攻击僵直计时 > 攻击僵直)
        {
            刚体.velocity = new Vector2(移动速度, 刚体.velocity.y);
        }
        else
        {
            刚体.velocity = new Vector2(0, 刚体.velocity.y);
        }
    }

    public Transform target;
    public Transform point;
    public float YThanTarget;
    public Vector3 velocity;

    public Vector3 计算抛物线()
    {
        float Gravity = Mathf.Abs(Physics2D.gravity.y * 子弹.GetComponent<Rigidbody2D>().gravityScale);
        float height_1, height_2;
        if (target.position.y > transform.position.y)
        {
            height_1 = YThanTarget + target.position.y - transform.position.y;
            height_2 = YThanTarget;
        }
        else
        {
            height_1 = YThanTarget;
            height_2 = transform.position.y + YThanTarget - target.position.y;
        }

        float time_1 = Mathf.Sqrt(2 * height_1 / Gravity);
        float time_2 = Mathf.Sqrt(2 * height_1 / Gravity) + Mathf.Sqrt(2 * height_2 / Gravity);
        Vector3 distance = target.position - transform.position;
        distance.y = 0;
        Vector3 speed = distance / time_2;
        velocity = speed + time_1 * Gravity * Vector3.up;
        return speed + time_1 * Gravity * Vector3.up;
    }

    private void 攻击()
    {
        计算抛物线();

        for (int i = 0; i < Random.Range(最小子弹数量, 最大子弹数量); i++)
        {
            GameObject b = Instantiate(子弹, 炮口.position, transform.rotation);
            var 速度 = velocity + new Vector3(Random.Range(-子弹x轴偏移, 子弹x轴偏移), Random.Range(-子弹y轴偏移, 子弹y轴偏移), 0);
            b.GetComponent<Rigidbody2D>().velocity = 速度;
        }
    }

    private void 播放攻击()
    {
        动画.SetTrigger("攻击");
        攻击僵直计时 = 0;
    }

    public override void 掉血(int 伤害)
    {
        当前血量 -= 伤害;
        if (当前血量 > 0)
        {
            if (当前血量 % 5 == 0)
            {
                Instantiate(掉落物, 炮口.position, transform.rotation);
            }

            血条.GetComponent<Image>().fillAmount = (float)当前血量 / (float)最大血量;
            if(血条.GetComponent<Image>().fillAmount < 0.5f) 血条框.GetComponentInChildren<Animator>().SetBool("狂暴",true);
        }
        else
        {
            for (int i = 0; i < Random.Range(最小掉落物数量, 最大掉落物数量); i++)
            {
                Instantiate(掉落物, 当前位置 + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0),
                    transform.rotation);
            }
            Destroy(gameObject);
            血条框.SetActive(false);
        }

        纹理.color = new Color(0.99f, 0.3f, 0.3f, 1f);
        Invoke("恢复颜色", 受伤变色时间);
    }

    public void 恢复颜色()
    {
        纹理.color = 初始颜色;
    }
}