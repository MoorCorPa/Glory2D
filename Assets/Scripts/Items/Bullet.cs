using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    private float destoryTime = 1f;
    private float startTime;

    public float force = 15f;
    public GameObject hitEffect;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        var flag = FindObjectOfType<PlayerController>().flag;
        if (flag<0)
        {
            float scaleX = transform.localScale.x;
            float scaleY = transform.localScale.y;
            float scaleZ = transform.localScale.z;
            transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rb.gravityScale = 1;

        string colname = collision.gameObject.name;
        if (!colname.Equals("bullet(Clone)") && !colname.Equals("Player"))
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
