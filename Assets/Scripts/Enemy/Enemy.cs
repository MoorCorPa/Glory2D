using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Min(0)] public int 最大血量;
    [Min(0)] public int 当前血量;
    [Min(0)] public int 攻击力;
    [Min(0f)] public float 移动速度;

    public GameObject 掉落物;

    public virtual void 掉血(int 伤害)
    {
    }

    public void 销毁()
    {
        Instantiate(掉落物, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}