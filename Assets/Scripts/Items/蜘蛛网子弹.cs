using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 蜘蛛网子弹 : BulletEnemy
{
    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
    }

    new void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("子弹") && !collision.CompareTag("Enemy"))
        {
            if (!是否触发)
            {
                是否触发 = true;
                if (collision.GetComponent<PlayerController>())
                {
                    collision.GetComponent<PlayerController>().TakeDamage(子弹伤害);
                }

                子弹刚体.velocity = new Vector2();
                transform.position = collision.ClosestPoint(transform.position);
            }
        }
    }
}
