using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy飞鼠 : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        animator.SetInteger("怪物ID", 1);
    }

    // Update is called once per frame
    void Update() 
    {
        base.Update();
    }

    void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
