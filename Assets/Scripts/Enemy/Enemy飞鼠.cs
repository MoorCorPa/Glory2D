using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy飞鼠 : EnemyFly
{

    bool 攻击成功 = false;

    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update() 
    {
        base.Update();
        攻击成功处理();
    }

    void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void 攻击成功处理()
    {
        if (攻击成功)
        {
            PlayerController.instance.TakeDamage(攻击力);
            攻击成功 = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 攻击成功 = true;
    }
}
