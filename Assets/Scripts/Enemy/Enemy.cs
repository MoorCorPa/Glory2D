using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Animator animator;

    public float health;
    public float damage;

    public float moveSpeed;
    public float moveTime;
    public float moveColldown;

    public float attackTime;
    public float attackColldown;

    public Collider2D triggerColl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Death
        if (health <= 0)
        {
            Destroy(gameObject);
        }


    }
}
