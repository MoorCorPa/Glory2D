using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyRNG : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public void attackAnim()
    {
        transform.localScale = new Vector3(getFaceAt()?1:-1, 1, 1);
        animator.SetTrigger("Attack");
        
    }

    //µôÑª
    public void cause()
    {
        PlayerController.instance.takeDamage(damage);
    }
    
    public void activeAttack()
    {
        isAttacked = false;
    }

    public void resetAnim()
    {
        animator.ResetTrigger("Attack");
        isAttacked = true;
    }

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        
       // Debug.Log(collision.gameObject.tag + collision.gameObject.tag.Equals("Player"));
        if (collision.CompareTag("Player"))
        {
            attackAnim();
        }
    }*/
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isAttacked)
        {
            cause();
        }
    }
}
