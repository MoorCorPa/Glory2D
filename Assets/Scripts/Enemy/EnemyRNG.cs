using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRNG : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void attackAnim()
    {
        animator.SetTrigger("Attack");
    }

    public void resetAnim()
    {
        animator.ResetTrigger("attack");
    }

    public void takeDamage()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag + collision.gameObject.tag.Equals("Player"));
        if (collision.gameObject.tag.Equals("Player"))
        {
            attackAnim();
        }
    }
}
