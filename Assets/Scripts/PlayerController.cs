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

    private float speedX, jumpForce;

    private bool isOnGround;
    public bool isAttacking;

    private GameObject[] arms;
    public int flag;

    // Ѫ��
    public int health = 10000000;

    private void Awake()
    {
        instance = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        plRigi = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        arms = GameObject.FindGameObjectsWithTag("arms");

        speedX = 5f;
        jumpForce = 5f;

    }

    // Update is called once per frame
    void Update()
    {
        followMouse();
        Jump();

        plRigi.velocity = new Vector2(direction * speedX, plRigi.velocity.y);
        animator.SetBool("isMove", plRigi.velocity.x != 0 ? true : false);
    }

    public void Movement(InputAction.CallbackContext context)
    {
        Vector2 val = context.ReadValue<Vector2>();
        direction = val.x;
    }

    private void followMouse()
    {
        Vector3 mosPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        flag = mosPos.x > plRigi.position.x ? 1 : -1;
        foreach (var a in arms)
        {
            Vector3 drc = (mosPos - a.transform.position).normalized;
            float angle = 450 + (flag==1?0:180) - Mathf.Atan2(drc.x, drc.y) * Mathf.Rad2Deg;
            a.transform.eulerAngles = new Vector3(0, 0, angle);
        }

        transform.localScale = new Vector3(flag, 1, 1);

        /*foreach (var a in arms)
        {
            Vector3 display = Camera.main.WorldToScreenPoint(a.transform.position);
            Vector2 vector = Input.mousePosition - display;
            a.transform.up = vector;
        }*/

    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            animator.SetTrigger("jump");
            plRigi.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        //if (context.started && !isAttacking) isAttacking = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
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
    }

    void Grounding(Collision2D col, bool exitState)
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
            if (col.gameObject.layer == LayerMask.NameToLayer("Ground") && !isOnGround && contact0 == Vector2.up)
            {
                isOnGround = true;
                animator.ResetTrigger("jump");
            }
        }
        animator.SetBool("isOnGround", isOnGround);
    }

    // �ܵ��˺���Ѫ
    public void takeDamage(int damage)
    {
        Debug.Log(damage);
        health -= damage;
        Debug.Log(health);
    }
}
