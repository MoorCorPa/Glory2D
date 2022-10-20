using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    float direction;

    private Rigidbody2D plRigi;
    public Animator animator;

    public float speedX, jumpForce;

    private bool isOnGround;
    public bool 是否触地 = false;
    public float 触地射线检测长度 = 0.5f;
    public bool isAttacking;

    private GameObject[] arms;
    public int flag;

    // 人物血量
    [Min(0)]
    public int health;
    [Min(0)]
    public float 受伤变色时间;
    
    private void Awake()
    {
        instance = this; 
    }

    void Start()
    {
        plRigi = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        arms = GameObject.FindGameObjectsWithTag("arms");
        // speedX = 5f;
        // jumpForce = 5f;

    }

    void Update()
    {
        followMouse();
        Jump();

        plRigi.velocity = new Vector2(direction * speedX, plRigi.velocity.y);
        animator.SetBool("isMove", plRigi.velocity.x != 0 ? true : false);
        
        if (plRigi.velocity.x != 0 && isOnGround)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            if (GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Stop();
            }
        }
        
        if (health < 0) health = 0;

        触地检测();
    }

    public void Movement(InputAction.CallbackContext context)
    {
        
        Vector2 val = context.ReadValue<Vector2>();
        direction = val.x;
    }


    
    private void followMouse()
    {
        Vector3 mosPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0,-0.05f,0);
        flag = mosPos.x > plRigi.position.x ? 1 : -1;
        
        Gun.instance.换弹进度条.transform.localScale = new Vector3(flag * Gun.instance.换弹进度条缩放.x,
            Gun.instance.换弹进度条缩放.y, Gun.instance.换弹进度条缩放.z);
        transform.localScale = new Vector3(flag, 1, 1);
        foreach (var a in arms)
        {
            Vector3 drc = (mosPos - a.transform.position).normalized;
            float angle = 450 + (flag==1?0:180) - Mathf.Atan2(drc.x, drc.y) * Mathf.Rad2Deg;
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
        if (Input.GetButtonDown("Jump") && 是否触地)
        {   
            animator.SetTrigger("jump");
            plRigi.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        //if (context.started && !isAttacking) isAttacking = true;
    }

    void 触地检测()
    {
        Vector3 终点 = new Vector3(0, -1, 0);
        Debug.DrawLine(transform.position, transform.position + 终点 * 触地射线检测长度, Color.red);
        if (Physics2D.Raycast(transform.position, 终点, 触地射线检测长度, LayerMask.GetMask("Ground")))
        {
            是否触地 = true;
        }
        else
        {
            是否触地 = false;
        }
        animator.SetBool("是否触地", 是否触地);
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
        animator.SetTrigger("掉血");
        Invoke("恢复颜色",受伤变色时间);
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
}
