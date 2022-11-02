using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float speedX, jumpForce;
    public int flag;

    // 人物血量
    [Min(0)] public int health;
    [Min(0)] public float 受伤变色时间;
    [Min(0)] public int 水晶数量;


    [SerializeField] private AudioSource 内部音效器;
    [SerializeField] private AudioClip[] 移动音效;
    [SerializeField] private AudioClip 跳跃音效;
    [SerializeField] private AudioClip 落地音效;
    [SerializeField] private AudioSource 外部音效器;
    [SerializeField] private AudioClip 受伤音效;

    public GameObject 信息;

    public bool isAttacking;
    public float 触地射线检测长度 = 0.5f;
    public bool 是否触地 = false;

    public GameObject 跳跃粒子;
    public GameObject 落地粒子;
    public bool 是否落地 => plRigi.velocity.y < 0f && 是否触地;

    public float 移动音效切换时间;
    private float 移动音效切换计时;
    private int 移动音效序号;

    private Vector3 信息初始缩放;

    private Rigidbody2D plRigi;
    private Animator animator;
    private RaycastHit2D info;
    private GameObject[] arms;
    private float direction;
    private bool isOnGround;

    private float 落地前速度;

    private InputControler 行为控制;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        行为控制 = KeySetter.input;
        //行为控制 = new InputControler();
        行为控制.Player.Movement.performed += Movement;
        行为控制.Player.Movement.canceled += ctx => direction = 0;

        行为控制.Player.Movement.Enable();
    }

    void Start()
    {
        Time.timeScale = 1;
        plRigi = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        arms = GameObject.FindGameObjectsWithTag("arms");
        水晶数量 = 0;
        移动音效序号 = 0;
        移动音效切换计时 = 0;
        信息初始缩放 = 信息.transform.localScale;

        if (File.Exists(存档管理器.存档路径))
        {
            BinaryFormatter 二进制格式器 = new BinaryFormatter();
            FileStream 文件流 = File.Open(存档管理器.存档路径, FileMode.Open);

            存档 save = 二进制格式器.Deserialize(文件流) as 存档;
            文件流.Close();

            health = save.血量;
        }
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            跟随鼠标();
        }

        plRigi.velocity = new Vector2(direction * speedX, plRigi.velocity.y);
        animator.SetBool("isMove", plRigi.velocity.x != 0 ? true : false);

        if (plRigi.velocity.x != 0 && 是否触地)
        {
            if (!内部音效器.isPlaying)
            {
                内部音效器.PlayOneShot(移动音效[移动音效序号]);
                移动音效序号++;
                if (移动音效序号 >= 移动音效.Length)
                {
                    移动音效序号 = 0;
                }
            }
        }

        if (health < 0) health = 0;

        触地检测();
    }

    private void 跟随鼠标()
    {
        Vector3 mosPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0, -0.05f, 0);
        flag = mosPos.x > plRigi.position.x ? 1 : -1;
        信息.transform.localScale = new Vector3(flag * 信息初始缩放.x, 信息初始缩放.y, 信息初始缩放.z);
        
        // Gun.instance.换弹进度条.transform.localScale = new Vector3(flag * Gun.instance.换弹进度条缩放.x,
        //     Gun.instance.换弹进度条缩放.y, Gun.instance.换弹进度条缩放.z);
        
        transform.localScale = new Vector3(flag, 1, 1);
        foreach (var a in arms)
        {
            Vector3 drc = (mosPos - a.transform.position).normalized;
            float angle = 450 + (flag == 1 ? 0 : 180) - Mathf.Atan2(drc.x, drc.y) * Mathf.Rad2Deg;
            a.transform.eulerAngles = new Vector3(0, 0, angle);
        }


        /*foreach (var a in arms)
        {
            Vector3 display = Camera.main.WorldToScreenPoint(a.transform.position);
            Vector2 vector = Input.mousePosition - display;
            a.transform.up = vector;
        }*/
    }

    void Jump()
    {
        if (是否触地)
        {
            
            plRigi.velocity = new Vector2(plRigi.velocity.x, 0);

            plRigi.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            animator.SetTrigger("jump");
            内部音效器.PlayOneShot(跳跃音效);
            Instantiate(跳跃粒子, info.point, Quaternion.identity);
        }
    }

    public void 水晶增加(int 数量)
    {
        水晶数量 += 数量;
    }
    public bool 水晶消耗(int 数量)
    {
        if (水晶数量-数量<0)
        {
            return false;
        }

        水晶数量 -= 数量;
        return true;
    }
    
    void 触地检测()
    {
        Vector3 终点 = new Vector3(0, -1, 0);
        Debug.DrawLine(transform.position, transform.position + 终点 * 触地射线检测长度, Color.red);
        info = Physics2D.Raycast(transform.position, 终点, 触地射线检测长度, LayerMask.GetMask("Ground"));
        if (info.collider != null)
        {
            是否触地 = true;
            if (落地前速度 < 0 && plRigi.velocity.y == 0)
            {
                内部音效器.PlayOneShot(落地音效);
                Instantiate(落地粒子, info.point, Quaternion.identity);
            }
        }
        else
        {
            是否触地 = false;
        }

        落地前速度 = plRigi.velocity.y;
        animator.SetBool("是否触地", 是否触地);
    }

    // 人物掉血
    public void TakeDamage(int damage)
    {
        health -= damage;
        GetComponent<SpriteRenderer>().color = new Color(0.99f, 0.3f, 0.3f, 1f);
        GameObject.FindWithTag("Gun").GetComponent<SpriteRenderer>().color = new Color(0.99f, 0.3f, 0.3f, 1f);
        foreach (var a in arms)
        {
            a.GetComponent<SpriteRenderer>().color = new Color(0.99f, 0.3f, 0.3f, 1f);
        }

        外部音效器.PlayOneShot(受伤音效);
        animator.SetTrigger("掉血");
        Invoke("恢复颜色", 受伤变色时间);
    }

    public void 恢复颜色()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        GameObject.FindWithTag("Gun").GetComponent<SpriteRenderer>().color = Color.white;
        foreach (var a in arms)
        {
            a.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void 关闭换弹动画()
    {
        animator.ResetTrigger("换弹");
    }

    // 移动控制
    public void Movement(InputAction.CallbackContext context)
    {
        Vector2 val = context.ReadValue<Vector2>();
        if (val.y > 0)
        {
            Jump();
        }
        direction = val.x;
    }

    public void Attack(InputAction.CallbackContext context)
    {
        //if (context.started && !isAttacking) isAttacking = true;
    }

/*    private void OnCollisionEnter2D(Collision2D collision)
    {
        Grounding(collision, false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Grounding(collision, false);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Grounding(collision, true);
    }*/

/*    void Grounding(Collision2D col, bool exitState)
    {
        if (exitState)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                isOnGround = false;
            }
        }
        else
        {
            Vector2 contact0 = col.GetContact(0).normal;
            if (col.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")) && !isOnGround && contact0 == Vector2.up)
            {
                isOnGround = true;
                animator.ResetTrigger("jump");
            }
        }
        animator.SetBool("isOnGround", isOnGround);
    }*/
}