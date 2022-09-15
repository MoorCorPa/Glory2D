using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float destoryTime = 1f;
    private float startTime;
    public float force = 20f;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        startTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = PlayerController.instance.flag*transform.right * force;
        Physics2D.IgnoreLayerCollision(8, 9);
    }

    void Update()
    {
        if (Time.time - startTime > destoryTime)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.gravityScale = 1;

        string colname = collision.gameObject.name;
        if (!colname.Equals("bullet(Clone)") && !colname.Equals("Player"))
        {
            Destroy(this.gameObject);
        }
        
    }
}