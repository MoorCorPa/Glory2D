using UnityEngine;
using Random = UnityEngine.Random;

public class 掉落物 : MonoBehaviour
{
    private Rigidbody2D 掉落物刚体;
    public float 最大爆炸力;
    public float 最小爆炸力;
    void Start()
    {
        掉落物刚体 = GetComponent<Rigidbody2D>();
       // InvokeRepeating("丢", 0, 1);
       丢();
    }

    
    public void 丢()
    {
        var 爆炸力 = Random.Range(最小爆炸力, 最大爆炸力);
        var 方向 = new Vector2(Random.Range(-1f, 1f), 1);
        掉落物刚体.AddForce( 方向 *爆炸力, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.collider.CompareTag("掉落物")) 掉落物刚体.velocity = Vector2.zero;
        if (other.collider.CompareTag("Player"))
        {
            Destroy(gameObject);
            other.collider.GetComponent<PlayerController>().水晶增加(1);
        }
    }
}
